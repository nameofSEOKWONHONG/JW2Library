using FluentValidation;
using JWLibrary.Database;
using JWLibrary.ServiceExecutor;
using Microsoft.Data.SqlClient;
using SqlKata.Execution;
using TodoService.Data;

namespace AccountService.Svc {

    public class GetAccountSvc : ServiceExecutor<GetAccountSvc, int, USER>,
        IGetAccountSvc {
        public GetAccountSvc() {
            
        }

        public override void Execute() {
            JDatabaseResolver.Resolve<SqlConnection>()
                .DbExecuteKata((db, q) => {
                    this.Result = q.Query("TESTDB.DBO.USER").Where("ID", this.Request).First<USER>();
                });
        }
        
        public class Validator : AbstractValidator<GetAccountSvc> {
            public Validator() {
                RuleFor(m => m.Request).GreaterThan(0);
            }
        }
    }
    
}