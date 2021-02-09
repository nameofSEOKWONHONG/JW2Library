using JWService.WeatherForecast;
using Microsoft.Extensions.DependencyInjection;
using Service.Accounts;
using Service.Data;
using Service.WeatherForecast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIServer.Config {
    public static class ServiceLoader {
        public static void AccountServiceLoader(this IServiceCollection services) {
            services.AddScoped<IGetAccountSvc, GetAccountSvc>();
            services.AddScoped<IGetAccountsSvc, GetAccountsSvc>();
            services.AddScoped<IDeleteAccountSvc, DeleteAccountSvc>();
            services.AddScoped<ISaveAccountSvc, SaveAccountSvc>();
            services.AddScoped<IGetAccountByIdSvc, GetAccountByIdSvc>();
        }

        public static void WeatherServiceLoader(this IServiceCollection services) {
            services.AddScoped<IGetWeatherForecastSvc, GetWeatherForecastSvc>();
            services.AddScoped<IGetAllWeatherForecastSvc, GetAllWeatherForecastSvc>();
            services.AddScoped<ISaveWeatherForecastSvc, SaveWeatherForecastSvc>();
            services.AddScoped<IDeleteWeatherForecastSvc, DeleteWeatherForecastSvc>();
        }
    }
}
