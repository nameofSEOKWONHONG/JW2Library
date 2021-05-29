using System.Collections;
using System.Collections.Generic;
using System.Threading;
using JWLibrary.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AccountWebApi.Controllers {
    public class AccountController : JController<AccountController> {
        public AccountController(ILogger<AccountController> logger) : base(logger) {
        }

        [HttpGet]
        public string GetAccount() {
            return string.Empty;
        }

        [HttpGet]
        public IEnumerable<string> GetAccounts() {
            return null;
        }

        [HttpPost]
        public int SaveAccount() {
            return 0;
        }

        [HttpPost]
        public bool DeleteAccount() {
            return false;
        }

        [HttpPost]
        public IEnumerable<bool> DeleteAccounts() {
            return null;
        }


    }
}