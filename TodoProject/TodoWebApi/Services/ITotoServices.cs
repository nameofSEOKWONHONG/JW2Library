using System.Collections;
using System.Collections.Generic;
using JWLibrary;
using JWLibrary.ServiceExecutor;
using TodoWebApi.Entities;

namespace TodoWebApi.Services {
    public interface IGetTodoItemSvc : IServiceExecutor<int, TODO> {
        
    }

    public interface IGetTodoItemsSvc : IServiceExecutor<string, IEnumerable<TODO>> {
        
    }

    public interface IUpsertTodoItemSvc : IServiceExecutor<TODO, bool> {
        
    }

    public interface IDeleteTodoItemSvc : IServiceExecutor<int, bool> {
        
    }

    public interface IDeleteTodoITemsSvc : IServiceExecutor<IEnumerable<int>, bool> {
        
    }
}