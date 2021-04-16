using FluentValidation;
using JLiteDBFlex;
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
            var litedb = LiteDbFlexerManager.Instance.Create<Account>();
            Result = litedb.LiteCollection.FindById(Request.Data);
        }

        public class GetAccountByIdSvcValidator : AbstractValidator<GetAccountByIdSvc> {
        }
    }
}