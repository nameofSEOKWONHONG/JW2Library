using AccountService;
using eXtensionSharp;
using JWLibrary.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TodoService.Data;

namespace AccountWebApi.Controllers {
    public class JwtTokenController : JController<JwtTokenController> {
        private readonly IGetAccountSvc _getAccountSvc;
        public JwtTokenController(ILogger<JwtTokenController> logger,
            IGetAccountSvc getAccountSvc) : base(logger) {
            _getAccountSvc = getAccountSvc;
        }
        
        [HttpPost]
        public string GetAuthorizeToken(USER user) {
            var jwtTokenService = new JwtTokenService();
            var existsUser = this.CreateService<IGetAccountSvc, string, USER>(_getAccountSvc, user.USER_ID);
            if (existsUser.xIsNotNull()) {
                if (existsUser.PASSWORD == user.PASSWORD) {
                    var token = jwtTokenService.GenerateJwtToken(existsUser.USER_ID);
                    return token;
                }
            }

            return string.Empty;
        }
    }
}