using JWLibrary.ApiCore.Config;
using JWLibrary.Core;
using JWLibrary.ServiceExecutor;
using JWLibrary.Web;
using JWService.WeatherForecast;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;

namespace JWLibrary.ApiCore.Controllers {
    /// <summary>
    ///     WeatherForecastController
    ///     **no more use biner**
    ///     ref : http://www.binaryintellect.net/articles/03f580c4-84ad-4d78-847f-43103b4e4691.aspx
    /// </summary>
    [ApiVersion("0.0")]
    public class WeatherForecastController : JControllerBase<WeatherForecastController> {
        private readonly IGetWeatherForecastSvc    _getWeatherForecastSvc;
        private readonly IGetAllWeatherForecastSvc _getAllWeatherForecastSvc;
        private readonly ISaveWeatherForecastSvc   _saveWeatherForecastSvc;
        private readonly IDeleteWeatherForecastSvc _deleteWeatherForecastSvc;
        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IGetWeatherForecastSvc getWeatherForecastSvc,
            IGetAllWeatherForecastSvc getAllWeatherForecastSvc,
            ISaveWeatherForecastSvc saveWeatherForecastSvc,
            IDeleteWeatherForecastSvc deleteWeatherForecastSvc
            )
            : base(logger) {
            _getWeatherForecastSvc = getWeatherForecastSvc;
            _getAllWeatherForecastSvc = getAllWeatherForecastSvc;
            _saveWeatherForecastSvc = saveWeatherForecastSvc;
            _deleteWeatherForecastSvc = deleteWeatherForecastSvc;
        }

        /// <summary>
        ///     get
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "idx" })]
        [Transaction(TransactionScopeOption.Suppress)]
        public async Task<WEATHER_FORECAST> GetWeather(int idx = 1) {
            WEATHER_FORECAST result = null;
            using var executor = new ServiceExecutorManager<IGetWeatherForecastSvc>(this._getWeatherForecastSvc);
            await executor.SetRequest(o => o.Request = new WeatherForecastRequestDto() { ID = idx })
                .OnExecutedAsync(o => {
                    result = o.Result;
                });
            return result;
        }

        /// <summary>
        ///     get all
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        [Transaction(TransactionScopeOption.Suppress)]
        public async Task<IEnumerable<WEATHER_FORECAST>> GetWeathers() {
            IEnumerable<WEATHER_FORECAST> result = null;
            using var executor = new ServiceExecutorManager<IGetAllWeatherForecastSvc>(this._getAllWeatherForecastSvc);
            await executor.SetRequest(o => o.Request = new WeatherForecastRequestDto())
                .OnExecutedAsync(o => result = o.Result);
            return result;
        }

        /// <summary>
        ///     Post메서드
        /// </summary>
        /// <param name="request">요청:RequestDto<TestRequestDto></param>
        /// <returns></returns>
        [HttpPost]
        [Transaction(TransactionScopeOption.Required)]
        public async Task<int> SaveWeather(RequestDto<WEATHER_FORECAST> request) {
            var result = 0;
            using var executor = new ServiceExecutorManager<ISaveWeatherForecastSvc>(this._saveWeatherForecastSvc);
            await executor.SetRequest(o => o.Request = request.Data)
                .OnExecutedAsync(o => result = o.Result);
            return result;
        }

        /// <summary>
        ///     remove
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Transaction(TransactionScopeOption.Required)]
        public async Task<bool> RemoveWeather(RequestDto<WeatherForecastRequestDto> request) {
            var result = false;
            using var executor = new ServiceExecutorManager<IDeleteWeatherForecastSvc>(this._deleteWeatherForecastSvc);
            await executor.SetRequest(o => o.Request = request.Data)
                .OnExecutedAsync(o => result = o.Result);
            return result;
        }
    }
}