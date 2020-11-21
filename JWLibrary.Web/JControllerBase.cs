using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace JWLibrary.Web {
    /// <summary>
    /// base controller
    /// </summary>
    [ApiController]
    //[Route("api/[controller]/[action]")] //normal route
    [Route("api/{v:apiVersion}/[controller]/[action]")] //url version route
    public class JControllerBase<TController> : ControllerBase, IDisposable
        where TController : class {
        protected ILogger<TController> Logger;

        public JControllerBase(ILogger<TController> logger) {
            Logger = logger;
        }

        public void Dispose() {

        }
    }
}
