using eXtensionSharp;
using FluentValidation;
using JWLibrary.Database;
using JWLibrary.ServiceExecutor;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using SqlKata.Execution;
using TodoWebApi.Entities;

namespace TodoWebApi.Services {
    /// <summary>
    ///     todo 삭제
    /// </summary>
    public class DeleteTodoItemSvc : ServiceExecutor<DeleteTodoItemSvc, int, bool>, IDeleteTodoItemSvc {
        private readonly IGetTodoItemSvc _getTodoItemSvc;
        private TODO _exists;

        public DeleteTodoItemSvc(IGetTodoItemSvc getTodoItemSvc) {
            _getTodoItemSvc = getTodoItemSvc;
            SetValidator<Validator>();
        }

        public override bool PreExecute() {
            using var executor = new ServiceExecutorManager<IGetTodoItemSvc>(_getTodoItemSvc);
            return executor.SetRequest(o => o.Request = Request)
                .OnExecuted(o => {
                    _exists = o.Result;
                    return true;
                });
        }

        public override void Execute() {
#if __SQLKATA__
#if __MYSQLKATA__
            JDatabaseResolver.Resolve<MySqlConnection>()
                .DbExecutorKata(db => {
                    var todo = db.Query("TODO").Where("ID", Request).First<TODO>();
                    if (todo.xIsNotEmpty()) Result = db.Query("TODO").Where("ID", todo.ID).Delete() > 0;
                });
#else
            JDatabaseResolver.Resolve<SqlConnection>()
                .DbExecutorKata(db => {
                    var todo = db.Query("TODO").Where("ID", Request).First<TODO>();
                    if (todo.xIsNotEmpty()) Result = db.Query("TODO").Where("ID", todo.ID).Delete() > 0;
                });
#endif
#else
            JDatabaseResolver.Resolve<SqlConnection>()
                .DbExecutor(db => {
                    var exists = db.Get<TODO>(this.Request);
                    if (exists.xIsNotNull()) {
                        this.Result = db.Delete<TODO>(exists) > 0;
                    }   
                });
#endif
        }

        private class Validator : AbstractValidator<DeleteTodoItemSvc> {
            public Validator() {
                RuleFor(m => m.Request).GreaterThan(0);
            }
        }
    }
}