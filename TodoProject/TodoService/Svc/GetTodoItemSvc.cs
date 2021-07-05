//no use System.Data.SqlClient;

using System.Linq;
using RepoDb;
using FluentValidation;
using JWLibrary.Database;
using JWLibrary.ServiceExecutor;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using SqlKata.Execution;
using TodoService.Data;

namespace TodoService {
    /// <summary>
    ///     Todo 조회
    /// </summary>
    public class GetTodoItemSvc : ServiceExecutor<GetTodoItemSvc, int, TODO>, IGetTodoItemSvc {
        public GetTodoItemSvc() {
            SetValidator<Validator>();
        }

        public override void Execute() {
#if __SQLKATA__
#if __MYSQLKATA__
            JDatabaseResolver.Resolve<MySqlConnection>()
                .DbExecuteKata((db, query) => {
                    this.Result = query.Query("TODO").Where("ID", Request).FirstOrDefault<TODO>();
                });
#else
            JDatabaseResolver.Resolve<SqlConnection>()
                .DbExecuteKata(db => {
                    this.Result = db.Query("TODO").Where("ID", Request).FirstOrDefault<TODO>();
                });
#endif
#else
            JDatabaseResolver.Resolve<SqlConnection>()
                .DbExecute((db, tran) => {
                    this.Result = db.Query<TODO>(m => m.ID == this.Request).First();
                });
#endif
        }

        private class Validator : AbstractValidator<GetTodoItemSvc> {
        }
    }
}