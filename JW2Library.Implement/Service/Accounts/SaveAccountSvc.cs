using FluentValidation;
using JWLibrary.Core;
using JWLibrary.ServiceExecutor;
using JWService.Data.Models;
using LiteDB;
using LiteDbFlex;
using Service.Data;

namespace Service.Accounts {
    public class SaveAccountSvc : AccountServiceBase<SaveAccountSvc, Account, bool>, ISaveAccountSvc {
        Account _exists = null;
        IGetAccountSvc _getAccountSvc;
        public SaveAccountSvc(IGetAccountSvc getAccountSvc) {
            base.SetValidator(new Validator());
            this._getAccountSvc = getAccountSvc;
        }

        public override bool PreExecute() {
            using var executor = new ServiceExecutorManager<IGetAccountSvc>(this._getAccountSvc);
            executor.SetRequest(o => o.Request = this.Request)
                .AddFilter(o => o.Request.jIsNotNull())
                .OnExecuted(o => {
                    this._exists = o.Result;
                });

            if (this._exists.jIsNull()) return false;
            return true;
        }

        public override void Execute() {
            if (_exists.jIsNotNull()) {
                _exists.UserId = this.Request.UserId;
                _exists.Passwd = this.Request.Passwd;

                var result = LiteDbFlex.LiteDbFlexerManager<Account>.Instance.Value.Create().Insert(Request).GetResult<BsonValue>();
                this.Result = (int) result > 0;
            }
            else {
                var result = LiteDbSafeFlexer<Account>.Instance.Value.Execute(litedb => {
                    return litedb.BeginTrans()
                    .Insert(this.Request)
                    .Commit()
                    .GetResult<BsonValue>();
                });

                this.Result = (int)result > 0;
            }
        }

        public class Validator : AbstractValidator<SaveAccountSvc> {
            public Validator() {

            }
        }
    }
}
