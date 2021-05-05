﻿using System;
using System.Threading.Tasks;
using System.Transactions;
using eXtensionSharp;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JWLibrary.Web {
    /// <summary>
    /// use only single database transaction.
    /// dotnet core & 5 not support multiple database transactions.
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