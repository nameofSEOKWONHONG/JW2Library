using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MultiInterfaceContainerExample.Services;

namespace MultiInterfaceContainerExample.Controllers {
    [ApiController]
    [Microsoft.AspNetCore.Components.Route("[controller]")]
    public class HelloController : ControllerBase {
        private ILogger _logger;
        private IBaseService<Hello1Service> _service;
        private IBaseService<Hello2Service> _service2;
        public HelloController(ILogger<HelloController> logger, IBaseService<Hello1Service> service, IBaseService<Hello2Service> service2) {
            _logger = logger;
            _service = service;
            _service2 = service2;
        }

        [HttpGet("Hello1")]
        public string Hello1(string str) {
            return _service.SayHello(str);
        }

        [HttpGet("Hello2")]
        public string Hello2(string str) {
            return _service2.SayHello(str);
        }
    }
}