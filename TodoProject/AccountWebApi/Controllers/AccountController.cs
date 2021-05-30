using System.Collections.Generic;
using AccountService;
using JWLibrary.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TodoService.Data;

namespace AccountWebApi.Controllers {
    [CustomAuthorize]
    public class AccountController : JController<AccountController> {
        private readonly IDeleteAccountSvc _deleteAccountSvc;
        private readonly IGetAccountsSvc _getAccountsSvc;
        private readonly IGetAccountSvc _getAccountSvc;
        private readonly ISaveAccountSvc _saveAccountSvc;

        public AccountController(ILogger<AccountController> logger,
            IGetAccountSvc getAccountSvc,
            IGetAccountsSvc getAccountsSvc,
            ISaveAccountSvc saveAccountSvc,
            IDeleteAccountSvc deleteAccountSvc) : base(logger) {
            _getAccountSvc = getAccountSvc;
            _getAccountsSvc = getAccountsSvc;
            _saveAccountSvc = saveAccountSvc;
            _deleteAccountSvc = deleteAccountSvc;
        }

        [HttpGet]
        public USER GetAccount(string userId) {
            return CreateService<IGetAccountSvc, string, USER>(_getAccountSvc, userId);
        }

        [HttpGet]
        public IEnumerable<USER> GetAccounts(USER user) {
            return CreateService<IGetAccountsSvc, USER, IEnumerable<USER>>(_getAccountsSvc, user);
        }

        [HttpPost]
        public int SaveAccount(USER user) {
            return CreateService<ISaveAccountSvc, USER, int>(_saveAccountSvc, user);
        }

        [HttpPost]
        public bool DeleteAccount(string userId) {
            return CreateService<IDeleteAccountSvc, string, bool>(_deleteAccountSvc, userId);
        }
    }
}