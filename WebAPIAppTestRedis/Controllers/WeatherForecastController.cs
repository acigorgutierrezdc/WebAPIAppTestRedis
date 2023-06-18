using Microsoft.AspNetCore.Mvc;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;

namespace WebAPIAppTestRedis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            ConfigurationOptions options = new ConfigurationOptions
            {
                //list of available nodes of the cluster along with the endpoint port.
                EndPoints = {
                                { "localhost", 6379 }
                            },

                            AbortOnConnectFail = false

                            };

            ConnectionMultiplexer cluster = ConnectionMultiplexer.Connect(options);
            IDatabase db = cluster.GetDatabase();

            db.StringSet("foo", "bar");
            Console.WriteLine(db.StringGet("foo")); // prints bar



            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}