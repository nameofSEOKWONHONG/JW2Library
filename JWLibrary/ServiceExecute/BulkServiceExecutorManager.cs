using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWLibrary.Core;
using JWLibrary.DI;
using Microsoft.Extensions.DependencyInjection;

namespace JWLibrary.ServiceExecutor {
    
    public sealed class BulkServiceExecutorManager<TIService, TRequest> : IDisposable 
        where TIService : IServiceBase {

        private readonly List<BulkService<TIService, TRequest>> _bulkServices;

        public BulkServiceExecutorManager(IEnumerable<TRequest> requestItems) {
            this._bulkServices = new List<BulkService<TIService, TRequest>>(requestItems.Count());
            requestItems.jForEach(request => {
                var instance = ServiceLocator.Current.GetInstance<TIService>();
                var serviceExecutorManager = new ServiceExecutorManager<TIService>(instance);
                this._bulkServices.Add(new BulkService<TIService, TRequest>() {
                    SvcExecManager = serviceExecutorManager,
                    Reqeust = request
                });
                return true;
            });
        }
        
        public BulkServiceExecutorManager<TIService, TRequest> SetRequest(Action<TIService, TRequest> action) {
            this._bulkServices.jForEach(bulkService => {
                bulkService.SvcExecManager.SetRequest<TRequest>(bulkService.Reqeust, action);
                return true;
            });
            return this;
        }

        public BulkServiceExecutorManager<TIService, TRequest> AddFilter(Func<TIService, bool> func) {
            this._bulkServices.jForEach(bulkService => {
                bulkService.SvcExecManager.AddFilter(func);
                return true;
            });
            return this;
        }
        
        public void OnExecuted(Action<TIService> action) {
            this._bulkServices.jForEach(bulkService => {
                bulkService.SvcExecManager.OnExecuted(action);
                return true;
            });
        }
        
        public async Task OnExecutedAsync(Func<TIService, Task> func) {
            foreach (var bulkService in this._bulkServices) {
                await bulkService.SvcExecManager.OnExecutedAsync(func);
            }
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