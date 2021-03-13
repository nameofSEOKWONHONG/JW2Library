using FluentValidation;
using JLiteDBFlex;
using JWLibrary.Core;
using JWLibrary.ServiceExecutor;
using JWService.Data.Models;
using LiteDB;
using LiteDbFlex;
using Service.Data;

namespace Service.Accounts {
    public class SaveAccountSvc : AccountServiceBase<SaveAccountSvc, Account, bool>, ISaveAccountSvc {
        IGetAccountSvc _getAccountSvc;
        public SaveAccountSvc(IGetAccountSvc getAccountSvc) {
            base.SetValidator(new Validator());
            this._getAccountSvc = getAccountSvc;
        }

        public override bool PreExecute() {
            Account exists = null; 
            using var executor = new ServiceExecutorManager<IGetAccountSvc>(this._getAccountSvc);
            executor.SetRequest(o => o.Request = this.Request)
                .AddFilter(o => o.Request.jIsNotNull())
                .OnExecuted(o => {
                    exists = o.Result;
                });

            if (exists.jIsNull()) return false;
            return true;
        }

        public override void Execute() {
            var litedb = JLiteDbFlexerManager.Create<Account>();
            var result= litedb.LiteCollection.Insert(this.Request);
            this.Result = (int) result > 0;
        }

        public class Validator : AbstractValidator<SaveAccountSvc> {
            public Validator() {

            }
        }
    }
}
