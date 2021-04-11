using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWLibrary.Core;
using NetFabric.Hyperlinq;

namespace JWLibrary.ServiceExecutor {
    public sealed class ServiceExecutorManager<TIService> : IDisposable
        where TIService : IServiceBase {
        private readonly JList<Func<TIService, bool>> filters = new();
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
                if (pass.isFalse()) return false;
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
                if (pass.isFalse()) return false;
            }

            if (service.Validate()) {
                var preExecuted = service.PreExecute();
                if (preExecuted) await service.ExecuteAsync();
                service.PostExecute();

                var executed = await func(service);
                return executed;
            }

            return false;
        }

        protected void Dispose(bool disposing) {
            if (disposed) return;
            if (disposing) service.Dispose();

            disposed = true;
        }
    }
}