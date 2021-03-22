namespace Service.Contract {
    public abstract class BasicContract : IContract {
        public abstract bool UserCheck(IUser user);

        public abstract bool PreContract(IUser user, IGoods goods);

        public abstract bool DoContract(IUser user, IGoods goods, ICompany company);

        public abstract bool PostContract(IUser user, IGoods goods, ICompany company);

        public abstract bool CancelContract(IUser user, IGoods goods, ICompany company);
    }
}