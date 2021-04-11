using System.Collections.Generic;
using JWLibrary.Core;
using Microsoft.Scripting.Interpreter;
using SqlKata.Compilers;

namespace JWLibrary.Database {
    public class JDatabaseConst {
        public static readonly Dictionary<ENUM_SQL_COMPILER_TYPE, Compiler> COMPILER_MAP =
            new() {
                {ENUM_SQL_COMPILER_TYPE.MSSQL, new SqlServerCompiler()},
                {ENUM_SQL_COMPILER_TYPE.MYSQL, new MySqlCompiler()},
                {ENUM_SQL_COMPILER_TYPE.ORACLE, new OracleCompiler()},
                {ENUM_SQL_COMPILER_TYPE.POSTGRESQL, new PostgresCompiler()},
                {ENUM_SQL_COMPILER_TYPE.SQLLITE, new SqliteCompiler()}
            };
    }

    public class ENUM_SQL_COMPILER_TYPE : JENUM_BASE<ENUM_SQL_COMPILER_TYPE> {
        public static readonly ENUM_SQL_COMPILER_TYPE MSSQL = define("MSSQL");
        public static readonly ENUM_SQL_COMPILER_TYPE MYSQL = define("MYSQL");
        public static readonly ENUM_SQL_COMPILER_TYPE ORACLE = define("ORACLE");
        public static readonly ENUM_SQL_COMPILER_TYPE FIREBIRD = define("FIREBIRD");
        public static readonly ENUM_SQL_COMPILER_TYPE SQLLITE = define("SQLLITE");
        public static readonly ENUM_SQL_COMPILER_TYPE POSTGRESQL = define("POSTGRESQL");
    }
}