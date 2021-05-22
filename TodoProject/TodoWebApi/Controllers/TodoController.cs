using System.Collections;
using System.Collections.Generic;
using System.Threading;
using JWLibrary.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TodoWebApi.Entities;
using TodoWebApi.Services;

namespace TodoWebApi.Controllers {
    public class TodoController : JControllerBase<TodoController> {
        private IGetTodoItemSvc _getTodoItemSvc;
        private IGetTodoItemsSvc _getTodoItemsSvc;
        private ISaveTodoItemSvc _saveTodoItemSvc;
        private IDeleteTodoItemSvc _deleteTodoItemSvc;
        public TodoController(ILogger<TodoController> logger, 
            IGetTodoItemSvc getTodoItemSvc,
            IGetTodoItemsSvc getTodoItemsSvc,
            ISaveTodoItemSvc saveTodoItemSvc,
            IDeleteTodoItemSvc deleteTodoItemSvc) : base(logger) {
            _getTodoItemSvc = getTodoItemSvc;
            _getTodoItemsSvc = getTodoItemsSvc;
            _saveTodoItemSvc = saveTodoItemSvc;
            _deleteTodoItemSvc = deleteTodoItemSvc;
        }

        [HttpGet]
        public TODO GetTodoItem(int id) => 
            this.CreateService<IGetTodoItemSvc, int, TODO>(_getTodoItemSvc, id);

        [HttpGet]
        public IEnumerable<TODO> GetTotoItems(string todoText = null) =>
            this.CreateService<IGetTodoItemsSvc, string, IEnumerable<TODO>>(_getTodoItemsSvc, todoText);

        [HttpPost]
        public int SaveTodoItem(TODO todo) =>
            this.CreateService<ISaveTodoItemSvc, TODO, int>(_saveTodoItemSvc, todo);

        [HttpPost]
        public bool DeleteTodoItem(int id) =>
            this.CreateService<IDeleteTodoItemSvc, int, bool>(_deleteTodoItemSvc, id);

        [HttpPost]
        public IEnumerable<bool> DeleteTodoItems(IEnumerable<int> keys) =>
            this.CreateBulkService<IDeleteTodoItemSvc, int, bool>(keys);
    }
}