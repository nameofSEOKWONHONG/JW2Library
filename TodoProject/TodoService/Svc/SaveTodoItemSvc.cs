using eXtensionSharp;
using FluentValidation;
using JWLibrary.Database;
using JWLibrary.ServiceExecutor;
using Microsoft.Data.SqlClient;
using RepoDb;
using TodoService.Data;

namespace TodoService
{
    /// <summary>
    ///     todo 저장
    /// </summary>
    public class SaveTodoItemSvc : ServiceExecutor<SaveTodoItemSvc, TODO, int>,
        ISaveTodoItemSvc
    {
        private readonly IGetTodoItemSvc _getTodoItemSvc;
        private TODO _exists;

        public SaveTodoItemSvc(IGetTodoItemSvc getTodoItemSvc)
        {
            _getTodoItemSvc = getTodoItemSvc;
            SetValidator<Validator>();
        }

        public override bool PreExecute()
        {
            using var executor = new ServiceExecutorManager<IGetTodoItemSvc>(_getTodoItemSvc);
            return executor.SetRequest(o => o.Request = Request.ID)
                .OnExecuted(o =>
                {
                    _exists = o.Result;
                    return true;
                });
        }

        public override void Execute()
        {
#if __SQLKATA__
#if __MYSQLKATA__
            JDatabaseResolver.Resolve<MySqlConnection>()
                .DbExecuteKata((db, query) => {
                    if (_exists.xIsNotNull()) {
                        var effectedRow = query.Query("TODO").Where("ID", _exists.ID).Delete();
                        if (effectedRow > 0) {
                            this.Result = query.Query("TODO").InsertGetId<int>(new {
                                TODO_TEXT = this.Request.TODO_TEXT,
                                W_DATE = DateTime.Now,
                                M_DATE = DateTime.Now
                            });
                        }
                    }
                    else {
                        this.Result = query.Query("TODO").InsertGetId<int>(new {
                            TODO_TEXT = this.Request.TODO_TEXT,
                            W_DATE = DateTime.Now,
                            M_DATE = DateTime.Now
                        });
                    }
                });
#else
            JDatabaseResolver.Resolve<SqlConnection>()
                .DbExecuteKata(db => {
                    if (_exists.xIsNotNull()) {
                        var effectedRow = db.Query("TODO").Where("ID", _exists.ID).Delete();
                        if (effectedRow > 0) {
                            this.Result = db.Query("TODO").InsertGetId<int>(new {
                                TODO_TEXT = this.Request.TODO_TEXT,
                                W_DATE = DateTime.Now,
                                M_DATE = DateTime.Now
                            });
                        }
                    }
                    else {
                        this.Result = db.Query("TODO").InsertGetId<int>(new {
                            TODO_TEXT = this.Request.TODO_TEXT,
                            W_DATE = DateTime.Now,
                            M_DATE = DateTime.Now
                        });
                    }
                });
#endif
#else
            JDatabaseResolver.Resolve<SqlConnection>()
                .DbExecute((db, tran) =>
                {
                    if (_exists.xIsNotNull())
                    {
                        if (db.Delete(_exists) > 0) Result = db.Insert<TODO, int>(Request);
                    }
                    else
                    {
                        Result = db.Insert<TODO, int>(Request);
                    }
                });
#endif
        }

        private class Validator : AbstractValidator<SaveTodoItemSvc>
        {
            public Validator()
            {
                RuleFor(m => m.Request).NotNull();
                //RuleFor(m => m.Request.ID).GreaterThan(0);
                RuleFor(m => m.Request.TODO_TEXT).NotEmpty();
            }
        }
    }
}