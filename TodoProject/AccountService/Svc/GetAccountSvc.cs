using FluentValidation;
using JWLibrary.Database;
using JWLibrary.ServiceExecutor;
using Microsoft.Data.SqlClient;
using SqlKata.Execution;
using TodoService.Data;

namespace AccountService.Svc {

    public class GetAccountSvc : ServiceExecutor<GetAccountSvc, string, USER>,
        IGetAccountSvc {
        public GetAccountSvc() {
            
        }

        public override void Execute() {
            JDatabaseResolver.Resolve<SqlConnection>()
                .DbExecuteKata((db, q) => {
                    this.Result = q.Query("USER").Where("USER_ID", this.Request).FirstOrDefault<USER>();
                });
        }
        
        public class Validator : AbstractValidator<GetAccountSvc> {
            public Validator() {
                RuleFor(m => m.Request).NotEmpty();
            }
        }
    }
    
}