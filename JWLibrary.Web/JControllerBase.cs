using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eXtensionSharp;
using JWLibrary.DI;
using JWLibrary.ServiceExecutor;
using JWLibrary.Util.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JWLibrary.Web {
    [ApiController]
    [Route("api/[controller]/[action]")] //url version route
    public class JControllerBase<TController> : ControllerBase, IDisposable
        where TController : class {
        protected ISessionContext Context = ServiceLocator.Current.GetInstance<ISessionContext>();
        protected ILogger<TController> Logger;

        public JControllerBase(ILogger<TController> logger) {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            Logger = logger;
        }

        public virtual void Dispose() {
        }

        /// <summary>
        ///     single execute
        /// </summary>
        /// <param name="serviceExecutor"></param>
        /// <param name="request"></param>
        /// <param name="func"></param>
        /// <typeparam name="TServiceExecutor"></typeparam>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        protected TResult CreateService<TServiceExecutor, TRequest, TResult>
            (TServiceExecutor serviceExecutor, TRequest request, Func<TServiceExecutor, bool> func = null)
            where TServiceExecutor : IServiceExecutor<TRequest, TResult> {
            var result = default(TResult);
            using var executor = new ServiceExecutorManager<TServiceExecutor>(serviceExecutor);
            executor.SetRequest(o => o.Request = request)
                .AddFilter(o => func.xIsNotNull() ? func(serviceExecutor) : true)
                .OnExecuted(o => {
                    result = o.Result;
                    return true;
                });
            return result;
        }

        protected async Task<TResult> CreateServiceAsync<TServiceExecutor, TRequest, TResult>
            (TServiceExecutor serviceExecutor, TRequest request, Func<TServiceExecutor, bool> func = null)
            where TServiceExecutor : IServiceExecutor<TRequest, TResult> {
            var result = default(TResult);
            using var executor = new ServiceExecutorManager<TServiceExecutor>(serviceExecutor);
            await executor.SetRequest(o => o.Request = request)
                .AddFilter(o => func.xIsNotNull() ? func(serviceExecutor) : true)
                .OnExecutedAsync(async o => {
                    result = o.Result;
                    return true;
                });
            return result;
        }

        protected IEnumerable<TResult> CreateBulkService<TServiceExecutor, TRequest, TResult>(
            TServiceExecutor serviceExecutor,
            IEnumerable<TRequest> requests, Func<TServiceExecutor, bool> func = null)
            where TServiceExecutor : IServiceExecutor<TRequest, TResult> {
            var results = new XList<TResult>();
            using var bulkExecutor = new BulkServiceExecutorManager<TServiceExecutor, TRequest>(requests);
            bulkExecutor.SetRequest((o, c) => o.Request = c)
                .AddFilter(func)
                .OnExecuted(o => {
                    results.Add(o.Result);
                    return true;
                });

            return results;
        }
    }

    /// <summary>
    ///     base controller
    /// </summary>

    //[Route("api/[controller]/[action]")] //normal route
    [Route("api/{v:apiVersion}/[controller]/[action]")] //url version route
    public class JVersionControllerBase<TController> : JControllerBase<TController>, IDisposable
        where TController : class {
        public JVersionControllerBase(ILogger<TController> logger) : base(logger) {
        }

        public override void Dispose() {
            //your code...

            base.Dispose();
        }
    }
}