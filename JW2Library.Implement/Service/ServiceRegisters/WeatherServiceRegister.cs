using JWService.WeatherForecast;
using Microsoft.Extensions.DependencyInjection;

namespace Service.WeatherForecast {
    public class WeatherServiceRegister : IServiceRegister {
        public void ServiceRegistry(IServiceCollection services) {
            services.AddScoped<IGetWeatherForecastSvc, GetWeatherForecastSvc>();
            services.AddScoped<IGetAllWeatherForecastSvc, GetAllWeatherForecastSvc>();
            services.AddScoped<ISaveWeatherForecastSvc, SaveWeatherForecastSvc>();
            services.AddScoped<IDeleteWeatherForecastSvc, DeleteWeatherForecastSvc>();
        }
    }
}