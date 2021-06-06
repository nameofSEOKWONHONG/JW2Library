using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using eXtensionSharp;
using MongoDB.Bson.IO;
using MySql.Data.MySqlClient;
using Npgsql;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace JWLibrary.Database {
    /// <summary>
    ///     Database Client Extension
    /// TODO:Transaction Commit 부분 변경해야 함.
    /// </summary>
    public class JDbClientBulkExtension {
        /// <summary>
        ///     bulk insert, use datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="bulkDatas"></param>
        /// <param name="tableName"></param>
        public void BulkInsert<T>(IDbConnection connection, IEnumerable<T> bulkDatas,
            string tableName = null)
            where T : class, new() {
            try {
                if (tableName.xIsNullOrEmpty()) throw new NullReferenceException("table name is null or empty.");

                var entity = new T();
                var dt = bulkDatas.xToDateTable();

                using (var bulkCopy = new SqlBulkCopy((SqlConnection) connection, SqlBulkCopyOptions.Default, null)) {
                    bulkCopy.DestinationTableName = tableName.xIsNullOrEmpty() ? entity.GetType().Name : tableName;
                    foreach (var property in entity.GetType().GetProperties())
                        bulkCopy.ColumnMappings.Add(property.Name, property.Name);

                    connection.Open();
                    bulkCopy.WriteToServer(dt);
                }
            }
            finally {
                connection.Close();
            }
        }

        public async Task BulkInsertAsync<T>(IDbConnection connection, IEnumerable<T> bulkDatas,
            string tableName = null)
            where T : class, new() {
            try {
                if (tableName.xIsNullOrEmpty()) throw new NullReferenceException("table name is null or empty.");

                var entity = new T();
                var dt = bulkDatas.xToDateTable();

                using (var bulkCopy = new SqlBulkCopy((SqlConnection) connection, SqlBulkCopyOptions.Default, null)) {
                    bulkCopy.DestinationTableName = tableName.xIsNullOrEmpty() ? entity.GetType().Name : tableName;
                    foreach (var property in entity.GetType().GetProperties())
                        bulkCopy.ColumnMappings.Add(property.Name, property.Name);

                    connection.Open();
                    await bulkCopy.WriteToServerAsync(dt);
                }
            }
            finally {
                connection.Close();
            }
        }
    }

    /// <summary>
    ///     create dbconnection only
    /// </summary>
    public class JDbClientExecutor : IDisposable {
        private bool _isThrow;
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        private Func<IDbConnection, IDbTransaction> _addTran;
        private Action<JDataCacheHandler> _cacheHandler;

        public JDbClientExecutor(IDbConnection connection) {
            _connection = connection;
        }

        public JDbClientExecutor BeginTran() {
            _addTran = connection => connection.BeginTransaction(); 
            return this;
        }
        
        #region [self impletment func method]

        /// <summary>
        ///     you can execute func method, use dbconnection, any code. (use dapper, ef, and so on...)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public JDbClientExecutor DbExecute(Action<IDbConnection, IDbTransaction> action) {
            try {
                _connection.xOpen();
                if (_addTran.xIsNotNull()) {
                    _transaction = _addTran(_connection);    
                }
                action(_connection, _transaction);
            }
            catch {
                if(_transaction.xIsNotNull()) _transaction.Rollback();
                _transaction = null;
                throw;
            }

            return this;
        }

        public JDbClientExecutor DbExecuteKata(Action<IDbConnection, QueryFactory> action) {
            try {
                _connection.xOpen();

                if (_addTran.xIsNotNull()) {
                    _transaction = _addTran(_connection);    
                }
                
                var queryFactory = SqlKataCompilerFactory.CreateInstance(_connection);
                if (queryFactory.xIsNull()) throw new NotImplementedException();

                action(_connection, queryFactory);
            }
            catch {
                if(_transaction.xIsNotNull()) _transaction.Rollback();
                _transaction = null;
                throw;
            }

            return this;
        }

        /// <summary>
        ///     async execute(select, update, delete, insert) db, use any code on func method. (use dapper, ef and so on...)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_connection"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task<JDbClientExecutor> DbExecutorAsync(Func<IDbConnection, Task> func) {
            try {
                _connection.xOpen();
                if (_addTran.xIsNotNull()) {
                    _transaction = _addTran(_connection);    
                }
                await func(_connection);
            }
            catch {
                if(_transaction.xIsNotNull()) _transaction.Rollback();
                _transaction = null;
                throw;
            }

            return this;
        }
        
        #endregion [self impletment func method]
        public void Dispose() {
            if (_transaction.xIsNotNull()) {
                _transaction.Commit();
            }
            if (_connection.xIsNotNull()) {
                _connection.xClose();
            }
        }
    }

    public static class JDbExtension {
        public static void xOpen(this IDbConnection connection) {
            if(connection.State != ConnectionState.Open) connection.Open();
        }

        public static void xClose(this IDbConnection connection) {
            if(connection.State == ConnectionState.Open) connection.Close();
        }
    }

    
    public class JDataCacheHandler {
        public JDataCacheHandler() {
            
        }

        public void SetCache(string key, params object[] param) {
            //if is exists
            //remove and insert
            //전제 조건 : 캐시 그룹이 명확하게 이루어져 있어야 한다.
            //즉, 해당 캐시가 단 하나의 사용자 또는 사용자 그룹에만 동작할 것이라는 전제가 꼭 필요함.
            //전역성의 데이터는 machine마다의 전역성을 포함하므로 메모리 캐시만으로 할 수 있지만
            //사용자 또는 사용자 그룹은 메모리캐시로 처리되면 안됨.
            //즉, 사용자 또는 사용자 그룹의 State이므로 DB로 저장되어야 하고 그 DB는 그룹이 되어 있는 형태이어야 한다.
            //예전 MS가 실패한게 머신마다 State를 만들려고 했었는데, 시스템이 점점 거대화해지고 State가 메모리 형태로 가질 수 있는 상태를 벗어나게 되므로
            //ScaleOut으로는 해결할 수 없게 되자 DB에 몰빵하게 되고 DB는 또 ScaleOut을 하기에 좋은 조건인가?
            //CPU는? Memeory는? 비용은? 라이센스는?
            //DB가 제일 비싸....
            //그래서 ScaleUp을 하게 되는데... 결국 DB긴 하다...
            //근데 하나의 DB만??? No!!! 다중DB
            //중요Biz는 Oracle, Mssql, DB2, 기타 로그성, 단순 저장형 데이터는 Mysql, Postgresql
            //결국 DB가 State를 저장한다. 모든 입력행위가 State를 생성하는 거고, 조회하는 행위 자체가 State임.
            //근데 결국 O(N)에 따라 데이터, 사용자가 많아지면 속도, 부하가 문제가 되고...
            //근데 또 비용은 아껴야 되고...
            //예전 DBA들이 항상 하던 이야기가 Oracle쓰라고... Oracle이 끝판왕이긴 하다...
            //근데 이제는 또 클라우드 시대 하닌가????
            //DB가 뭐가 중요함???
            //서버를 무한정 늘릴 수 있는데... 그것도 사용한 비용만큼만 돈 내면 되는데...
            //예전처럼 견적받아서 서버 구매하고 SacleOut할때 서비스 중단하고 서버 CPU교체하고 램 장착하고, 밤새면서 지랄지랄....하아...
            //이런 행위를 이제는 클릭 한번으로도 끝낼 수 있는데...(오바긴 하다...)
            //참, 모르겠다... 
            //뭐하자는 건지...
            //닷넷 개발자는 다 저 모양들이야...
            //자바는 그래도 좀 깨어 있긴하다...
            //node는 클라우드 기본 장착이드라...
            //근데 SI개발자는 또 다 저모양이야...
            //아우 짜증난다. 진짜...
            //그래서 빨리 돈 모아서 은퇴해야돼...
            //하고 싶은 걸 하고 살아야지... 이놈에 스트레스 어떻게 버텨...
        }

        public void GetCache<T>(string key) where T : class {
            //get data
        }
    }

    // TODO : 쓸것인가 말것인가... using 안쓸려면 이걸로 해야 하는데...
    // public class JTransaction {
    //     private IDbConnection _connection;
    //     private ConcurrentStack<IDbTransaction>
    //         _transactions = new ConcurrentStack<IDbTransaction>();
    //
    //     public int Count {
    //         get {
    //             return _transactions.Count();
    //         }
    //     }
    //
    //     public JTransaction(IDbConnection connection) {
    //         _connection = connection;
    //     }
    //
    //     public void BeginTran() {
    //         IDbTransaction tran = _connection.BeginTransaction();
    //         _transactions.Push(tran);
    //     }
    //
    //     public void Commit() {
    //         _transactions.xFor(tran => {
    //             tran.Commit();
    //         });
    //         
    //         (0, _transactions.Count()).xForEach(i => {
    //             IDbTransaction tran = null;
    //             if (_transactions.TryPop(out tran)) {
    //                 tran.Commit();
    //             }
    //         });
    //     }
    //
    //     public void Rollback() {
    //         _transactions.xFor(tran => {
    //             tran.Rollback();
    //         });
    //         
    //         (0, _transactions.Count()).xForEach(i => {
    //             IDbTransaction tran = null;
    //             if (_transactions.TryPop(out tran)) {
    //                 tran.Rollback();
    //             }
    //         });
    //     }
    //     
    // }
}