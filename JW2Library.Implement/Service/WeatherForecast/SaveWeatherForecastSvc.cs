using System.Data.SqlClient;
using Dapper;
using FluentValidation;
using JWLibrary.Core;
using JWLibrary.Database;
using JWLibrary.ServiceExecutor;
using JWService.WeatherForecast;
using Service.Data;

namespace Service.WeatherForecast {
    public class SaveWeatherForecastSvc : ServiceExecutor<SaveWeatherForecastSvc, WEATHER_FORECAST, int>,
        ISaveWeatherForecastSvc {
        private readonly IGetWeatherForecastSvc _getWeatherForecastSvc;
        private WEATHER_FORECAST _exists;

        public SaveWeatherForecastSvc(IGetWeatherForecastSvc getWeatherForecastSvc) {
            base.SetValidator(new Validator());
            _getWeatherForecastSvc = getWeatherForecastSvc;
        }

        public override bool PreExecute() {
            using var executor = new ServiceExecutorManager<IGetWeatherForecastSvc>(_getWeatherForecastSvc);
            executor.SetRequest(o => o.Request = new WeatherForecastRequestDto {ID = Request.ID})
                .OnExecuted(o => {
                    _exists = o.Result;
                    return true;
                });

            if (_exists.jIsNotNull()) return true;
            return false;
        }

        public override void Execute() {
            JDatabaseResolver.Resolve<SqlConnection>()
                .DbExecutor<int>(db => {
                    if (_exists.jIsNotNull()) Result = db.Update(Request);
                    Result = db.Insert(Request).Value;
                });
        }

        public class Validator : AbstractValidator<SaveWeatherForecastSvc> {
            public Validator() {
                RuleFor(o => o.Request).NotNull();
                RuleFor(o => o.Request.ID).GreaterThan(0);
            }
        }
    }
}