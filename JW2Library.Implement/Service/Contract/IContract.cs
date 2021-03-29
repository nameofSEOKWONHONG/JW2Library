namespace Service.Contract {
    public interface IContract {
        bool UserCheck(IUser user);
        bool PreContract(IUser user, IGoods goods);
        bool DoContract(IUser user, IGoods goods, ICompany company);
        bool PostContract(IUser user, IGoods goods, ICompany company);
        bool CancelContract(IUser user, IGoods goods, ICompany company);
    }
}