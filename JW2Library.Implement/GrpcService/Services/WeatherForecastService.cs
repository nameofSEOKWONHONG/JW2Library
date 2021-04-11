using System.Threading.Tasks;
using Grpc.Core;
using JWLibrary.ServiceExecutor;
using JWService.WeatherForecast;
using Microsoft.Extensions.Logging;
using Service.Data;
using Service.WeatherForecast;
using WeatherForecastService;

namespace GrpcService {
    public class WeatherForecastService : WeatherForecastor.WeatherForecastorBase {
        private readonly ILogger<WeatherForecastService> _logger;

        public WeatherForecastService(ILogger<WeatherForecastService> logger) {
            _logger = logger;
            _logger.LogInformation("instance create");
        }

        public override Task<WeatherForecastResponse> GetWeatherForecast(WeatherForecastRequest request,
            ServerCallContext context) {
            WEATHER_FORECAST result = null;
            using var executor = new ServiceExecutorManager<IGetWeatherForecastSvc>(new GetWeatherForecastSvc());
            executor.SetRequest(o => o.Request = new WeatherForecastRequestDto {ID = request.Id})
                .OnExecuted(o => {
                    result = o.Result;
                    return true;
                });
            return Task.FromResult(new WeatherForecastResponse {
                Id = result.ID,
                Date = result.DATE.ToString(),
                TemperatureC = result.TEMPERATURE_C.Value,
                TemperatureF = result.TEMPERATURE_F.Value,
                Summary = result.SUMMARY
            });
        }
    }
}