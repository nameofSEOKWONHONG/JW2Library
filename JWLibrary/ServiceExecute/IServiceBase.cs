using System;
using System.Threading.Tasks;

namespace JWLibrary.ServiceExecutor {
    public interface IServiceBase : IDisposable {
        bool PreExecute();
        void Execute();
        Task ExecuteAsync();
        void PostExecute();
        bool Validate();
    }
}