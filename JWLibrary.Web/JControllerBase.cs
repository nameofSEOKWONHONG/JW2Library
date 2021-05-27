using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eXtensionSharp;
using JWLibrary.DI;
using JWLibrary.ServiceExecutor;
using JWLibrary.Util.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using ILogger = NLog.ILogger;

namespace JWLibrary.Web {
    [ApiController]
    public abstract class JControllerBase<T> : ControllerBase, IDisposable  where T : class{
        protected ILogger CLogger;
        protected ILogger<T> BaseLogger;

        public JControllerBase(ILogger<T> logger, ISessionContext context = null) {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            this.BaseLogger = logger;
            CLogger = LogManager.GetLogger("Log");
            if (!CLogger.Factory.Configuration.Variables.Keys.Contains("runtime"))
            {
                CLogger.Factory.Configuration.Variables.Add("runtime", "test");    
                CLogger.Factory.ReconfigExistingLoggers();
            }
        }
        
                /// <summary>
        /// 서비스 생성 메서드
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

        /// <summary>
        /// 비동기 서비스 생성 메서드
        /// </summary>
        /// <param name="serviceExecutor"></param>
        /// <param name="request"></param>
        /// <param name="func"></param>
        /// <typeparam name="TServiceExecutor"></typeparam>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        protected async Task<TResult> CreateServiceAsync<TServiceExecutor, TRequest, TResult>
            (TServiceExecutor serviceExecutor, TRequest request, Func<TServiceExecutor, bool> func = null)
            where TServiceExecutor : IServiceExecutor<TRequest, TResult> {
            var result = default(TResult);
            using var executor = new ServiceExecutorManager<TServiceExecutor>(serviceExecutor);
            await executor.SetRequest(o => o.Request = request)
                .AddFilter(o => func.xIsNotNull() ? func(serviceExecutor) : true)
                .OnExecutedAsync(o => {
                    result = o.Result;
                    return Task.FromResult(true);
                });
            return result;
        }

        /// <summary>
        /// 대량 서비스 생성을 위한 메서드
        /// 비공유 인스턴스 생성을 위해 servicelocator를 사용함.
        /// </summary>
        /// <param name="requests"></param>
        /// <typeparam name="TServiceExecutor"></typeparam>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        protected IEnumerable<TResult> CreateBulkService<TServiceExecutor, TRequest, TResult>(
            IEnumerable<TRequest> requests)
            where TServiceExecutor : IServiceExecutor<TRequest, TResult> {
            var results = new XList<TResult>();
            using var bulkExecutor = new BulkServiceExecutorManager<TServiceExecutor, TRequest>(requests);
            bulkExecutor.SetRequest((o, c) => o.Request = c)
                .OnExecuted(o => {
                    results.Add(o.Result);
                    return true;
                });

            return results;
        }

        public void Dispose() {
            CLogger.Factory.Configuration.Variables.Clear();
        }
    }
    /// <summary>
    /// 컨트롤러 베이스
    /// </summary>
    /// <typeparam name="TController"></typeparam>
    [Route("api/[controller]/[action]")] //url version route
    public class JController<T> : JControllerBase<T> where T : class{
        //protected ISessionContext Context = ServiceLocator.Current.GetInstance<ISessionContext>();
        public JController(ILogger<T> logger) : base(logger) {
        }

        public void Dispose() {
            
        }
    }

    /// <summary>
    /// 버전 관리 컨트롤러 베이스
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]/[action]")] //url version route
    public class JVersionControllerBase<T> : JControllerBase<T> where T : class {
        public JVersionControllerBase(ILogger<T> logger) : base(logger) {
        }

        public void Dispose() {
            
        }
    }
}