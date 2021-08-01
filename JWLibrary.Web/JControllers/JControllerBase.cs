using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using eXtensionSharp;
using JWLibrary.DI;
using JWLibrary.ServiceExecutor;
using JWLibrary.Util.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JWLibrary.Web {
    [ApiController]
    public abstract class JControllerBase<TController> : ControllerBase, IDisposable 
        where TController : class {
        protected ILogger<TController> logger;
        private NLog.ILogger _fileLogger;

        public JControllerBase(ILogger<TController> logger) {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            this.logger = logger;
            _fileLogger = NLog.LogManager.GetLogger("Log");
        }

        protected void WriteLog(string folderPath, string message, Exception e, params object[] args) {
            Thread.Sleep(1);
            _fileLogger.Factory.Configuration.Variables.Clear();
            _fileLogger.Factory.Configuration.Variables.Add("runtime", folderPath);    
            _fileLogger.Factory.ReconfigExistingLoggers();
            
            _fileLogger.Trace(message, e, args);
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
            var results = new List<TResult>();
            using var bulkExecutor = new BulkServiceExecutorManager<TServiceExecutor, TRequest>(requests);
            bulkExecutor.SetRequest((o, c) => o.Request = c)
                .OnExecuted(o => {
                    results.Add(o.Result);
                    return true;
                });

            return results;
        }

        public abstract void Dispose();
    }
}