using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace Service.Contract {
    public class ContractProcessor : IContractProcessor {
        private IContract _contract;
        private IUser _user;
        private IGoods _goods;
        private ICompany _company;
        
        public ContractProcessor(IContract contract, IUser user, IGoods goods, ICompany company) {
            this._contract = contract;
            this._user = user;
            this._goods = goods;
            this._company = company;
        }

        public bool Process() {
            bool isFailed = false;

            if (!isFailed) {
                try {
                    if (this._contract.UserCheck(this._user)) {
                        if (this._contract.PreContract(this._user, this._goods)) {
                            isFailed = this._contract.DoContract(this._user, this._goods, this._company);
                            this._contract.PostContract(this._user, this._goods, this._company);
                        }
                    }
                }
                catch (Exception e) {
                    isFailed = true;
                }
            }
            
            if (isFailed) {
                this._contract.CancelContract(_user, _goods, _company);
            }

            return isFailed;
        }
    }
}