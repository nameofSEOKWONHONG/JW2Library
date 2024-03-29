﻿using System;
using System.Collections.Generic;
using eXtensionSharp;
using JWLibrary.DI;
using JWLibrary.ServiceExecutor;
using JWService.WeatherForecast;
using Microsoft.Extensions.DependencyInjection;
using Service.Data;
using Service.WeatherForecast;

namespace JCoreSvcTest {
    /// <summary>
    /// 
    /// </summary>
    public class Program {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args) {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddTransient<IGetWeatherForecastSvc, GetWeatherForecastSvc>()
                .BuildServiceProvider();


            ServiceLocator.SetLocatorProvider(serviceProvider);

            var request = new List<WeatherForecastRequestDto> {
                new() {
                    ID = 2003
                },
                new() {
                    ID = 2004
                }
            };

            var result = new List<WEATHER_FORECAST>();
            using (var svc =
                new BulkServiceExecutorManager<IGetWeatherForecastSvc, WeatherForecastRequestDto>(request)) {
                svc.SetRequest((s, r) => s.Request = r)
                    .AddFilter(s => s.Request.ID.xIsNotNull())
                    .OnExecuted(s => {
                        result.Add(s.Result);
                        return true;
                    });
            }

            result.xForEach(item => { Console.WriteLine(item.xToJson()); });
        }
    }
}