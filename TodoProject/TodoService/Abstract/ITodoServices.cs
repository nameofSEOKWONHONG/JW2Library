using System.Collections;
using System.Collections.Generic;
using eXtensionSharp;
using JWLibrary;
using JWLibrary.ServiceExecutor;
using TodoService.Data;

/*
 * todo 인터페이스
 */
namespace TodoService {
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

    public interface ITransactionSampleSvc : IServiceExecutor<bool, XList<TODO>> {
        
    }
    
}