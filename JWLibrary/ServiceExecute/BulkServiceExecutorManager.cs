using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWLibrary.Core;

namespace JWLibrary.ServiceExecutor {
    public class BulkServiceExecutorManager<TIService, TRequest> : IDisposable 
        where TIService : IServiceBase, new() {

        private readonly List<Tuple<ServiceExecutorManager<TIService>, TRequest>> _services;
        public BulkServiceExecutorManager(IEnumerable<TRequest> requestItems) {
            var enumerable = requestItems as TRequest[] ?? requestItems.ToArray();
            this._services = new List<Tuple<ServiceExecutorManager<TIService>, TRequest>>(enumerable.Count());
            enumerable.jForEach(request => {
                var serviceExecutorManager = new ServiceExecutorManager<TIService>(new TIService());
                this._services.Add(
                    new Tuple<ServiceExecutorManager<TIService>, TRequest>(serviceExecutorManager, request));
                return true;
            });
        }
        
        public BulkServiceExecutorManager<TIService, TRequest> SetRequest(Action<TIService, TRequest> action) {
            this._services.jForEach(service => {
                service.Item1.SetRequest<TRequest>(service.Item2, action);
                return true;
            });
            return this;
        }

        public BulkServiceExecutorManager<TIService, TRequest> AddFilter(Func<TIService, bool> func) {
            this._services.jForEach(service => {
                service.Item1.AddFilter(func);
                return true;
            });
            return this;
        }
        
        public void OnExecuted(Action<TIService> action) {
            this._services.jForEach(service => {
                service.Item1.OnExecuted(action);
                return true;
            });
        }
        
        public async Task OnExecutedAsync(Func<TIService, Task> func) {
            foreach (var service in this._services) {
                await service.Item1.OnExecutedAsync(func);
            }
        }
        
        public void Dispose() {
            foreach (var service in this._services) {
                service.Item1.Dispose();
            }
        }
    }
}