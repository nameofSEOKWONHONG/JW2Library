using FluentValidation;
using JWLibrary.ServiceExecutor;
using JWService.Data.Models;
using LiteDbFlex;
using Service.Data;

namespace Service.Accounts {
    public class GetAccountSvc : AccountServiceBase<GetAccountSvc, Account, Account>, IGetAccountSvc {
        public GetAccountSvc() {
            base.SetValidator(new Validator());
        }

        public override void Execute()
        {
            var account = LiteDbSafeFlexer<Account>.Instance.Value.Execute(litedb => {
                return litedb.Get(m => m.UserId == this.Request.UserId &&
                          m.Passwd == this.Request.Passwd)
                .GetResult<Account>();
            });

            this.Result = account;
        }

        public class Validator : ValidatorBase<GetAccountSvc> {
            public Validator() {

            }
        }
    }
}
