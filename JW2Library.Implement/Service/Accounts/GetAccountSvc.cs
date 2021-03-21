using JLiteDBFlex;
using JWLibrary.ServiceExecutor;
using JWService.Data.Models;
using Service.Data;

namespace Service.Accounts {
    public class GetAccountSvc : AccountServiceBase<GetAccountSvc, Account, Account>, IGetAccountSvc {
        public GetAccountSvc() {
            base.SetValidator(new Validator());
        }

        public override void Execute() {
            var litedb = JLiteDbFlexerManager.Create<Account>();
            var account = litedb.LiteCollection
                .FindOne(m => m.UserId == Request.UserId && m.Passwd == Request.Passwd);

            Result = account;
        }

        public class Validator : ValidatorBase<GetAccountSvc> {
        }
    }
}