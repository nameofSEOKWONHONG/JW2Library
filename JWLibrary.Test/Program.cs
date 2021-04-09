using System;
using JWLibrary.Core;
using JWLibrary.DI;
using JWLibrary.ServiceExecutor;
using JWService.WeatherForecast;
using Microsoft.Extensions.DependencyInjection;
using Service.Data;
using Service.WeatherForecast;

namespace JCoreSvcTest {
    public class Program {
        public static void Main(string[] args) {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddTransient<IGetWeatherForecastSvc, GetWeatherForecastSvc>()
                .BuildServiceProvider();


            ServiceLocator.SetLocatorProvider(serviceProvider);

            var request = new JList<WeatherForecastRequestDto>() {
                new WeatherForecastRequestDto() {
                    ID = 2003,
                },
                new WeatherForecastRequestDto() {
                    ID = 2004
                }
            };

            JList<WEATHER_FORECAST> result = new JList<WEATHER_FORECAST>();
            using (var svc =
                new BulkServiceExecutorManager<IGetWeatherForecastSvc, WeatherForecastRequestDto>(request)) {
                svc.SetRequest((s, r) => s.Request = r)
                    .AddFilter(s => s.Request.ID.jIsNotNull())
                    .OnExecuted(s => {
                        result.Add(s.Result);
                    });
            }
            
            result.jForEach(item => {
                Console.WriteLine(item.jObjectToJson());
            });
        }
    }
}