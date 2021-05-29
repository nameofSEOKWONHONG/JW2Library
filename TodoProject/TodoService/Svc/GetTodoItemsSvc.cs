using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using eXtensionSharp;
using JWLibrary.Database;
using JWLibrary.ServiceExecutor;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using MySql.Data.MySqlClient;
using SqlKata.Execution;
using TodoService.Data;

namespace TodoService {
    /// <summary>
    /// todo list 조회
    /// </summary>
    public class GetTodoItemsSvc : ServiceExecutor<GetTodoItemsSvc, string, IEnumerable<TODO>>,
        IGetTodoItemsSvc {
        public GetTodoItemsSvc() {
            
        }

        public override void Execute() {
#if __SQLKATA__
#if __MYSQLKATA__
            JDatabaseResolver.Resolve<MySqlConnection>()
                .DbExecuteKata((db, query) => {
                    this.Result = query.Query("TODO")
                        .WhereStarts("TODO_TEXT", this.Request.xSafe(), true)//WHERE TODO_TEXT LIKE '[TEXT]%'
                        .Get<TODO>();
                });
#else
            JDatabaseResolver.Resolve<SqlConnection>()
                .DbExecuteKata((db, query) => {
                    this.Result = db.Query("TODO")
                        .WhereStarts("TODO_TEXT", this.Request.xValue(), true)//WHERE TODO_TEXT LIKE '[TEXT]%'
                        .Get<TODO>();
                });
#endif       
#else
            JDatabaseResolver.Resolve<SqlConnection>()
                .DbExecute((db, tran) => {
                    this.Result = db.GetList<TODO>($"WHERE TODO_TEXT LIKE '%{this.Request}%'");
                });
#endif
        }
    }
}