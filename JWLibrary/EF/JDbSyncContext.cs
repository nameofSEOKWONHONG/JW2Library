﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using JWLibrary.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Scripting.Utils;
using NetFabric.Hyperlinq;
using NiL.JS.Expressions;

namespace JWLibrary.EF {
    public interface IJDbSyncContext : IDisposable{
        void Insert<T>(Func<DbContext, T> func) where T : class;
        void Update<T>(Func<DbContext, T> func) where T : class;
        void Delete<T>(Func<DbContext, T> func) where T : class;
        Task InsertAsync<T>(Func<DbContext, T> func) where T : class;
        Task UpdateAsync<T>(Func<DbContext, T> func) where T : class;
        Task DeleteAsync<T>(Func<DbContext, T> func) where T : class;
    }
    
    /// <summary>
    /// 원본 데이터를 다른 DB로 동기화
    /// <remarks>원본 테이블과 동일한 엔티티에 대하여 동기화 한다.</remarks>
    /// <remarks>만약 원본 테이블의 Key가 AutoIncrement라면 동기화 대상 테이블의 Key는 AutoIncrement가 아니어야 한다.</remarks>
    /// <remarks>집계 대상 테이블의 데이터를 복사할 용도로 사용함. (대부분 대상 테이블은 인덱스만 있는 상태로 운영됨)</remarks>
    /// </summary>
    public class JDbSyncContext : IJDbSyncContext {
        private DbContext _srcDbContext = null;
        private C5.IList<DbContext> _destDbContexts = new JList<DbContext>();
        private bool _isRollback;
        
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="srcDbContext">원본 db context</param>
        /// <param name="destDbContext">복사 대상 db context</param>
        public JDbSyncContext(DbContext srcDbContext, IEnumerable<DbContext> destDbContext) {
            _srcDbContext = srcDbContext;
            _destDbContexts.AddRange(destDbContext);
            
            BeginTrasaction();
        }

        public void Insert<T>(Func<DbContext, T> func) where T : class {
            var entity = func(_srcDbContext);
            try {
                _srcDbContext.Add<T>(entity);
                _srcDbContext.SaveChanges();

                _destDbContexts.jForeach(context => {
                    context.Add<T>(entity);
                    context.SaveChanges();
                });
            }
            catch (Exception e) {
                _isRollback = true;
            }
        }
        
        public async Task InsertAsync<T>(Func<DbContext, T> func) where T : class {
            try {
                var entity = func(_srcDbContext);
                _srcDbContext.Add<T>(entity);
                await _srcDbContext.SaveChangesAsync();

                _destDbContexts.jForeachAsync(async context => {
                    context.Add<T>(entity);
                    await context.SaveChangesAsync();
                });
            }
            catch (Exception e) {
                _isRollback = true;
            }
        }

        public void Update<T>(Func<DbContext, T> func) where T : class {
            var entity = func(_srcDbContext);

            try {
                _srcDbContext.Update<T>(entity);
                _srcDbContext.SaveChanges();

                _destDbContexts.jForeach(context => {
                    context.Update<T>(entity);
                    context.SaveChanges();
                });
            }
            catch (Exception e) {
                _isRollback = true;
            }
        }

        public async Task UpdateAsync<T>(Func<DbContext, T> func) where T : class {
            try {
                var entity = func(_srcDbContext);
                _srcDbContext.Update<T>(entity);
                await _srcDbContext.SaveChangesAsync();
            
                _destDbContexts.jForeachAsync(async context => {
                    context.Update<T>(entity);
                    await context.SaveChangesAsync();
                });
            }
            catch (Exception e) {
                _isRollback = true;
            }
        }

        public void Delete<T>(Func<DbContext, T> func) where T : class {
            var entity = func(_srcDbContext);
            try {
                _srcDbContext.Remove<T>(entity);
                _srcDbContext.SaveChanges();

                _destDbContexts.jForeach(context => {
                    context.Remove<T>(entity);
                    context.SaveChanges();
                });
            }
            catch (Exception e) {
                _isRollback = true;
            }
        }

        public async Task DeleteAsync<T>(Func<DbContext, T> func) where T : class {
            var entity = func(_srcDbContext);
            try {
                _srcDbContext.Remove<T>(entity);
                await _srcDbContext.SaveChangesAsync();

                _destDbContexts.jForeachAsync(async context => {
                    context.Remove<T>(entity);
                    await context.SaveChangesAsync();
                });
            }
            catch (Exception e) {
                _isRollback = true;
            }
        }

        public void DoRollback() {
            _isRollback = true;
        }

        public void Dispose() {
            if (_isRollback) {
                RollbackTransaction();
            }
            else {
                CommitTransaction();
            }
            _srcDbContext.Dispose();
            _destDbContexts.jForeach(context => {
                context.Dispose();
            });
        }
        
        private void BeginTrasaction() {
            _srcDbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);
            _destDbContexts.jForeach(context => {
                context.Database.BeginTransaction(IsolationLevel.ReadUncommitted);
            });
        }

        private void CommitTransaction() {
            _srcDbContext.Database.CommitTransaction();
            _destDbContexts.jForeach(context => {
                context.Database.CommitTransaction();
            });
        }

        private void RollbackTransaction() {
            _srcDbContext.Database.RollbackTransaction();
            _destDbContexts.jForeach(context => {
                context.Database.RollbackTransaction();
            });
        }        
    }
}