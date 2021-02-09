﻿using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;
using System.Transactions;
using JWLibrary.Core;
using Microsoft.Extensions.Logging;

namespace JWLibrary.Web {

    public class TransactionAttribute : Attribute, IAsyncActionFilter
    {
        //make sure filter marked as not reusable
        private readonly TransactionScopeOption _transactionScopeOption;

        public TransactionAttribute(TransactionScopeOption transactionScopeOption)
        {
            this._transactionScopeOption = transactionScopeOption;
        }

        public bool IsReusable => false;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) {
            using (var transactionScope = new TransactionScope(_transactionScopeOption, TimeSpan.FromSeconds(5),
                TransactionScopeAsyncFlowOption.Enabled)) {
                var action = await next();
                if (action.Exception.jIsNull()) {
                    transactionScope.Complete();
                }
            }
        }
    }
}