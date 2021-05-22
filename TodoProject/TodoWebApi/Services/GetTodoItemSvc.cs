//no use System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using Dapper;
using FluentValidation;
using JWLibrary.Database;
using JWLibrary.ServiceExecutor;
using TodoWebApi.Entities;

namespace TodoWebApi.Services {
    /// <summary>
    /// Todo 조회
    /// </summary>
    public class GetTodoItemSvc : ServiceExecutor<GetTodoItemSvc, int, TODO>, IGetTodoItemSvc {
        public GetTodoItemSvc() {
            this.SetValidator<Validator>();
        }

        public override void Execute() {
            JDatabaseResolver.Resolve<SqlConnection>()
                .DbExecutor(db => {
                    this.Result = db.Get<TODO>(this.Request);
                });
        }
        
        private class Validator : AbstractValidator<GetTodoItemSvc> {
            public Validator() {
                //RuleFor(m => m.Request).GreaterThan(0);
            }
        }
    }
}