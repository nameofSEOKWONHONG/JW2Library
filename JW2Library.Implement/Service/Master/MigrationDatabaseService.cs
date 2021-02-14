using JWLibrary.Database;
using JWLibrary.ServiceExecutor;
using JWLibrary.Util;
using NiL.JS.Extensions;
using RepoDb;
using System.Data;
using System.Data.SqlClient;

namespace Service {
    public interface IMigrationDatabaseService : IServiceExecutor<bool, bool> { }

    public class MigrationDatabaseService : ServiceExecutor<MigrationDatabaseService, bool, bool>, IMigrationDatabaseService {
        #region [private]
        private void InitDatabase(IDbConnection con) {
            var sql = string.Empty;
            "./Master/Query/CREATE_DATABASE.js".ExecuteJavascriptFile(pre => {
                pre.DefineVariable("DBNAME").Assign("JWLIBRARY");
            }, end => {
                sql = end.GetVariable("sql").As<string>();
            });

            con.ExecuteNonQuery(sql);
        }

        private void InitTable(IDbConnection con) {
            var createUserSql = string.Empty;
            var createTableSql = string.Empty;

            "./Master/Query/CREATE_USER.js".ExecuteJavascriptFile(null, end => {
                createUserSql = end.GetVariable("sql").As<string>();
            });

            "./Master/Query/CREATE_TABLE.js".ExecuteJavascriptFile(pre => {
                pre.DefineVariable("CREATE_USER").Assign(createUserSql);
            }, end => {
                createTableSql = end.GetVariable("sql").As<string>();
            });

            con.ExecuteNonQuery(createTableSql);
        }

        private bool IsExistsTable(IDbConnection con) {
            var existsSql = string.Empty;
            "./Master/Query/EXISTS_TABLE.js".ExecuteJavascriptFile(end => {
                existsSql = end.GetVariable("sql").As<string>();
            });

            var result = con.ExecuteScalar<int>(existsSql);
            return result > 0;
        }
        #endregion

        public override void Execute() {
            string sql = string.Empty;
            JDataBase.Resolve<SqlConnection>()
                .DbExecutor<bool>((con) =>  {
                    InitDatabase(con);
                    InitTable(con);
                    this.Result = IsExistsTable(con);
                });
        }
    }
}