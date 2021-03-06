﻿using System.Collections.Generic;
using System.Data.SqlClient;
using eXtensionSharp;
using FluentValidation;
using JWLibrary.Database;
using JWLibrary.ServiceExecutor;
using Service.Data;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace JWService.WeatherForecast {
    public class GetAllWeatherForecastSvc :
        ServiceExecutor<GetAllWeatherForecastSvc, WeatherForecastRequestDto, IEnumerable<WEATHER_FORECAST>>,
        IGetAllWeatherForecastSvc {
        public override void Execute() {
            JDatabaseResolver.Resolve<SqlConnection>()
                .DbExecutor(con => {
                    var compiler = new SqlServerCompiler();
                    var db = new QueryFactory(con, compiler);
                    var weathers = db.Query("dbo.WEATHER_FORECAST").Get<WEATHER_FORECAST>();
                    weathers.xForEach(item => { item.TEMPERATURE_F = 32 + (int) (item.TEMPERATURE_C / 0.5556); });

                    Result = weathers;
                });
        }

        public class Validator : AbstractValidator<GetAllWeatherForecastSvc> {
        }
    }
}