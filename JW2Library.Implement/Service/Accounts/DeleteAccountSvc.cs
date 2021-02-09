using FluentValidation;
using JWLibrary.Core;
using JWLibrary.ServiceExecutor;
using JWService.Data.Models;
using Service.Data;

namespace Service.Accounts {
    public class DeleteAccountSvc : AccountServiceBase<DeleteAccountSvc, RequestDto<int>, bool>,
        IDeleteAccountSvc {
        private IGetAccountByIdSvc _getAccountByIdSvc;
        public DeleteAccountSvc(IGetAccountByIdSvc getAccountByIdSvc) {
            _getAccountByIdSvc = getAccountByIdSvc;
            base.SetValidator(new DeleteAccountServiceValidator());
        }

        public override bool PreExecute() {
            var result = false;
            using var executor = new ServiceExecutorManager<IGetAccountByIdSvc>(_getAccountByIdSvc);
            executor.SetRequest(o => o.Request = this.Request)
                .OnExecuted(o => {
                    result = o.Result.jIsNotNull();
                });

            return result;
        }

        public override void Execute() {
            this.Result = LiteDbFlex.LiteDbSafeFlexer<Account>.Instance.Value.Execute(litedb => {
                return litedb.BeginTrans()
                    .Delete(m => m.Id == this.Request.Data)
                    .GetResult<int>() > 0;
            });
        }

        public class DeleteAccountServiceValidator : AbstractValidator<DeleteAccountSvc> {
            public DeleteAccountServiceValidator() {
                RuleFor(o => o.Request).NotNull();
            }
        }
    }
}
