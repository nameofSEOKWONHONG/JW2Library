using System;
using System.Threading.Tasks;
using eXtensionSharp;

namespace JWLibrary.ServiceExecutor {
    public sealed class ServiceExecutorManager<TIService> : IDisposable
        where TIService : IServiceBase {
        private readonly XList<Func<TIService, bool>> filters = new();
        private bool disposed;
        private TIService service;

        public ServiceExecutorManager(TIService service) {
            this.service = service;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public ServiceExecutorManager<TIService> SetRequest(Action<TIService> action) {
            action(service);
            return this;
        }

        public ServiceExecutorManager<TIService> SetRequest<TRequest>(TRequest request, Action<TIService, TRequest> action) {
            action(service, request);
            return this;
        }

        public ServiceExecutorManager<TIService> AddFilter(Func<TIService, bool> func) {
            filters.Add(func);
            return this;
        }

        public bool OnExecuted(Func<TIService, bool> func) {
            foreach (var filter in filters) {
                var pass = filter(service);
                if (pass.xIsFalse()) return false;
            }

            if (service.Validate()) {
                var preExecuted = service.PreExecute();
                if (preExecuted) service.Execute();
                service.PostExecute();

                var executed =  func(service);
                return executed;
            }

            return false;
        }

        public async Task<bool> OnExecutedAsync(Func<TIService, Task<bool>> func) {
            foreach (var filter in filters) {
                var pass = filter(service);
                if (pass.xIsFalse()) return false;
            }

            if (service.Validate()) {
                var preExecuted = service.PreExecute();
                if (preExecuted) await service.ExecuteAsync();
                service.PostExecute();
                
                var executed = await func(service);
                if (executed.xIsFalse()) {
                    throw new Exception($"{typeof(TIService).Name} executed failed.");
                }
                return executed;
            }

            return false;
        }

        public void Dispose(bool disposing) {
            if (disposed) return;
            if (disposing) service.Dispose();

            disposed = true;
        }
    }
}