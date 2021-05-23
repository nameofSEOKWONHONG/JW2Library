using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using APIServer.Config;
using JWLibrary;
using JWLibrary.ServiceExecutor;
using JWLibrary.Web;
using JWService.WeatherForecast;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Data;

namespace APIServer.Controllers {
    /// <summary>
    ///     WeatherForecastController
    ///     **no more use biner**
    ///     ref : http://www.binaryintellect.net/articles/03f580c4-84ad-4d78-847f-43103b4e4691.aspx
    /// </summary>
    [ApiVersion("0.0")]
    public class WeatherForecastController : JVersionControllerBase<WeatherForecastController> {
        private readonly IDeleteWeatherForecastSvc _deleteWeatherForecastSvc;
        private readonly IGetAllWeatherForecastSvc _getAllWeatherForecastSvc;
        private readonly IGetWeatherForecastSvc _getWeatherForecastSvc;
        private readonly ISaveWeatherForecastSvc _saveWeatherForecastSvc;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IGetWeatherForecastSvc getWeatherForecastSvc,
            IGetAllWeatherForecastSvc getAllWeatherForecastSvc,
            ISaveWeatherForecastSvc saveWeatherForecastSvc,
            IDeleteWeatherForecastSvc deleteWeatherForecastSvc)
            : base(logger) {
            //_getWeatherForecastSvc = serviceAccessor("A");
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
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] {"idx"})]
        [Transaction(TransactionScopeOption.Suppress)]
        public async Task<WEATHER_FORECAST> GetWeather(int idx = 1) {
            WEATHER_FORECAST result = null;
            using var executor = new ServiceExecutorManager<IGetWeatherForecastSvc>(_getWeatherForecastSvc);
            await executor.SetRequest(o => o.Request = new WeatherForecastRequestDto {ID = idx})
                .OnExecutedAsync(async o => {
                    result = o.Result;
                    return true;
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
            using var executor = new ServiceExecutorManager<IGetAllWeatherForecastSvc>(_getAllWeatherForecastSvc);
            await executor.SetRequest(o => o.Request = new WeatherForecastRequestDto())
                .OnExecutedAsync(async o => {
                    result = o.Result;
                    return true;
                });
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
            using var executor = new ServiceExecutorManager<ISaveWeatherForecastSvc>(_saveWeatherForecastSvc);
            await executor.SetRequest(o => o.Request = request.Data)
                .OnExecutedAsync(async o => {
                    result = o.Result;
                    return true;
                });
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
            using var executor = new ServiceExecutorManager<IDeleteWeatherForecastSvc>(_deleteWeatherForecastSvc);
            await executor.SetRequest(o => o.Request = request.Data)
                .OnExecutedAsync(async o => result = o.Result);
            return result;
        }
    }
}