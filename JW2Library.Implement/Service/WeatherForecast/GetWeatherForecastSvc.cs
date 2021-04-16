using System.Data.SqlClient;
using FluentValidation;
using JWLibrary.Database;
using JWLibrary.ServiceExecutor;
using JWService.WeatherForecast;
using Service.Data;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Service.WeatherForecast {
    public class GetWeatherForecastSvc :
        ServiceExecutor<GetWeatherForecastSvc, WeatherForecastRequestDto, WEATHER_FORECAST>, IGetWeatherForecastSvc {
        public GetWeatherForecastSvc() {
            base.SetValidator(new Validator());
        }

        public override void Execute() {
            //use sqlkata
            var query = new Query("WEATHER_FORECAST").Where("ID", Request.ID).Select("*");
            JDatabaseResolver.Resolve<SqlConnection>()
                .DbExecutor<WEATHER_FORECAST>(con => {
                    var compiler = new SqlServerCompiler();
                    var db = new QueryFactory(con, compiler);
                    var weather = db.Query("dbo.WEATHER_FORECAST").Where("ID", Request.ID)
                        .FirstOrDefault<WEATHER_FORECAST>();
                    weather.TEMPERATURE_F = 32 + (int) (weather.TEMPERATURE_C / 0.5556);
                    Result = weather;
                });
        }

        public class Validator : AbstractValidator<GetWeatherForecastSvc> {
            public Validator() {
                RuleFor(o => o.Request).NotNull();
                RuleFor(o => o.Request.ID).GreaterThan(0);
            }
        }
    }
}