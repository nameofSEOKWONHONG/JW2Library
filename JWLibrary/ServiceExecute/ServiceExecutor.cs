using System;
using System.Threading.Tasks;
using FluentValidation;
using JWLibrary.Core;

namespace JWLibrary.ServiceExecutor {
    public class ServiceExecutor<TOwner, TRequest, TResult> : ServiceBase<TOwner>, IServiceExecutor<TRequest, TResult>
        where TOwner : ServiceExecutor<TOwner, TRequest, TResult> {
        public ServiceExecutor() {
            Owner = (TOwner) this;
        }

        public TOwner Owner { get; }
        public IValidator<TOwner> ServiceValidator { get; private set; }
        public TRequest Request { get; set; }
        public TResult Result { get; set; }

        public override void Execute() {
        }

        public override Task ExecuteAsync() {
            return Task.CompletedTask;
        }

        public override void PostExecute() {
        }

        public override bool PreExecute() {
            return true;
        }

        public override void Validate() {
            if (ServiceValidator.jIsNotNull()) {
                var result = ServiceValidator.Validate(Owner);
                if (result.IsValid.jIsFalse()) throw new Exception(result.Errors.jFirst().ErrorMessage);
            }
        }

        public override void Dispose() {
        }

        public override void SetValidator(IValidator<TOwner> validator) {
            ServiceValidator = validator;
        }
    }
}