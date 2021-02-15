using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using JWLibrary.Core;
using JWLibrary.ServiceExecutor;

namespace JWLibrary.Web {
    /// <summary>
    /// base controller
    /// </summary>
    [ApiController]
    //[Route("api/[controller]/[action]")] //normal route
    [Route("api/{v:apiVersion}/[controller]/[action]")] //url version route
    public class JControllerBase<TController> : ControllerBase, IDisposable
        where TController : class {
        protected ILogger<TController> Logger;

        public JControllerBase(ILogger<TController> logger)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            this.Logger = logger;
        }

        protected async Task<TResult> ExecuteServiceAsync<TServiceExecutor, TRequest, TResult>
            (TServiceExecutor serviceExecutor, TRequest request, Func<TServiceExecutor, bool> func = null) where TServiceExecutor : IServiceExecutor<TRequest, TResult> {
            TResult result = default(TResult);
            using var executor = new ServiceExecutorManager<TServiceExecutor>(serviceExecutor);
            await executor.SetRequest(o => o.Request = request)
                .AddFilter(o => func.jIsNotNull() ? func(serviceExecutor) : true)
                .OnExecutedAsync(async o => {
                    result = o.Result;
                });
            return result;
        }

        public void Dispose() {

        }
    }
}
