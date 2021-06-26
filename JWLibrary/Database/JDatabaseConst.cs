using System.Collections.Generic;
using eXtensionSharp;
using Microsoft.Scripting.Interpreter;
using SqlKata.Compilers;

namespace JWLibrary.Database {
    public class JDatabaseConst {
        public static readonly Dictionary<ENUM_SQL_COMPILER_TYPE, Compiler> SQLKATA_COMPILERS =
            new() {
                {ENUM_SQL_COMPILER_TYPE.MSSQL, new SqlServerCompiler()},
                {ENUM_SQL_COMPILER_TYPE.MYSQL, new MySqlCompiler()},
                {ENUM_SQL_COMPILER_TYPE.ORACLE, new OracleCompiler()},
                {ENUM_SQL_COMPILER_TYPE.POSTGRESQL, new PostgresCompiler()},
                {ENUM_SQL_COMPILER_TYPE.SQLLITE, new SqliteCompiler()}
            };
    }

    public class ENUM_SQL_COMPILER_TYPE : XEnumBase<ENUM_SQL_COMPILER_TYPE> {
        public static readonly ENUM_SQL_COMPILER_TYPE MSSQL = Define("MSSQL");
        public static readonly ENUM_SQL_COMPILER_TYPE MYSQL = Define("MYSQL");
        public static readonly ENUM_SQL_COMPILER_TYPE ORACLE = Define("ORACLE");
        public static readonly ENUM_SQL_COMPILER_TYPE FIREBIRD = Define("FIREBIRD");
        public static readonly ENUM_SQL_COMPILER_TYPE SQLLITE = Define("SQLLITE");
        public static readonly ENUM_SQL_COMPILER_TYPE POSTGRESQL = Define("POSTGRESQL");
    }
}