using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JWLibrary.Web {
    /// <summary>
    /// 컨트롤러 베이스
    /// </summary>
    /// <typeparam name="TController"></typeparam>
    [Route("api/[controller]/[action]")] //url version route
    public class JController<TController> : JControllerBase<TController>
        where TController : class {
        //protected ISessionContext Context = ServiceLocator.Current.GetInstance<ISessionContext>();
        public JController(ILogger<TController> logger) : base(logger) {
        }

        public override void Dispose() {
            
        }
    }
}