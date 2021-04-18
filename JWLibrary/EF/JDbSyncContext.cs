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
        void Insert(Action<DbContext> action);
        void Update(Action<DbContext> action);
        void Delete(Action<DbContext> action);
    }
    
    public class JDbSyncContext : IJDbSyncContext {
        private C5.IList<DbContext> _list = new JList<DbContext>();
        public JDbSyncContext(IEnumerable<DbContext> dbcontexts) {
            _list.AddRange(dbcontexts);
        }

        public void Insert(Action<DbContext> action) {
            _list.jForeach(context => {
                action(context);
                context.SaveChanges();
            });
        }

        public void Update(Action<DbContext> action) {
            _list.jForeach(context => {
                action(context);
                context.SaveChanges();
            });
        }

        public void Delete(Action<DbContext> action) {
            _list.jForeach(context => {
                action(context);
                context.SaveChanges();
            });
        }

        public void Dispose() {
            _list.jForeach(context => {
                context.Dispose();
            });
        }
    }

    public class Demo {
        public void Test() {
            
        }
    }
}