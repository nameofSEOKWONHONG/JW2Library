namespace JWService.WeatherForecast {
    using JWLibrary.ServiceExecutor;
    using Service.Data;
    using System.Collections.Generic;

    public interface IGetWeatherForecastSvc : IServiceExecutor<WeatherForecastRequestDto, WEATHER_FORECAST> { }

    public interface IGetAllWeatherForecastSvc : IServiceExecutor<WeatherForecastRequestDto, IEnumerable<WEATHER_FORECAST>> { }

    public interface ISaveWeatherForecastSvc : IServiceExecutor<WEATHER_FORECAST, int> { }

    public interface IDeleteWeatherForecastSvc : IServiceExecutor<WeatherForecastRequestDto, bool> { }
}