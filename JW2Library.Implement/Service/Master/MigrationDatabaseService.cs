using JWLibrary.Database;
using JWLibrary.ServiceExecutor;
using JWLibrary.Util;
using NiL.JS.Extensions;
using RepoDb;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

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

        private async Task InitDatabaseAsync(IDbConnection con) {
            var sql = string.Empty;
            "./Master/Query/CREATE_DATABASE.js".ExecuteJavascriptFile(pre => {
                pre.DefineVariable("DBNAME").Assign("JWLIBRARY");
            }, end => {
                sql = end.GetVariable("sql").As<string>();
            });

            await con.ExecuteNonQueryAsync(sql);
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

        private async Task InitTableAsync(IDbConnection con) {
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

            await con.ExecuteNonQueryAsync(createTableSql);
        }

        private bool IsExistsTable(IDbConnection con) {
            var existsSql = string.Empty;
            "./Master/Query/EXISTS_TABLE.js".ExecuteJavascriptFile(end => {
                existsSql = end.GetVariable("sql").As<string>();
            });

            var result = con.ExecuteScalar<int>(existsSql);
            return result > 0;
        }

        private async Task<int> IsExistsTableAsync(IDbConnection con) {
            var existsSql = string.Empty;
            "./Master/Query/EXISTS_TABLE.js".ExecuteJavascriptFile(end => {
                existsSql = end.GetVariable("sql").As<string>();
            });

            return await con.ExecuteScalarAsync<int>(existsSql);
        }
        #endregion

        public override void Execute() {
            string sql = string.Empty;
            JDataBase.Resolve<SqlConnection>()
                .DbExecutor<bool>((con) => {
                    InitDatabase(con);
                    InitTable(con);
                    this.Result = IsExistsTable(con);
                });
        }

        public override async Task ExecuteAsync() {
            string sql = string.Empty;
            await JDataBase.Resolve<SqlConnection>()
                .DbExecutorAsync<bool>(async con => {
                    await InitDatabaseAsync(con);
                    await InitTableAsync(con);
                    var r = await IsExistsTableAsync(con);
                    this.Result = r > 0;
                });
        }
    }
}