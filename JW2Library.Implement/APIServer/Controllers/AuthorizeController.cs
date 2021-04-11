using System.Threading.Tasks;
using APIServer.Config;
using JWLibrary.Core;
using JWLibrary.ServiceExecutor;
using JWLibrary.Web;
using JWService.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Data;

namespace APIServer.Controllers {
    public class AuthorizeController : JControllerBase<AuthorizeController> {
        private readonly IGetAccountSvc _svc;

        public AuthorizeController(ILogger<AuthorizeController> logger,
            IGetAccountSvc svc)
            : base(logger) {
            _svc = svc;
        }

        [HttpPost]
        public async Task<string> GetToken([FromBody] Account account) {
            var jwtTokenService = new JwtTokenService();
            Account result = null;
            using var executor = new ServiceExecutorManager<IGetAccountSvc>(_svc);
            await executor.SetRequest(o => o.Request = account)
                .AddFilter(o => o.Request.isNotNull())
                .OnExecutedAsync(async o => {
                    result = o.Result;
                    return true;
                });

            var jwtToken = jwtTokenService.GenerateJwtToken(result.Id);
            return jwtToken;
        }
    }
}