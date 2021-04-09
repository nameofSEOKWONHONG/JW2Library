using System;
using JWLibrary.Core;

namespace Service.Contract {
    public class ProviderContract : BasicContract {
        /// <summary>
        ///     서비스 제공 회사의 사용자 체크
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override bool UserCheck(IUser user) {
            var validator = new User.UserValidator();
            var result = validator.Validate(user);
            if (!result.IsValid) return false;

            return true;
        }

        /// <summary>
        ///     사용자 및 상품 체크
        /// </summary>
        /// <param name="user"></param>
        /// <param name="goods"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override bool PreContract(IUser user, IGoods goods) {
            var validator = new Goods.GoodsValidator();
            var result = validator.Validate(goods);
            if (!result.IsValid) return false;

            return true;
        }

        /// <summary>
        ///     사용자, 상품, 연결 회사의 실제 계약 체결
        /// </summary>
        /// <param name="user"></param>
        /// <param name="goods"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override bool DoContract(IUser user, IGoods goods, ICompany company) {
            var validator = new Company.CompanyValidator();
            var result = validator.Validate(company);

            if (!result.IsValid) return false;

            return true;
        }

        /// <summary>
        ///     실제 계약 후 후속 작업 (notification)
        /// </summary>
        /// <param name="user"></param>
        /// <param name="goods"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override bool PostContract(IUser user, IGoods goods, ICompany company) {
            //notice
            Console.WriteLine(user.fromObjectToJson());
            Console.WriteLine(goods.fromObjectToJson());
            Console.WriteLine(company.fromObjectToJson());
            return true;
        }

        /// <summary>
        ///     계약 취소
        /// </summary>
        /// <param name="user"></param>
        /// <param name="goods"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override bool CancelContract(IUser user, IGoods goods, ICompany company) {
            //cancel process;

            return true;
        }
    }
}