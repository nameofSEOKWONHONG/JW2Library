using System.Collections;
using System.Collections.Generic;
using JWLibrary.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TodoWebApi.Entities;
using TodoWebApi.Services;

namespace TodoWebApi.Controllers {
    public class TodoController : JControllerBase<TodoController> {
        private IGetTodoItemSvc _getTodoItemSvc;
        private IGetTodoItemsSvc _getTodoItemsSvc;
        public TodoController(ILogger<TodoController> logger, 
            IGetTodoItemSvc getTodoItemSvc,
            IGetTodoItemsSvc getTodoItemsSvc) : base(logger) {
            _getTodoItemSvc = getTodoItemSvc;
            _getTodoItemsSvc = getTodoItemsSvc;
        }

        [HttpGet]
        public TODO GetTodoItem(int id) => 
            this.CreateService<IGetTodoItemSvc, int, TODO>(_getTodoItemSvc, id);

        [HttpGet]
        public IEnumerable<TODO> GetTotoItems(string todoText = null) =>
            this.CreateService<IGetTodoItemsSvc, string, IEnumerable<TODO>>(_getTodoItemsSvc, todoText);
    }
}