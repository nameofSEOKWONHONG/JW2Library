using System;
using Newtonsoft.Json;

namespace MultiInterfaceContainerExample.Services {
    public interface IReserveService {
        bool Reserve(string name);
        bool Cancel(string name);
    }

    public class ReserveService : IReserveService {
        public ITheaterService<CgvService> CgvService;
        public ICompanyService CompanyService;
        public ITheaterService<LotteService> LotteService;

        public ReserveService(ICompanyService companyService, CgvService cgvService, LotteService lotteService) {
            CompanyService = companyService;
            CgvService = cgvService;
            LotteService = lotteService;
        }

        public bool Reserve(string name) {
            name = "hsw";
            try {
                if (CompanyService.CheckUserInfo(name)) {
                    CompanyService.CheckOut(name);

                    if (CompanyService.TicketType == TICKET_TYPE.CGV) {
                        CgvService.PreAction(name);
                        CgvService.DoAction(name);
                        CgvService.PostAction(name);
                    }
                    else if (CompanyService.TicketType == TICKET_TYPE.LOTTE) {
                        LotteService.PreAction(name);
                        LotteService.DoAction(name);
                        LotteService.PostAction(name);
                    }
                    else if (CompanyService.TicketType == TICKET_TYPE.MEGABOX) {
                        throw new NotImplementedException();
                    }

                    CompanyService.Notify(name);
                }
            }
            catch (Exception e) {
                Console.WriteLine(JsonConvert.SerializeObject(e));
                Cancel(name);
            }

            return true;
        }

        public bool Cancel(string name) {
            name = "hsw";
            CompanyService.Cancel(name);
            if (CompanyService.TicketType == TICKET_TYPE.CGV)
                CgvService.Cancel(name);
            else if (CompanyService.TicketType == TICKET_TYPE.LOTTE)
                LotteService.Cancel(name);
            else if (CompanyService.TicketType == TICKET_TYPE.MEGABOX) throw new NotImplementedException();

            return true;
        }
    }
}