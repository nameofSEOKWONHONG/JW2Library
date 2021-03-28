using System;
using System.Threading.Tasks;
using JWLibrary.Core;

namespace JWLibrary.ServiceExecutor {
    public class ServiceExecutorManager<TIService> : IDisposable
        where TIService : IServiceBase {
        private bool disposed;
        private readonly JList<Func<TIService, bool>> filters = new();
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

        public ServiceExecutorManager<TIService> AddFilter(Func<TIService, bool> func) {
            filters.Add(func);
            return this;
        }

        public void OnExecuted(Action<TIService> action) {
            foreach (var filter in filters) {
                var pass = filter(service);
                if (pass.jIsFalse()) return;
            }

            if (service.Validate()) {
                var preExecuted = service.PreExecute();
                if (preExecuted) service.Execute();
                service.PostExecute();

                action(service);               
            }
        }

        public async Task OnExecutedAsync(Func<TIService, Task> func) {
            foreach (var filter in filters) {
                var pass = filter(service);
                if (pass.jIsFalse()) return;
            }

            if (service.Validate()) {
                var preExecuted = service.PreExecute();
                if (preExecuted) await service.ExecuteAsync();
                service.PostExecute();

                await func(service);               
            }
        }

        protected virtual void Dispose(bool disposing) {
            if (disposed) return;
            if (disposing) service.Dispose();

            disposed = true;
        }
    }
}