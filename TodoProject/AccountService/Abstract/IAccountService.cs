using System.Collections.Generic;
using JWLibrary.ServiceExecutor;
using TodoService.Data;

namespace AccountService {
    public interface IGetAccountSvc : IServiceExecutor<string, USER> {
        
    }
    
    public interface IGetAccountsSvc : IServiceExecutor<USER, IEnumerable<USER>> {
        
    }
    
    public interface ISaveAccountSvc : IServiceExecutor<USER, int> {
        
    }
    
    public interface IDeleteAccountSvc : IServiceExecutor<string, bool> {
        
    }
}