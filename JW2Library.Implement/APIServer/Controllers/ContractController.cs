using JWLibrary.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Contract;

namespace APIServer.Controllers {
    public class ContractController : JController<ContractController> {
        public ContractController(ILogger<ContractController> logger) : base(logger) {
        }

        [HttpPost("Contract")]
        public bool Contract([FromBody] ContractRequetDto contractRequetDto) {
            var user = new User();
            var goods = new Goods();
            var company = new Company();
            var contract = ContractFactory.CreateInstance(contractRequetDto.ContractType);
            var contractProcessor = new ContractProcessor(contract, user, goods, company);
            contractProcessor.Process();
            return true;
        }
    }
}