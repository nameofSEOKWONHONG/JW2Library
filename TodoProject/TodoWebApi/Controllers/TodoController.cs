using System.Collections;
using System.Collections.Generic;
using System.Threading;
using JWLibrary.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TodoWebApi.Entities;
using TodoWebApi.Services;

namespace TodoWebApi.Controllers {
    public class TodoController : JController<TodoController> {
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

        /// <summary>
        /// todo 조회
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public TODO GetTodoItem(int id) => 
            this.CreateService<IGetTodoItemSvc, int, TODO>(_getTodoItemSvc, id);

        /// <summary>
        /// todo 리스트 조회
        /// </summary>
        /// <param name="todoText"></param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<TODO> GetTotoItems(string todoText = null) =>
            this.CreateService<IGetTodoItemsSvc, string, IEnumerable<TODO>>(_getTodoItemsSvc, todoText);

        /// <summary>
        /// todo 저장
        /// </summary>
        /// <param name="todo"></param>
        /// <returns></returns>
        [HttpPost]
        public int SaveTodoItem(TODO todo) =>
            this.CreateService<ISaveTodoItemSvc, TODO, int>(_saveTodoItemSvc, todo);

        /// <summary>
        /// todo 삭제
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public bool DeleteTodoItem(int id) =>
            this.CreateService<IDeleteTodoItemSvc, int, bool>(_deleteTodoItemSvc, id);

        /// <summary>
        /// todo bulk 삭제
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<bool> DeleteTodoItems(IEnumerable<int> keys) =>
            this.CreateBulkService<IDeleteTodoItemSvc, int, bool>(keys);
    }
}