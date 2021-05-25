using System.Collections.Generic;
using eXtensionSharp;
using JWLibrary.Database;
using JWLibrary.ServiceExecutor;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using NetFabric.Hyperlinq;
using RepoDb;
using SqlKata.Execution;
using TodoWebApi.Entities;

namespace TodoWebApi.Services {
    
    public class TransactionSampleSvc : ServiceExecutor<TransactionSampleSvc, bool, XList<TODO>>,
        ITransactionSampleSvc {
        public override void Execute() {
            this.Result = new XList<TODO>();
            var sql = "SELECT * FROM TODO";

            using (var dispose1 = JDatabaseResolver.Resolve<SqlConnection>()
                .AddTran()
                .DbExecute((c, t) => {
                    this.Result.AddAll(c.ExecuteQuery<TODO>(sql, transaction: t));
                }))
            using (var dispose2 = JDatabaseResolver.Resolve<MySqlConnection>()
                .AddTran()
                .DbExecute((c1, t1) => {
                    this.Result.AddAll(c1.ExecuteQuery<TODO>(sql, transaction: t1));
                }))
                ;
        }
    }
}