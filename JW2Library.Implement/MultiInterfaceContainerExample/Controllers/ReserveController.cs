using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MultiInterfaceContainerExample.Services;

namespace MultiInterfaceContainerExample.Controllers {
    [ApiController]
    [Microsoft.AspNetCore.Components.Route("[controller]")]
    public class ReserveController : ControllerBase {
        private ILogger _logger;
        private readonly IReserveService _service;

        public ReserveController(ILogger<ReserveController> logger, IReserveService reserveService) {
            _logger = logger;
            _service = reserveService;
        }

        [HttpGet("Reserve")]
        public bool Reserve(string name) {
            var result = _service.Reserve(name);
            return true;
        }

        [HttpGet("Cancel")]
        public bool Cancel(string name) {
            var result = _service.Cancel(name);
            return true;
        }
    }
}