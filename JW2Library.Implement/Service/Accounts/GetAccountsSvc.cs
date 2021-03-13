using FluentValidation;
using JWLibrary.Core;
using JWLibrary.Database;
using JWService.Data.Models;
using LiteDbFlex;
using Mapster;
using Service.Data;
using System.Collections.Generic;
using JLiteDBFlex;

namespace Service.Accounts {
    public class GetAccountsSvc : AccountServiceBase<GetAccountsSvc, PagingRequestDto<Account>, PagingResultDto<IEnumerable<Account>>>,
        IGetAccountsSvc {

        public GetAccountsSvc() {
            base.SetValidator(new GetAccountsSvcValidator());
        }

        public override void Execute() {
            var litedb = JLiteDbFlexerManager.Create<Account>();
            var query = litedb.LiteCollection.Query();
            if (this.Request.Data.jIsNotNull()) {
                if (this.Request.Data.Id > 0) query = query.Where(m => m.Id >= this.Request.Data.Id);
                if (this.Request.Data.UserId.jIsNullOrEmpty())
                    query = query.Where(m => m.UserId == this.Request.Data.UserId);

                var accounts = query.Limit(this.Request.Size).Offset((this.Request.Page - 1) * this.Request.Page)
                    .ToList();
                
                var result = this.Request.Adapt<PagingResultDto<IEnumerable<Account>>>();
                result.TotalCount = litedb.LiteCollection.Count();
                result.Data = accounts;
                this.Result = result;
            }
        }

        public class GetAccountsSvcValidator : AbstractValidator<GetAccountsSvc> {
            public GetAccountsSvcValidator() {

            }
        }
    }
}
