using JWLibrary.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AccountWebApi.Controllers {
    public class JwtTokenController : JController<JwtTokenController> {
        public JwtTokenController(ILogger<JwtTokenController> logger) : base(logger) {
        }
        
        [HttpGet]
        public string GetAuthorizeToken() {
            return string.Empty;
        }
    }
}