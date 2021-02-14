namespace Service.WeatherForecast {
    using Dapper;
    using FluentValidation;
    using JWLibrary.Core;
    using JWLibrary.Database;
    using JWLibrary.ServiceExecutor;
    using JWService.WeatherForecast;
    using Service.Data;
    using System.Data.SqlClient;

    public class SaveWeatherForecastSvc : ServiceExecutor<SaveWeatherForecastSvc, WEATHER_FORECAST, int>, ISaveWeatherForecastSvc {
        private WEATHER_FORECAST _exists = null;
        private IGetWeatherForecastSvc _getWeatherForecastSvc;
        public SaveWeatherForecastSvc(IGetWeatherForecastSvc getWeatherForecastSvc) {
            base.SetValidator(new Validator());
            this._getWeatherForecastSvc = getWeatherForecastSvc;
        }

        public override bool PreExecute() {
            using var executor = new ServiceExecutorManager<IGetWeatherForecastSvc>(this._getWeatherForecastSvc);
            executor.SetRequest(o => o.Request = new WeatherForecastRequestDto() { ID = this.Request.ID })
                .OnExecuted(o => {
                    this._exists = o.Result;
                });

            if (this._exists.jIsNotNull()) return true;
            return false;
        }

        public override void Execute() {
                JDataBase.Resolve<SqlConnection>()
                    .DbExecutor<int>(db => {
                        if (this._exists.jIsNotNull()) {
                            this.Result = db.Update<WEATHER_FORECAST>(this.Request);
                        }
                        this.Result = db.Insert<WEATHER_FORECAST>(this.Request).Value;
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