using System.Collections.Generic;
using eXtensionSharp;
using JWLibrary.Database;
using JWLibrary.ServiceExecutor;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using RepoDb;
using SqlKata.Execution;
using TodoService.Data;

namespace TodoService {
    
    public class TransactionSampleSvc : ServiceExecutor<TransactionSampleSvc, bool, List<TODO>>,
        ITransactionSampleSvc {
        public override void Execute() {
            this.Result = new List<TODO>();
            var sql = "SELECT * FROM TODO";

            JDatabaseResolver.Resolve<SqlConnection>()
                .BeginTran()
                .DbExecute((c, t) =>
                {
                    this.Result.AddRange(c.ExecuteQuery<TODO>(sql, transaction: t));

                    JDatabaseResolver.Resolve<MySqlConnection>()
                        .BeginTran()
                        .DbExecute((c1, t1) =>
                        {
                            this.Result.AddRange(c1.ExecuteQuery<TODO>(sql, transaction: t1));
                        });
                });
        }
    }
}