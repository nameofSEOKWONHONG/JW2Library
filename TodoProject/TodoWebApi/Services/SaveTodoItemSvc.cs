using Dapper;
using eXtensionSharp;
using FluentValidation;
using JWLibrary.Database;
using JWLibrary.ServiceExecutor;
using Microsoft.Data.SqlClient;
using TodoWebApi.Entities;

namespace TodoWebApi.Services {
    /// <summary>
    /// todo 저장
    /// </summary>
    public class SaveTodoItemSvc : ServiceExecutor<SaveTodoItemSvc, TODO, int>,
        ISaveTodoItemSvc {
        private TODO _exists;
        private readonly IGetTodoItemSvc _getTodoItemSvc;

        public SaveTodoItemSvc(IGetTodoItemSvc getTodoItemSvc) {
            _getTodoItemSvc = getTodoItemSvc;
            SetValidator<Validator>();
        }

        public override bool PreExecute() {
            using var executor = new ServiceExecutorManager<IGetTodoItemSvc>(_getTodoItemSvc);
            return executor.SetRequest(o => o.Request = Request.ID)
                .OnExecuted(o => {
                    _exists = o.Result;
                    return true;
                });
        }

        public override void Execute() {
            JDatabaseResolver.Resolve<SqlConnection>()
                .DbExecutor(db => {
                    if (_exists.xIsNotNull()) {
                        if (db.Delete<TODO>(_exists) > 0) {
                            this.Result = db.Insert<TODO>(this.Request).Value;    
                        }
                    }
                    else {
                        this.Result =  db.Insert<TODO>(this.Request).Value;
                    }
                });
        }

        private class Validator : AbstractValidator<SaveTodoItemSvc> {
            public Validator() {
                RuleFor(m => m.Request).NotNull();
                //RuleFor(m => m.Request.ID).GreaterThan(0);
                RuleFor(m => m.Request.TODO_TEXT).NotEmpty();
            }
        }
    }
}