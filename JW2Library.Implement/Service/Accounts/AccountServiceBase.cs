using JWLibrary.ServiceExecutor;

namespace Service.Accounts {
    public abstract class AccountServiceBase<TOwner, TRequest, TResult> : ServiceExecutor<TOwner, TRequest, TResult>
        where TOwner : AccountServiceBase<TOwner, TRequest, TResult> {
    }
}