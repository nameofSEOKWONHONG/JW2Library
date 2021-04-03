using System;

namespace Service.Contract {
    public class ContractProcessor : IContractProcessor {
        private readonly ICompany _company;
        private readonly IContract _contract;
        private readonly IGoods _goods;
        private readonly IUser _user;

        public ContractProcessor(IContract contract, IUser user, IGoods goods, ICompany company) {
            _contract = contract;
            _user = user;
            _goods = goods;
            _company = company;
        }

        public bool Process() {
            var isFailed = false;

            if (!isFailed)
                try {
                    if (_contract.UserCheck(_user))
                        if (_contract.PreContract(_user, _goods)) {
                            isFailed = _contract.DoContract(_user, _goods, _company);
                            _contract.PostContract(_user, _goods, _company);
                        }
                }
                catch (Exception e) {
                    isFailed = true;
                }

            if (isFailed) _contract.CancelContract(_user, _goods, _company);

            return isFailed;
        }
    }
}