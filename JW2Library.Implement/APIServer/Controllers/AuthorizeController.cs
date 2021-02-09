using JWLibrary.ApiCore.Config;
using JWLibrary.Core;
using JWLibrary.ServiceExecutor;
using JWLibrary.Web;
using JWService.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Accounts;
using Service.Data;
using System;
using System.Threading.Tasks;

namespace JWLibrary.ApiCore.Controllers {
    public class AuthorizeController : JControllerBase<AuthorizeController> {
        private readonly IGetAccountSvc _svc;
        public AuthorizeController(ILogger<AuthorizeController> logger,
            IGetAccountSvc svc)
            : base(logger) {
            _svc = svc;
        }

        [HttpPost]
        public async Task<string> GetToken([FromBody] Account account) {
            var jwtTokenService = new JWTTokenService();
            Account result = null;
            using var executor = new ServiceExecutorManager<IGetAccountSvc>(_svc);
            await executor.SetRequest(o => o.Request = account)
                .AddFilter(o => o.Request.jIsNotNull())
                .OnExecutedAsync(o => {
                    result = o.Result;
                });

            var jwtToken = jwtTokenService.GenerateJwtToken(result.Id);
            return jwtToken;
        }
    }
}