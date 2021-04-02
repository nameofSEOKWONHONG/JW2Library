using System;
using System.Threading.Tasks;
using JWLibrary.Core;
using JWLibrary.ServiceExecutor;
using JWLibrary.Util.Cache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JWLibrary.Web {
    /// <summary>
    ///     base controller
    /// </summary>
    [ApiController]
    //[Route("api/[controller]/[action]")] //normal route
    [Route("api/{v:apiVersion}/[controller]/[action]")] //url version route
    public class JControllerBase<TController> : ControllerBase, IDisposable
        where TController : class {
        protected ILogger<TController> Logger;
        protected ISessionContext Context = ServiceLocator.Current.GetInstance<ISessionContext>();

        public JControllerBase(ILogger<TController> logger) {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            this.Logger = logger;
        }

        public void Dispose() {
        }

        protected TResult CreateService<TServiceExecutor, TRequest, TResult>
            (TServiceExecutor serviceExecutor, TRequest request, Func<TServiceExecutor, bool> func = null)
            where TServiceExecutor : IServiceExecutor<TRequest, TResult> {
            var result = default(TResult);
            using var executor = new ServiceExecutorManager<TServiceExecutor>(serviceExecutor);
            executor.SetRequest(o => o.Request = request)
                .AddFilter(o => func.jIsNotNull() ? func(serviceExecutor) : true)
                .OnExecuted(o => { result = o.Result; });
            return result;
        }

        protected async Task<TResult> CreateServiceAsync<TServiceExecutor, TRequest, TResult>
            (TServiceExecutor serviceExecutor, TRequest request, Func<TServiceExecutor, bool> func = null)
            where TServiceExecutor : IServiceExecutor<TRequest, TResult> {
            var result = default(TResult);
            using var executor = new ServiceExecutorManager<TServiceExecutor>(serviceExecutor);
            await executor.SetRequest(o => o.Request = request)
                .AddFilter(o => func.jIsNotNull() ? func(serviceExecutor) : true)
                .OnExecutedAsync(o => {
                    result = o.Result;
                    return Task.CompletedTask;
                });
            return result;
        }
    }

    public interface ISessionContext {
        CacheManager CacheManager { get; set; }
    }

    public class SessionContext : ISessionContext {
        public CacheManager CacheManager { get; set; } = new CacheManager();
    }
}