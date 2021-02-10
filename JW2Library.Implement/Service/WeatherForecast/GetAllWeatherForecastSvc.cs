namespace JWService.WeatherForecast {
    using System.Linq;
    using Dapper;
    using SqlKata.Compilers;
    using SqlKata.Execution;    
    using FluentValidation;
    using JWLibrary.Core;
    using JWLibrary.Database;
    using JWLibrary.ServiceExecutor;
    using Service.Data;
    using SqlKata;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    public class GetAllWeatherForecastSvc : ServiceExecutor<GetAllWeatherForecastSvc, WeatherForecastRequestDto, IEnumerable<WEATHER_FORECAST>>, IGetAllWeatherForecastSvc {
        public override void Execute() {
            this.Result = JDataBase.Resolve<SqlConnection>()
                .DbContainer(con => {
                    var compiler = new SqlServerCompiler();
                    var db = new QueryFactory(con, compiler);
                    var weathers = db.Query("dbo.WEATHER_FORECAST").Get<WEATHER_FORECAST>();
                    weathers.jForEach(item => {
                        item.TEMPERATURE_F = 32 + (int)(item.TEMPERATURE_C / 0.5556);
                        return true;
                    });

                    return weathers;
                });
        }

        public class Validator : AbstractValidator<GetAllWeatherForecastSvc> {
            public Validator() {

            }
        }
    }
}