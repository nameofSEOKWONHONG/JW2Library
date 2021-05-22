using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eXtensionSharp;
using JWLibrary.DI;

namespace JWLibrary.ServiceExecutor {
    
    public sealed class BulkServiceExecutorManager<TIService, TRequest> : IDisposable 
        where TIService : IServiceBase {

        private readonly List<BulkService<TIService, TRequest>> _bulkServices;

        public BulkServiceExecutorManager(IEnumerable<TRequest> requestItems) {
            this._bulkServices = new List<BulkService<TIService, TRequest>>(requestItems.Count());
            requestItems.xForEach(request => {
                var instance = ServiceLocator.Current.GetInstance<TIService>();
                var serviceExecutorManager = new ServiceExecutorManager<TIService>(instance);
                this._bulkServices.Add(new BulkService<TIService, TRequest>() {
                    SvcExecManager = serviceExecutorManager,
                    Reqeust = request
                });
                return true;
            });
        }

        public BulkServiceExecutorManager(TIService service, IEnumerable<TRequest> requests) {
            this._bulkServices = new List<BulkService<TIService, TRequest>>(requests.Count());
            requests.xForEach(request => {
                var serviceExecutorManager = new ServiceExecutorManager<TIService>(service);
                this._bulkServices.Add(new BulkService<TIService, TRequest>() {
                    SvcExecManager = serviceExecutorManager,
                    Reqeust = request
                });
            });
        }
        
        public BulkServiceExecutorManager<TIService, TRequest> SetRequest(Action<TIService, TRequest> action) {
            this._bulkServices.xForEach(bulkService => {
                bulkService.SvcExecManager.SetRequest<TRequest>(bulkService.Reqeust, action);
                return true;
            });
            return this;
        }

        public BulkServiceExecutorManager<TIService, TRequest> AddFilter(Func<TIService, bool> func) {
            this._bulkServices.xForEach(bulkService => {
                bulkService.SvcExecManager.AddFilter(func);
                return true;
            });
            return this;
        }
        
        public bool OnExecuted(Func<TIService, bool> func) {
            var isResult = true;
            this._bulkServices.xForEach(bulkService => {
                isResult = bulkService.SvcExecManager.OnExecuted(func);
                if (isResult.xIsFalse()) {
                    throw new Exception($"{typeof(TIService).Name} execute failed.");
                }

                return true;
            });

            return isResult;
        }
        
        public async Task<bool> OnExecutedAsync(Func<TIService, Task<bool>> func) {
            var isResult = true;
            foreach (var bulkService in this._bulkServices) {
                isResult = await bulkService.SvcExecManager.OnExecutedAsync(func);
                if (isResult.xIsFalse()) return false;
            }

            return isResult;
        }
        
        public void Dispose() {
            foreach (var bulkService in this._bulkServices) {
                bulkService.SvcExecManager.Dispose();
            }
        }
    }

    internal class BulkService<TIService, TRequest> where TIService : IServiceBase {
        public ServiceExecutorManager<TIService> SvcExecManager { get; set; }
        public TRequest Reqeust { get; set; }
    }
}