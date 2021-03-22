using FluentValidation;

namespace Service.Contract {
    public enum ENUM_CONTRACT_TYPE {
        BOOK,
        MOVIE,
        TICKET
    }
    
    public class ContractRequetDto {
        public ENUM_CONTRACT_TYPE ContractType { get; set; }
        public IUser User { get; set; }
        public IGoods Goods { get; set; }
        public ICompany Company { get; set; }
            
    }
    public interface IUser {
        string UserName { get; set; }
        string UserToken { get; set; }
        int Qty { get; set; }
    }
    
    public class User : IUser {
        public string UserName { get; set; }
        public string UserToken { get; set; }
        
        public int Qty { get; set; }
        
        public class UserValidator : AbstractValidator<IUser> {
            public UserValidator() {
                RuleFor(m => m.UserName).NotEmpty();
                RuleFor(m => m.UserToken).NotEmpty();
            }
        }
    }

    public interface IGoods {
        string GoodsToken { get; set; }
    }

    public class Goods : IGoods {
        public string GoodsToken { get; set; }
        
        public class GoodsValidator : AbstractValidator<IGoods> {
            public GoodsValidator() {
                RuleFor(m => m.GoodsToken).NotEmpty();
            }
        }
    }

    public interface ICompany {
        string CompanyName { get; set; }
        string CompanyToken { get; set; }
    }
    
    public class Company : ICompany {
        public string CompanyName { get; set; }
        public string CompanyToken { get; set; }

        public class CompanyValidator : AbstractValidator<ICompany> {
            public CompanyValidator() {
                RuleFor(m => m.CompanyName).NotEmpty();
                RuleFor(m => m.CompanyToken).NotEmpty();
            }
        }
    }       
}