using System;
using System.Linq.Expressions;
using eXtensionSharp;

namespace JWLibrary.Database {
    public class JTableExpression<T> where T : class {
        public JTableExpression() {
            
        }

        public JTableExpression<T> PK(Expression<Func<T, object>> expression) {
            dynamic body = expression.Body;
            return this;
        }

        public JTableExpression<T> FK<TF>(Expression<Func<T, TF, object>> expression) {
            dynamic body = expression.Body;
            return this;
        }

        public JTableExpression<T> AutoIncrementKey(Expression<Func<T, object>> expression) {
            dynamic body = expression.Body;
            return this;
        }

        public JTableExpression<T> Index(Expression<Func<T, object>> expression) {
            dynamic body = expression.Body;
            return this;
        }

        public string Build() {
            var sql = string.Empty;

            var xsb = new XStringBuilder();

            return sql;
        }
    }
}