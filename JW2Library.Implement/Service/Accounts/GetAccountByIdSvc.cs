using FluentValidation;
using JWLibrary.Core;
using JWService.Data.Models;
using Service.Data;

namespace Service.Accounts {
    public class GetAccountByIdSvc : AccountServiceBase<GetAccountByIdSvc, RequestDto<int>, Account>,
        IGetAccountByIdSvc {

        public GetAccountByIdSvc() {
            base.SetValidator(new GetAccountByIdSvcValidator());
        }

        public override void Execute() {
            this.Result = LiteDbFlex.LiteDbSafeFlexer<Account>.Instance.Value.Execute<Account>(litedb => {
                return litedb.Get(this.Request.Data)
                    .GetResult<Account>();
            });
        }

        public class GetAccountByIdSvcValidator : AbstractValidator<GetAccountByIdSvc> {
            public GetAccountByIdSvcValidator() {

            }
        }
    }
}
