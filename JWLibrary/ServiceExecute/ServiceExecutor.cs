using FluentValidation;
using JWLibrary.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWLibrary.ServiceExecutor {
    public class ServiceExecutor<TOwner, TRequest, TResult> : ServiceBase<TOwner>, IServiceExecutor<TRequest, TResult>
        where TOwner : ServiceExecutor<TOwner, TRequest, TResult> {
        public TOwner Owner { get; private set; }
        public TRequest Request { get; set; }
        public TResult Result { get; set; }
        public IValidator<TOwner> ServiceValidator { get; private set; }

        public ServiceExecutor() {
            this.Owner = (TOwner)this;
        }

        public override void SetValidator(IValidator<TOwner> validator) {
            this.ServiceValidator = validator;
        }

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
            if(this.ServiceValidator.jIsNotNull()) {
                var result = this.ServiceValidator.Validate(Owner);
                if(result.IsValid.jIsFalse()) {
                    throw new Exception(result.Errors.jFirst().ErrorMessage);
                }
            }
        }

        public override void Dispose() {

        }
    }


}
