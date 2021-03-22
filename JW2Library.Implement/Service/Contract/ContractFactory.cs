using System;

namespace Service.Contract {
    public class ContractFactory {
        private ContractFactory() {
        }

        public static IContract CreateInstance(ENUM_CONTRACT_TYPE contractType) {
            switch (contractType) {
                case ENUM_CONTRACT_TYPE.BOOK:
                    return new BookContract();
                case ENUM_CONTRACT_TYPE.MOVIE:
                    return new MovieContract();
                case ENUM_CONTRACT_TYPE.TICKET:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}