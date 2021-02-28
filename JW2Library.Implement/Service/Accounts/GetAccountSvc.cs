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

        public override void Execute() {
            var flexer = new JLiteDBFlex.JLiteDbFlexerManager<Account>();
            var account = flexer.Create().LiteCollection
                .FindOne(m => m.UserId == this.Request.UserId && m.Passwd == this.Request.Passwd);

            this.Result = account;
        }

        public class Validator : ValidatorBase<GetAccountSvc> {
            public Validator() {

            }
        }
    }
}
