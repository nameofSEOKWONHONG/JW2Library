using System;
using System.Collections;
using System.Collections.Generic;
using JWLibrary.ServiceExecutor;

namespace Service.Config {
    public interface IGetConfigSvc : IServiceExecutor<string, string> {
        
    }

    public interface ISaveConfigSvc : IServiceExecutor<string, Tuple<bool, string>> {
        
    }

    public interface IGetListConfigSvc : IServiceExecutor<IEnumerable<string>, IEnumerable<string>> {
        
    }

    public interface IBulkSaveConfigSvc : IServiceExecutor<IEnumerable<string>, Tuple<bool, IEnumerable<string>>> {
        
    }
}