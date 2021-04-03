// using JWLibrary.Web;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Logging;
//
// namespace APIServer.Controllers {
//     public class ConfigStrageController : JControllerBase<ConfigStrageController> {
//         public ConfigStrageController(ILogger<ConfigStrageController> logger) : base(logger) {
//         }
//
//         [HttpPost]
//         public string Save([FromBody] string jsonConfig) {
//             var config = base.Context.CacheManager.Get<string>(jsonConfig);
//             return true;
//         }
//         
//         [HttpGet]
//         public bool 
//     }
// }