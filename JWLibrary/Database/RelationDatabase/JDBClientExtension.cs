using System;
using System.Data;
using System.Threading.Tasks;
using eXtensionSharp;
using SqlKata.Execution;

namespace JWLibrary.Database
{
    /// <summary>
    ///     create dbconnection only
    ///     2021.07.10 : 외부 캐시는 포기한다. repodb는 캐시 전략을 제공하므로 repodb에 의존적으로 처리한다.
    ///     캐시 전략이 포함된 프레임워크는 ef, dapper(dapper extension으로 제공-3자 라이브러리)등이 있으며
    ///     내부 구현으로 가져가지 않는다.
    /// </summary>
    public class JDbClientExecutor : IDisposable
    {
        private Func<IDbConnection, IDbTransaction> _addTran;
        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;

        public JDbClientExecutor(IDbConnection connection)
        {
            _connection = connection;
        }

        public void Dispose()
        {
            if (_transaction.xIsNotNull()) _transaction.Commit();
            if (_connection.xIsNotNull()) _connection.xClose();
        }

        public JDbClientExecutor BeginTran()
        {
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
        public JDbClientExecutor DbExecute(Action<IDbConnection, IDbTransaction> action)
        {
            try
            {
                _connection.xOpen();
                if (_addTran.xIsNotNull()) _transaction = _addTran(_connection);
                action(_connection, _transaction);
            }
            catch
            {
                if (_transaction.xIsNotNull())
                {
                    _transaction.Rollback();
                    _transaction.Dispose();
                    _transaction = null;
                }

                throw;
            }

            return this;
        }

        public JDbClientExecutor DbExecuteKata(Action<IDbConnection, QueryFactory> action)
        {
            try
            {
                _connection.xOpen();

                if (_addTran.xIsNotNull()) _transaction = _addTran(_connection);

                var queryFactory = SqlKataCompilerFactory.CreateInstance(_connection);
                if (queryFactory.xIsNull()) throw new NotImplementedException();

                action(_connection, queryFactory);
            }
            catch
            {
                if (_transaction.xIsNotNull())
                {
                    _transaction.Rollback();
                    _transaction.Dispose();
                    _transaction = null;
                }

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
        public async Task<JDbClientExecutor> DbExecutorAsync(Func<IDbConnection, Task> func)
        {
            try
            {
                _connection.xOpen();
                if (_addTran.xIsNotNull()) _transaction = _addTran(_connection);
                await func(_connection);
            }
            catch
            {
                if (_transaction.xIsNotNull())
                {
                    _transaction.Rollback();
                    _transaction.Dispose();
                    _transaction = null;
                }

                throw;
            }

            return this;
        }

        #endregion [self impletment func method]
    }

    public static class JDbExtension
    {
        public static void xOpen(this IDbConnection connection)
        {
            if (connection.State != ConnectionState.Open) connection.Open();
        }

        public static void xClose(this IDbConnection connection)
        {
            if (connection.State == ConnectionState.Open) connection.Close();
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