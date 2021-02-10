using SqlKata.Compilers;
using SqlKata.Execution;

namespace Service.WeatherForecast {
    using FluentValidation;
    using JWLibrary.Core;
    using JWLibrary.Database;
    using JWLibrary.ServiceExecutor;
    using JWService.WeatherForecast;
    using Service.Data;
    using SqlKata;
    using System.Data.SqlClient;

    public class GetWeatherForecastSvc : ServiceExecutor<GetWeatherForecastSvc, WeatherForecastRequestDto, WEATHER_FORECAST>, IGetWeatherForecastSvc {
        public GetWeatherForecastSvc() {
            base.SetValidator(new Validator());
        }
        public override void Execute() {
            //use sqlkata
            var query = new Query("WEATHER_FORECAST").Where("ID", this.Request.ID).Select("*");
            this.Result = JDataBase.Resolve<SqlConnection>()
                .DbContainer(con => {
                    var compiler = new SqlServerCompiler();
                    var db = new QueryFactory(con, compiler);
                    var weather = db.Query("dbo.WEATHER_FORECAST").Where("ID", this.Request.ID).FirstOrDefault<WEATHER_FORECAST>();
                    weather.TEMPERATURE_F = 32 + (int)(weather.TEMPERATURE_C / 0.5556);
                    return weather;
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