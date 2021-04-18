using System.Collections.Generic;
using FluentValidation;
using JLiteDBFlex;
using JWLibrary.Core;
using JWService.Data.Models;
using Mapster;
using Service.Data;

namespace Service.Accounts {
    public class GetAccountsSvc : AccountServiceBase<GetAccountsSvc, PagingRequestDto<Account>,
            PagingResultDto<IEnumerable<Account>>>,
        IGetAccountsSvc {
        public GetAccountsSvc() {
            base.SetValidator(new GetAccountsSvcValidator());
        }

        public override void Execute() {
            var litedb = LiteDbFlexerManager.Instance.Create<Account>();
            var query = litedb.LiteCollection.Query();
            if (Request.Data.jIsNotNull()) {
                if (Request.Data.Id > 0) query = query.Where(m => m.Id >= Request.Data.Id);
                if (Request.Data.UserId.isNullOrEmpty())
                    query = query.Where(m => m.UserId == Request.Data.UserId);

                var accounts = query.Limit(Request.Size).Offset((Request.Page - 1) * Request.Page)
                    .ToList();

                var result = Request.Adapt<PagingResultDto<IEnumerable<Account>>>();
                result.TotalCount = litedb.LiteCollection.Count();
                result.Data = accounts;
                Result = result;
            }
        }

        public class GetAccountsSvcValidator : AbstractValidator<GetAccountsSvc> {
        }
    }
}