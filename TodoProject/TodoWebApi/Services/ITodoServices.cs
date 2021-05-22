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

    public interface ISaveTodoItemSvc : IServiceExecutor<TODO, int> {
        
    }

    public interface IDeleteTodoItemSvc : IServiceExecutor<int, bool> {
        
    }

    public interface IDeleteTodoItemsSvc : IServiceExecutor<IEnumerable<int>, bool> {
        
    }
}