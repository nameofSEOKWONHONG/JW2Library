using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    }
    
    /// <summary>
    /// 원본 데이터를 다른 DB로 동기화
    /// </summary>
    public class JDbSyncContext : IJDbSyncContext {
        private DbContext _srcDbContext = null;
        private C5.IList<DbContext> _destDbContexts = new JList<DbContext>();
        
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="srcDbContext">원본 db context</param>
        /// <param name="destDbContext">복사 대상 db context</param>
        public JDbSyncContext(DbContext srcDbContext, IEnumerable<DbContext> destDbContext) {
            _srcDbContext = srcDbContext;
            _destDbContexts.AddRange(destDbContext);
        }

        public void Insert<T>(Func<DbContext, T> func) where T : class {
            var entity = func(_srcDbContext);
            _srcDbContext.Add<T>(entity);
            _srcDbContext.SaveChanges();
            
            _destDbContexts.jForeach(context => {
                context.Add<T>(entity);
                context.SaveChanges();
            });
        }

        public void Update<T>(Func<DbContext, T> func) where T : class {
            var entity = func(_srcDbContext);
            _srcDbContext.Update<T>(entity);
            _srcDbContext.SaveChanges();
            
            _destDbContexts.jForeach(context => {
                context.Update<T>(entity);
                context.SaveChanges();
            });
        }

        public void Delete<T>(Func<DbContext, T> func) where T : class {
            var entity = func(_srcDbContext);
            _srcDbContext.Remove<T>(entity);
            _srcDbContext.SaveChanges();
            
            _destDbContexts.jForeach(context => {
                context.Remove<T>(entity);
                context.SaveChanges();
            });
        }

        public void Dispose() {
            _destDbContexts.jForeach(context => {
                context.Dispose();
            });
        }
    }
}