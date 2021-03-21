using System.Collections.Generic;
using JWLibrary.ServiceExecutor;
using Service.Data;

namespace JWService.WeatherForecast {
    public interface IGetWeatherForecastSvc : IServiceExecutor<WeatherForecastRequestDto, WEATHER_FORECAST> {
    }

    public interface
        IGetAllWeatherForecastSvc : IServiceExecutor<WeatherForecastRequestDto, IEnumerable<WEATHER_FORECAST>> {
    }

    public interface ISaveWeatherForecastSvc : IServiceExecutor<WEATHER_FORECAST, int> {
    }

    public interface IDeleteWeatherForecastSvc : IServiceExecutor<WeatherForecastRequestDto, bool> {
    }
}