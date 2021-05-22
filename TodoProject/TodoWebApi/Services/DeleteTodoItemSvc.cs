using System.Data;
using Dapper;
using eXtensionSharp;
using FluentValidation;
using JWLibrary.Database;
using JWLibrary.ServiceExecutor;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using SharpCompress.Archives;
using TodoWebApi.Entities;

namespace TodoWebApi.Services {
    public class DeleteTodoItemSvc : ServiceExecutor<DeleteTodoItemSvc, int, bool>, IDeleteTodoItemSvc {
        private TODO _exists;
        private readonly IGetTodoItemSvc _getTodoItemSvc;
        
        public DeleteTodoItemSvc(IGetTodoItemSvc getTodoItemSvc) {
            _getTodoItemSvc = getTodoItemSvc;
            this.SetValidator<Validator>();
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
            JDatabaseResolver.Resolve<SqlConnection>()
                .DbExecutor(db => {
                    var exists = db.Get<TODO>(this.Request);
                    if (exists.xIsNotNull()) {
                        this.Result = db.Delete<TODO>(exists) > 0;
                    }   
                });
        }
        
        private class Validator : AbstractValidator<DeleteTodoItemSvc> {
            public Validator() {
                RuleFor(m => m.Request).GreaterThan(0);
            }
        }
    }
}