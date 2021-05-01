using System;
using eXtensionSharp;
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

            var request = new XList<WeatherForecastRequestDto> {
                new() {
                    ID = 2003
                },
                new() {
                    ID = 2004
                }
            };

            var result = new XList<WEATHER_FORECAST>();
            using (var svc =
                new BulkServiceExecutorManager<IGetWeatherForecastSvc, WeatherForecastRequestDto>(request)) {
                svc.SetRequest((s, r) => s.Request = r)
                    .AddFilter(s => s.Request.ID.xIsNotNull())
                    .OnExecuted(s => {
                        result.Add(s.Result);
                        return true;
                    });
            }

            result.xForEach(item => { Console.WriteLine(item.xObjectToJson()); });
        }
    }
}