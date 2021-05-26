using JWLibrary.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TodoWebApi.Controllers.Version.V2 {
    [ApiVersion("2.0")]
    public class Version2Controller : JVersionControllerBase<Version2Controller> {
        public Version2Controller(ILogger<Version2Controller> logger) : base(logger) {
        }
        
        [HttpGet]
        public string GetVersion() {
            return "api version 2";
        }
        
        [HttpPost]
        public string SetVersion(string v) {
            return $"api version 2 on {v}";
        }
    }
}