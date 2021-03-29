using System.Collections.Generic;
using JWLibrary.Core;
using JWLibrary.ServiceExecutor;
using JWService.Data.Models;

namespace Service.Data {
    public interface IGetAccountByIdSvc : IServiceExecutor<RequestDto<int>, Account> {
    }

    public interface IGetAccountSvc : IServiceExecutor<Account, Account> {
    }

    public interface
        IGetAccountsSvc : IServiceExecutor<PagingRequestDto<Account>, PagingResultDto<IEnumerable<Account>>> {
    }

    public interface ISaveAccountSvc : IServiceExecutor<Account, bool> {
    }

    public interface IDeleteAccountSvc : IServiceExecutor<RequestDto<int>, bool> {
    }

    public interface IBatchService<TQ, TR> : IServiceExecutor<TQ, TR> {
    }
}