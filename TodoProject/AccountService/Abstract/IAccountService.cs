using System.Collections.Generic;
using JWLibrary.ServiceExecutor;
using TodoService.Data;

namespace AccountService {
    public interface IGetAccountSvc : IServiceExecutor<int, USER> {
        
    }
    
    public interface IGetAccountsSvc : IServiceExecutor<USER, IEnumerable<USER>> {
        
    }
    
    public interface ISaveAccountSvc : IServiceExecutor<USER, int> {
        
    }
    
    public interface IDeleteAccountSvc : IServiceExecutor<int, bool> {
        
    }
}