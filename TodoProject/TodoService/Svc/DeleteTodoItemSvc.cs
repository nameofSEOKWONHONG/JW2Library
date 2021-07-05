using System.Linq;
using RepoDb;
using eXtensionSharp;
using FluentValidation;
using JWLibrary.Database;
using JWLibrary.ServiceExecutor;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using SqlKata.Execution;
using TodoService.Data;

namespace TodoService {
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
                .DbExecuteKata((db, query) => {
                    var todo = query.Query("TODO").Where("ID", Request).First<TODO>();
                    if (todo.xIsNotEmpty()) Result = query.Query("TODO").Where("ID", todo.ID).Delete() > 0;
                });
#else
            JDatabaseResolver.Resolve<SqlConnection>()
                .DbExecuteKata(db => {
                    var todo = db.Query("TODO").Where("ID", Request).First<TODO>();
                    if (todo.xIsNotEmpty()) Result = db.Query("TODO").Where("ID", todo.ID).Delete() > 0;
                });
#endif
#else
            JDatabaseResolver.Resolve<SqlConnection>()
                .DbExecute((db, tran) => {
                    var exists = db.Query<TODO>(m => m.ID == this.Request).First();
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