using System.Collections;
using System.Collections.Generic;
using eXtensionSharp;
using JWLibrary.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TodoService;
using TodoService.Data;

namespace TodoWebApi.Controllers {
    public class TransactionSampleController : JController<TransactionSampleController> {
        private ITransactionSampleSvc _transactionSampleSvc;
        public TransactionSampleController(ILogger<TransactionSampleController> logger, ITransactionSampleSvc transactionSampleSvc) : base(logger) {
            _transactionSampleSvc = transactionSampleSvc;
        }

        [HttpGet]
        public IEnumerable<TODO> GetSample1() {
            return this.CreateService<ITransactionSampleSvc, bool, XList<TODO>>(_transactionSampleSvc, true);
        }
    }
}