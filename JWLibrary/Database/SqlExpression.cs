using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using eXtensionSharp;
using NetFabric.Hyperlinq;

namespace JWLibrary.Database {
    public class SqlExpression {
        public SqlExpression() {
            
        }
        
        public SqlExpression<T> Select<T>(Expression<Func<T, object>> predicate = null) 
            where T : class {
            if (predicate.xIsNotNull()) return QuerySelect(predicate);
            else return QuerySelectAll<T>();
        }

        private SqlExpression<T> QuerySelect<T>(Expression<Func<T, object>> predicate) where T : class {
            var body = (predicate.Body as NewExpression);
            var select = $"SELECT {string.Join(", ", body.Members.Select(m => m.Name))}";
            var from = $"FROM {typeof(T).Name}";
            return new SqlExpression<T>(select, from); 
        }

        private SqlExpression<T> QuerySelectAll<T>() where T : class {
            var publics = typeof(T).GetProperties();
            var columns = new XList<string>();
            publics.xForEach(column => {
                columns.Add(column.Name);
            });

            var select = $"SELECT {string.Join(", ", columns)}";
            var from  = $"FROM {typeof(T).Name}";
            return new SqlExpression<T>(select, from);
        }

        public SqlExpression<Ta, Tb> Join<Ta, Tb>(Expression<Func<Ta, Tb, object>> predicate) where Ta : class where Tb : class {
            var taAlias = predicate.Parameters.First().Name;
            var tbAlias = predicate.Parameters.Last().Name;

            dynamic body = predicate.Body;
            var memberlist = new XList<string>();
            foreach (var arg in body.Arguments) {
                memberlist.Add($"{arg.Expression.Name}.{arg.Member.Name}");
            }
            
            var select = $"SELECT {string.Join(", ", memberlist)}";
            
            //TODO : JOIN ON 부분 작업해야 함.
            var from = $"FROM {typeof(Ta).Name} {taAlias} JOIN {typeof(Tb).Name} {tbAlias} ON {predicate}";
            return new SqlExpression<Ta, Tb>(select, from);
        }
    }

    public class SqlExpression<Ta, Tb> where Ta : class where Tb : class {
        private string _select;
        private string _from;
        public SqlExpression(string select, string from) {
            _select = select;
            _from = from;
        }
        
        //where & and 구현하기
    }

    public class SqlExpression<T> where T : class {
        private XList<Expression> _list = new XList<Expression>();
        private string _select = string.Empty;
        private string _from = string.Empty;
        private XList<string> _where = new XList<string>();
        
        public SqlExpression(string select, string from) {
            _select = select;
            _from = from;
        }
        
        public SqlExpression<T> Where(Expression<Func<T, bool>> predicate) {
            dynamic body = predicate.Body;
            var sql = $"WHERE {body.Left.Member.Name} {GetExpressionType(body.NodeType)} {GetDbValue(body.Right.Value)}";
            _where.Add(sql);
            return this;
        }
        
        public SqlExpression<T> Join<Ta, Tb>(Expression<Func<Ta, Tb, object>> predicate) where Ta : class where Tb : class {
            _from = _from + $"FROM {typeof(Ta).Name} A JOIN {typeof(Ta).Name} B ON";
            return this;
        }

        public SqlExpression<T> And(Expression<Func<T, bool>> predicate) {
            dynamic body = predicate.Body;

            var sql = new StringBuilder();
            sql.Append("AND ");
            
            if (body.NodeType == ExpressionType.Call) {
                var args = body.Arguments as ReadOnlyCollection<Expression>;
                var commandNodeType = args.Last().NodeType;
                if (commandNodeType == ExpressionType.Constant) {
                    sql.Append(SqlIn(args));
                }
                else if (commandNodeType == ExpressionType.NewArrayInit) {
                    sql.Append(SqlLike(args));
                }
            }
            else {
                sql.Append($"{body.Left.Member.Name} {GetExpressionType(body.NodeType)} {GetDbValue(body.Right.Value)}");
            }
            _where.Add(sql.ToString());
            return this;
        }


        
        public SqlExpression<T> Or(Expression<Func<T, bool>> predicate) {
            dynamic body = predicate.Body;

            var sql = new StringBuilder();
            sql.Append("OR ");
            
            if (body.NodeType == ExpressionType.Call) {
                var args = body.Arguments as ReadOnlyCollection<Expression>;
                var commandNodeType = args.Last().NodeType;
                if (commandNodeType == ExpressionType.Constant) {
                    sql.Append(SqlIn(args));
                }
                else if (commandNodeType == ExpressionType.NewArrayInit) {
                    sql.Append(SqlLike(args));
                }
            }
            else {
                sql.Append($"{body.Left.Member.Name} {GetExpressionType(body.NodeType)} {GetDbValue(body.Right.Value)}");
            }
            _where.Add(sql.ToString());
            return this;
        }
        
        public string Build() {
            StringBuilder sb = new StringBuilder(10);
            sb.AppendLine(_select);
            sb.AppendLine(_from);
            _where.xForEach(where => {
                sb.AppendLine(where);
            });
            return sb.ToString();
        }

        #region [util]
        private string SqlIn(dynamic args) {
            var sql = string.Empty;
            foreach (var arg in args) {
                dynamic dyarg = arg; 
                if (arg.NodeType == ExpressionType.Lambda) {
                    var name = dyarg.Body.Member.Name;
                    sql = sql + $"{name} IN (";
                }
                else if(arg.NodeType == ExpressionType.Constant) {
                    sql = sql + $"{GetDbValue(dyarg.Value)})";
                }
            }

            return sql;
        }

        private string SqlLike(dynamic args) {
            var sql = string.Empty;
            foreach (var arg in args) {
                dynamic dyarg = arg; 
                if (arg.NodeType == ExpressionType.Lambda) {
                    var argBody = dyarg.Body;
                    var name = argBody.Operand.Member.Name;
                    sql = sql + $"{name} LIKE (";
                }
                else if(arg.NodeType == ExpressionType.NewArrayInit) {
                    var inlist = new List<object>();
                    foreach (var expression in dyarg.Expressions) {
                        inlist.Add(GetDbValue(expression.Operand.Value));
                    }

                    sql = sql + $"{string.Join(", ", inlist)})";
                }
            }

            return sql;
        }
        
        private string GetExpressionType(ExpressionType eptype) {
            switch (eptype) {
                case ExpressionType.Equal: return "=";
                case ExpressionType.NotEqual: return "<>";
            }

            return string.Empty;
        }

        private object GetDbValue(object o) {
            if (o.GetType() == typeof(string)) {
                return $"'{o}'";
            }

            return o;
        }
        #endregion
    }
    
    public class Sql {
        public static bool In<T>(Func<T, object> predicate, params object[] dest) {
            return true;
        }

        public static bool Like<T>(Func<T, object> predicate, object obj) {
            return true;
        }
    }
}