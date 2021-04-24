using System;
using eXtensionSharp;
using JWLibrary.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace APIServer.Controllers {
    [ApiVersion("1.0", Deprecated = true)] //비추천 공지함.
    [ApiVersion("1.1")]
    [ApiVersion("2.0")]
    public class VerionExampleController : JControllerBase<VerionExampleController> {
        public VerionExampleController(ILogger<VerionExampleController> logger) : base(logger) {
        }

        [HttpGet]
        public string Hello() {
            var str = "hello version 0.0" + DateTime.Now;
            var exists = Context.GetCacheManager().Get<string>(str);
            if (!exists.xIsNullOrEmpty()) return str;

            return str;
        }

        [HttpGet]
        [Route("{id}")]
        [MapToApiVersion("2.0")]
        public string Hello(int id) {
            return "hello id" + id;
        }
    }
}