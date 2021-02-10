using System.Collections.Generic;
using SqlKata.Compilers;

namespace JWLibrary.Database
{
    public class JDatabaseConst {
        public static readonly Dictionary<SQL_COMPILER_TYPE, Compiler> COMPILER_MAP =
            new Dictionary<SQL_COMPILER_TYPE, Compiler>
            {
                {SQL_COMPILER_TYPE.MSSQL, new SqlServerCompiler()},
                {SQL_COMPILER_TYPE.MYSQL, new MySqlCompiler()},
                {SQL_COMPILER_TYPE.ORACLE, new OracleCompiler()},
                {SQL_COMPILER_TYPE.POSTGRESQL, new PostgresCompiler()},
                {SQL_COMPILER_TYPE.SQLLITE, new SqliteCompiler()}
            };
    }
    
    public enum SQL_COMPILER_TYPE {
        MSSQL,
        MYSQL,
        ORACLE,
        FIREBIRD,
        SQLLITE,
        POSTGRESQL
    }       
}