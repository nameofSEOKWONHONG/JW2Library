using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using eXtensionSharp;
using FluentValidation;
using JWLibrary.Database;
using JWLibrary.ServiceExecutor;
using SqlKata.Execution;
using TodoService.Data;

namespace AccountService.Svc {

    public class GetAccountsSvc : ServiceExecutor<GetAccountsSvc, USER, IEnumerable<USER>>, 
        IGetAccountsSvc {
        public GetAccountsSvc() {
            
        }

        public override void Execute() {
            JDatabaseResolver.Resolve<SqlConnection>()
                .DbExecuteKata((db, q) => {
                    var query = q.Query("TESTDB.DBO.USER");
                    if (this.Request.USER_ID.xIsNotNullOrEmpty()) {
                        query = query.WhereLike("USER_ID", this.Request.USER_ID);
                    }

                    if (this.Request.USER_NM.xIsNotNullOrEmpty()) {
                        query = query.WhereLike("USER_NM", this.Request.USER_NM);
                    }

                    this.Result = query.Get<USER>();
                });
        }
        
        public class Validator : AbstractValidator<GetAccountsSvc> {
            public Validator() {
                RuleFor(m => m.Request).NotNull();
                RuleFor(m => m.Request.USER_ID).NotEmpty();
                RuleFor(m => m.Request.USER_NM).NotEmpty();
            }
        }
    }
}