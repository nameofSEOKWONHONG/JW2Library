using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using eXtensionSharp;
using FluentValidation;

namespace JWLibrary.ServiceExecutor
{
    public class ServiceExecutor<TOwner, TRequest, TResult> : ServiceBase<TOwner>, IServiceExecutor<TRequest, TResult>
        where TOwner : ServiceExecutor<TOwner, TRequest, TResult>
    {
        public ServiceExecutor()
        {
            Owner = (TOwner) this;
        }

        private TOwner Owner { get; }
        public IValidator<TOwner> ServiceValidator { get; private set; }
        public TRequest Request { get; set; }
        public TResult Result { get; set; }


        public override void Execute()
        {
        }

        public override Task ExecuteAsync()
        {
            return Task.CompletedTask;
        }

        public override bool PreExecute()
        {
            return true;
        }

        public override void PostExecute()
        {
        }

        public override bool Validate()
        {
            if (ServiceValidator.xIsNotNull())
            {
                var result = ServiceValidator.Validate(Owner);
                if (result.IsValid.xIsFalse())
                {
                    Debug.WriteLine($"service : {Owner.GetType().Name}");
                    Debug.WriteLine($"error : {result.Errors.First().ErrorMessage}");
                    return false;
                }
            }

            return true;
        }

        public override void Dispose()
        {
        }

        public override void SetValidator(IValidator<TOwner> validator)
        {
            ServiceValidator = validator;
        }

        public sealed override void SetValidator<T>()
        {
            ServiceValidator = new T();
        }
    }
}