namespace Service.Contract {
    public class BookContract : ProviderContract {
        public override bool UserCheck(IUser user) {
            return base.UserCheck(user);
        }

        public override bool PreContract(IUser user, IGoods goods) {
            return true;
        }

        public override bool DoContract(IUser user, IGoods goods, ICompany company) {
            return true;
        }

        public override bool PostContract(IUser user, IGoods goods, ICompany company) {
            return true;
        }

        public override bool CancelContract(IUser user, IGoods goods, ICompany company) {
            return true;
        }
    }
}