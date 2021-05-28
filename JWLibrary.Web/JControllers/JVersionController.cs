using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JWLibrary.Web {
    /// <summary>
    /// 버전 관리 컨트롤러 베이스
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]/[action]")] //url version route
    public class JVersionController<TController> : JControllerBase<TController>
        where TController : class {
        public JVersionController(ILogger<TController> logger) : base(logger) {
        }

        public override void Dispose() {
            
        }
    }
}