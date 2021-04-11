using FluentValidation;
using JLiteDBFlex;
using JWLibrary.Core;
using JWLibrary.ServiceExecutor;
using JWService.Data.Models;
using Service.Data;

namespace Service.Accounts {
    public class DeleteAccountSvc : AccountServiceBase<DeleteAccountSvc, RequestDto<int>, bool>,
        IDeleteAccountSvc {
        private readonly IGetAccountByIdSvc _getAccountByIdSvc;

        public DeleteAccountSvc(IGetAccountByIdSvc getAccountByIdSvc) {
            _getAccountByIdSvc = getAccountByIdSvc;
            base.SetValidator(new DeleteAccountServiceValidator());
        }

        public override bool PreExecute() {
            var result = false;
            using var executor = new ServiceExecutorManager<IGetAccountByIdSvc>(_getAccountByIdSvc);
            executor.SetRequest(o => o.Request = Request)
                .OnExecuted(o => {
                    result = o.Result.isNotNull();
                    return true;
                });

            return result;
        }

        public override void Execute() {
            var litedb = JLiteDbFlexerManager.Create<Account>();
            litedb.LiteDatabase.BeginTrans();
            Result = litedb.LiteCollection.Delete(Request.Data);
            litedb.LiteDatabase.Commit();
        }

        public class DeleteAccountServiceValidator : AbstractValidator<DeleteAccountSvc> {
            public DeleteAccountServiceValidator() {
                RuleFor(o => o.Request).NotNull();
            }
        }
    }
}