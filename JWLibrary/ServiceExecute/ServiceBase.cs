using System.Threading.Tasks;
using FluentValidation;

namespace JWLibrary.ServiceExecutor {
    public abstract class ServiceBase<TOwner> : IServiceBase {
        public abstract void Dispose();
        public abstract void Execute();
        public abstract Task ExecuteAsync();

        public abstract void PostExecute();
        public abstract bool PreExecute();
        public abstract bool Validate();
        public abstract void SetValidator(IValidator<TOwner> validator);
        public abstract void SetValidator<T>() where T : AbstractValidator<TOwner>, new();
    }
}