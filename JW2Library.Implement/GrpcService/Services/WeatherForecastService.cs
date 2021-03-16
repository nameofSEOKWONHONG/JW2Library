using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using WeatherForecastService;

namespace GrpcService {
    public class WeatherForecastService : WeatherForecastor.WeatherForecastorBase {
        private readonly ILogger<WeatherForecastService> _logger;
        public WeatherForecastService(ILogger<WeatherForecastService> logger) {
            _logger = logger;
            _logger.LogInformation("instance create");
        }
        public override Task<WeatherForecastResponse> GetWeatherForecast(WeatherForecastRequest request, ServerCallContext context) {
            return Task.FromResult(new WeatherForecastResponse {
                Date = DateTime.Now.ToString(),
                Id = 1,
                TemperatureC = 50.2,
                TemperatureF = 32.2
            });
        }
    }
}