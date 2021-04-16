using System.Data.SqlClient;
using Dapper;
using FluentValidation;
using JWLibrary.Core;
using JWLibrary.Database;
using JWLibrary.ServiceExecutor;
using Service.Data;

namespace JWService.WeatherForecast {
    public class DeleteWeatherForecastSvc : ServiceExecutor<DeleteWeatherForecastSvc, WeatherForecastRequestDto, bool>,
        IDeleteWeatherForecastSvc {
        private readonly IGetWeatherForecastSvc _getWeatherForecastSvc;
        private WEATHER_FORECAST _removeObj;

        public DeleteWeatherForecastSvc(IGetWeatherForecastSvc getWeatherForecastSvc) {
            _getWeatherForecastSvc = getWeatherForecastSvc;
        }

        public override bool PreExecute() {
            using var executor = new ServiceExecutorManager<IGetWeatherForecastSvc>(_getWeatherForecastSvc);
            executor.SetRequest(o => o.Request = new WeatherForecastRequestDto {ID = Request.ID})
                .OnExecuted(o => {
                    _removeObj = o.Result;
                    return true;
                });

            if (_removeObj.isNotNull()) return true;
            return false;
        }

        public override void Execute() {
            JDatabaseResolver.Resolve<SqlConnection>().DbExecutor<bool>(db => { Result = db.Delete(_removeObj) > 0; });
        }

        public class Validator : AbstractValidator<DeleteWeatherForecastSvc> {
        }
    }
}