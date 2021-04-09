using FluentValidation;
using JLiteDBFlex;
using JWLibrary.Core;
using JWLibrary.ServiceExecutor;
using JWService.Data.Models;
using Service.Data;

namespace Service.Accounts {
    public class SaveAccountSvc : AccountServiceBase<SaveAccountSvc, Account, bool>, ISaveAccountSvc {
        private readonly IGetAccountSvc _getAccountSvc;

        public SaveAccountSvc(IGetAccountSvc getAccountSvc) {
            base.SetValidator(new Validator());
            _getAccountSvc = getAccountSvc;
        }

        public override bool PreExecute() {
            Account exists = null;
            using var executor = new ServiceExecutorManager<IGetAccountSvc>(_getAccountSvc);
            executor.SetRequest(o => o.Request = Request)
                .AddFilter(o => o.Request.isNotNull())
                .OnExecuted(o => { exists = o.Result; });

            if (exists.isNull()) return false;
            return true;
        }

        public override void Execute() {
            var litedb = JLiteDbFlexerManager.Create<Account>();
            var result = litedb.LiteCollection.Insert(Request);
            Result = (int) result > 0;
        }

        public class Validator : AbstractValidator<SaveAccountSvc> {
        }
    }
}