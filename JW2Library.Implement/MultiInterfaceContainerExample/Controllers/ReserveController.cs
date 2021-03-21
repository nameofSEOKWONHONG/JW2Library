using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MultiInterfaceContainerExample.Services;

namespace MultiInterfaceContainerExample.Controllers {
    [ApiController]
    [Microsoft.AspNetCore.Components.Route("[controller]")]
    public class ReserveController: ControllerBase {
        private ILogger _logger;
        private IReserveService _service;
        public ReserveController(ILogger<ReserveController> logger, IReserveService reserveService) {
            this._logger = logger;
            this._service = reserveService;
        }

        [HttpGet("Reserve")]
        public bool Reserve(string name) {
            var result = this._service.Reserve(name);
            return true;
        }

        [HttpGet("Cancel")]
        public bool Cancel(string name) {
            var result = this._service.Cancel(name);
            return true;
        }
    }
}