using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace MultiInterfaceContainerExample.Services {
    public interface IReserveService {
        bool Reserve(string name);
        bool Cancel(string name);
    }
    
    public class ReserveService : IReserveService {
        public ICompanyService CompanyService;
        public ITheaterService<CgvService> CgvService;
        public ITheaterService<LotteService> LotteService;
        public ReserveService(ICompanyService companyService, CgvService cgvService, LotteService lotteService) {
            this.CompanyService = companyService;
            this.CgvService = cgvService;
            this.LotteService = lotteService;
        }

        public bool Reserve(string name) {
            name = "hsw";
            try {
                if (this.CompanyService.CheckUserInfo(name)) {
                    this.CompanyService.CheckOut(name);

                    if (this.CompanyService.TicketType == TICKET_TYPE.CGV) {
                        this.CgvService.PreAction(name);
                        this.CgvService.DoAction(name);
                        this.CgvService.PostAction(name);
                    }
                    else if (this.CompanyService.TicketType == TICKET_TYPE.LOTTE) {
                        this.LotteService.PreAction(name);
                        this.LotteService.DoAction(name);
                        this.LotteService.PostAction(name);
                    }
                    else if (this.CompanyService.TicketType == TICKET_TYPE.MEGABOX) {
                        throw new NotImplementedException();
                    }

                    this.CompanyService.Notify(name);
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
            this.CompanyService.Cancel(name);
            if (this.CompanyService.TicketType == TICKET_TYPE.CGV) {
                this.CgvService.Cancel(name);
            }
            else if (this.CompanyService.TicketType == TICKET_TYPE.LOTTE) {
                this.LotteService.Cancel(name);
            }
            else if (this.CompanyService.TicketType == TICKET_TYPE.MEGABOX) {
                throw new NotImplementedException();
            }

            return true;
        }
    }
}