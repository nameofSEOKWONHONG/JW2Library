using JWService.WeatherForecast;
using Microsoft.Extensions.DependencyInjection;

namespace Service.WeatherForecast {
    public static class ServiceLoader {
        public static void WeatherServiceLoader(this IServiceCollection services) {
            services.AddScoped<IGetWeatherForecastSvc, GetWeatherForecastSvc>();
            services.AddScoped<IGetAllWeatherForecastSvc, GetAllWeatherForecastSvc>();
            services.AddScoped<ISaveWeatherForecastSvc, SaveWeatherForecastSvc>();
            services.AddScoped<IDeleteWeatherForecastSvc, DeleteWeatherForecastSvc>();
            // services.AddTransient<ServiceResolver>(serviceProvider => key => {
            //     return _keyValuePair.First(m => m.Key == key).Value(serviceProvider);
            // });
        }

        // public delegate IServiceBase ServiceResolver(string key);
        //
        // public static Dictionary<string, Func<IServiceProvider, IServiceBase>> _keyValuePair =
        //     new Dictionary<string, Func<IServiceProvider, IServiceBase>>() {
        //         {
        //             "A", provider => { return provider.GetService<GetWeatherForecastSvc>(); }
        //         }
        //     };
    }
}