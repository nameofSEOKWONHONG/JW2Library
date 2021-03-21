using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using JWLibrary.Database;
using JWLibrary.ServiceExecutor;
using JWLibrary.Util;
using NiL.JS.Extensions;
using RepoDb;
using Service.QueryConst;

namespace Service {
    public interface IMigrationDatabaseService : IServiceExecutor<bool, bool> {
    }

    public class MigrationDatabaseService : ServiceExecutor<MigrationDatabaseService, bool, bool>,
        IMigrationDatabaseService {
        public override void Execute() {
            var sql = string.Empty;
            JDataBase.Resolve<SqlConnection>()
                .DbExecutor<bool>(con => {
                    InitDatabase(con);
                    InitTable(con);
                    Result = IsExistsTable(con);
                });
        }

        public override async Task ExecuteAsync() {
            var sql = string.Empty;
            await JDataBase.Resolve<SqlConnection>()
                .DbExecutorAsync<bool>(async con => {
                    await InitDatabaseAsync(con);
                    await InitTableAsync(con);
                    var r = await IsExistsTableAsync(con);
                    Result = r > 0;
                });
        }

        #region [private]

        private void InitDatabase(IDbConnection con) {
            var sql = string.Empty;
            MasterQuery.Self.CREATE_DATABASE.Execute(pre => { pre.DefineVariable("DBNAME").Assign("JWLIBRARY"); },
                end => { sql = end.GetVariable("sql").As<string>(); });

            con.ExecuteNonQuery(sql);
        }

        private async Task InitDatabaseAsync(IDbConnection con) {
            var sql = string.Empty;
            MasterQuery.Self.CREATE_DATABASE.Execute(pre => { pre.DefineVariable("DBNAME").Assign("JWLIBRARY"); },
                end => { sql = end.GetVariable("sql").As<string>(); });

            await con.ExecuteNonQueryAsync(sql);
        }

        private void InitTable(IDbConnection con) {
            var createUserSql = string.Empty;
            var createTableSql = string.Empty;

            MasterQuery.Self.CREATE_USER.Execute(null, end => { createUserSql = end.GetVariable("sql").As<string>(); });

            MasterQuery.Self.CREATE_TABLE.Execute(pre => { pre.DefineVariable("CREATE_USER").Assign(createUserSql); },
                end => { createTableSql = end.GetVariable("sql").As<string>(); });

            con.ExecuteNonQuery(createTableSql);
        }

        private async Task InitTableAsync(IDbConnection con) {
            var createUserSql = string.Empty;
            var createTableSql = string.Empty;

            MasterQuery.Self.CREATE_USER.Execute(null, end => { createUserSql = end.GetVariable("sql").As<string>(); });

            MasterQuery.Self.CREATE_TABLE.Execute(pre => { pre.DefineVariable("CREATE_USER").Assign(createUserSql); },
                end => { createTableSql = end.GetVariable("sql").As<string>(); });

            await con.ExecuteNonQueryAsync(createTableSql);
        }

        private bool IsExistsTable(IDbConnection con) {
            var existsSql = string.Empty;
            MasterQuery.Self.EXISTS_TABLE.Execute(end => { existsSql = end.GetVariable("sql").As<string>(); });

            var result = con.ExecuteScalar<int>(existsSql);
            return result > 0;
        }

        private async Task<int> IsExistsTableAsync(IDbConnection con) {
            var existsSql = string.Empty;
            MasterQuery.Self.EXISTS_TABLE.Execute(end => { existsSql = end.GetVariable("sql").As<string>(); });

            return await con.ExecuteScalarAsync<int>(existsSql);
        }

        #endregion
    }
}