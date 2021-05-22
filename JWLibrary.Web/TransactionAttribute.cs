using System;
using System.Threading.Tasks;
using System.Transactions;
using eXtensionSharp;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JWLibrary.Web {
    /// <summary>
    /// dotnet-core/dotnet 5는 단일 데이터 베이스 트랜젝션에서만 동작한다.  
    /// dotnet-framework에서만 복합 데이터 베이스 트랜젝션을 지원한다.
    /// </summary>
    //[Obsolete("use only single database transaction.", false)]
    public class TransactionAttribute : Attribute, IAsyncActionFilter {
        //make sure filter marked as not reusable
        private readonly TransactionScopeOption _transactionScopeOption;

        public TransactionAttribute(TransactionScopeOption transactionScopeOption) {
            _transactionScopeOption = transactionScopeOption;
        }

        public bool IsReusable => false;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) {
            using (var transactionScope = new TransactionScope(_transactionScopeOption, TimeSpan.FromSeconds(5),
                TransactionScopeAsyncFlowOption.Enabled)) {
                var action = await next();
                if (action.Exception.xIsNull()) transactionScope.Complete();
            }
        }
    }
}