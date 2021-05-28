using System;
using JWLibrary.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TodoWebApi.Controllers.Version.V1 {
    [ApiVersion("1.0")]
    public class VersionController : JVersionController<VersionController> {
        public VersionController(ILogger<VersionController> logger) : base(logger) {
        }
        
        [HttpGet]
        public string GetVersion() {
            logger.Log(LogLevel.Trace, "test");
            var exception = new Exception("test exception");
            WriteLog("seokwon", "test", exception, null);
            throw exception;
            return "api version 1";
        }

        [HttpPost]
        public string SetVersion(string v) {
            return $"api version 1 on {v}";
        }
    }
}