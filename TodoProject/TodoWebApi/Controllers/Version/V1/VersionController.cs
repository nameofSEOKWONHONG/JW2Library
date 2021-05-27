using JWLibrary.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;

namespace TodoWebApi.Controllers.Version.V1 {
    [ApiVersion("1.0")]
    public class VersionController : JVersionControllerBase<VersionController> {
        public VersionController(ILogger<VersionController> logger) : base(logger) {
        }
        
        [HttpGet]
        public string GetVersion() {
            return "api version 1";
        }

        [HttpPost]
        public string SetVersion(string v) {
            return $"api version 1 on {v}";
        }
    }
}