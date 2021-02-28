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
            var flexer = new JLiteDBFlex.JLiteDbFlexerManager<Account>();
            this.Result = flexer.Create().LiteCollection.FindById(this.Request.Data);
        }

        public class GetAccountByIdSvcValidator : AbstractValidator<GetAccountByIdSvc> {
            public GetAccountByIdSvcValidator() {

            }
        }
    }
}
