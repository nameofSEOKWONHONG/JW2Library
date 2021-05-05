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
            var litedb = LiteDbFlexerManager.Instance.Create<Account>();
            var account = litedb.LiteDatabase.GetCollection<Account>()
                .FindOne(m => m.UserId == Request.UserId && m.Passwd == Request.Passwd);

            Result = account;
        }

        public class Validator : ValidatorBase<GetAccountSvc> {
        }
    }
}