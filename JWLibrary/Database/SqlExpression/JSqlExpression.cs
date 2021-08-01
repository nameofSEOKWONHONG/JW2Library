using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Dapper;
using eXtensionSharp;

namespace JWLibrary.Database
{
    public class JSqlExpression
    {
        public JSqlExpression<T> Select<T>(Expression<Func<T, object>> predicate = null)
            where T : class
        {
            if (predicate.xIsNotNull()) return QuerySelect(predicate);
            return QuerySelectAll<T>();
        }

        public JSqlExpression<Ta, Tb> Join<Ta, Tb>(Expression<Func<Ta, Tb, object>> predicate)
            where Ta : class where Tb : class
        {
            var taAlias = predicate.Parameters.First().Name;
            var tbAlias = predicate.Parameters.Last().Name;

            dynamic body = predicate.Body;
            var memberlist = new List<string>();
            foreach (var arg in body.Arguments) memberlist.Add($"{arg.Expression.Name}.{arg.Member.Name}");

            var select = $"SELECT {string.Join(", ", memberlist)}";

            var from = $"FROM {typeof(Ta).Name} {taAlias} JOIN {typeof(Tb).Name} {tbAlias}";
            return new JSqlExpression<Ta, Tb>(select, from);
        }

        #region [util]

        private JSqlExpression<T> QuerySelect<T>(Expression<Func<T, object>> predicate) where T : class
        {
            var body = predicate.Body as NewExpression;
            var select = $"SELECT {string.Join(", ", body.Members.Select(m => m.Name))}";
            var from = $"FROM {typeof(T).Name}";
            return new JSqlExpression<T>(select, from);
        }

        private JSqlExpression<T> QuerySelectAll<T>() where T : class
        {
            var publics = typeof(T).GetProperties();
            var columns = new List<string>();
            publics.xForEach(column => { columns.Add(column.Name); });

            var select = $"SELECT {string.Join(", ", columns)}";
            var from = $"FROM {typeof(T).Name}";
            return new JSqlExpression<T>(select, from);
        }

        #endregion
    }

    /// <summary>
    ///     Single 쿼리 Expression
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JSqlExpression<T> where T : class
    {
        private string _from = string.Empty;
        private List<Expression> _list = new();
        private string _on = string.Empty;
        private readonly int _parameterCnt = 1;

        private readonly Dictionary<string, SqlParameter> _parameters = new();
        private readonly string _select = string.Empty;
        private readonly List<string> _where = new();


        public JSqlExpression(string select, string from)
        {
            _select = select;
            _from = from;
        }

        public JSqlExpression(string select, string from, string on)
        {
            _select = select;
            _from = from;
            _on = on;
        }

        public JSqlExpression<T> Where(Expression<Func<T, bool>> predicate)
        {
            dynamic body = predicate.Body;

            _parameters.Add($"@p{_parameterCnt}",
                new SqlParameter
                    {ParameterName = $"@p{_parameterCnt}", Value = SqlExpressionUtil.GetDbValue(body.Right.Value)});
            var sql =
                $"WHERE {body.Left.Member.Name} {SqlExpressionUtil.xExpressionToString(body.NodeType)} @p{_parameterCnt}";
            _where.Add(sql);
            return this;
        }

        public JSqlExpression<T> Join<Ta, Tb>(Expression<Func<Ta, Tb, object>> predicate)
            where Ta : class where Tb : class
        {
            _from = _from + $"FROM {typeof(Ta).Name} A JOIN {typeof(Ta).Name} B ON";
            return this;
        }

        public JSqlExpression<T> And(Expression<Func<T, bool>> predicate)
        {
            dynamic body = predicate.Body;

            var sql = new StringBuilder();
            sql.Append("AND ");

            if (body.NodeType == ExpressionType.Call)
            {
                var args = body.Arguments as ReadOnlyCollection<Expression>;
                var commandNodeType = args.Last().NodeType;
                if (commandNodeType == ExpressionType.Constant)
                    sql.Append(SqlExpressionUtil.SqlLike(args));
                else if (commandNodeType == ExpressionType.NewArrayInit) sql.Append(SqlExpressionUtil.SqlIn(args));
            }
            else
            {
                sql.Append(
                    $"{body.Left.Member.Name} {SqlExpressionUtil.xExpressionToString(body.NodeType)} {SqlExpressionUtil.GetDbValue(body.Right.Value)}");
            }

            _where.Add(sql.ToString());
            return this;
        }


        public JSqlExpression<T> Or(Expression<Func<T, bool>> predicate)
        {
            dynamic body = predicate.Body;

            var sql = new StringBuilder();
            sql.Append("OR ");

            if (body.NodeType == ExpressionType.Call)
            {
                var args = body.Arguments as ReadOnlyCollection<Expression>;
                var commandNodeType = args.Last().NodeType;
                if (commandNodeType == ExpressionType.Constant)
                    sql.Append(SqlExpressionUtil.SqlLike(args));
                else if (commandNodeType == ExpressionType.NewArrayInit) sql.Append(SqlExpressionUtil.SqlIn(args));
            }
            else
            {
                sql.Append(
                    $"{body.Left.Member.Name} {SqlExpressionUtil.xExpressionToString(body.NodeType)} {SqlExpressionUtil.GetDbValue(body.Right.Value)}");
            }

            _where.Add(sql.ToString());
            return this;
        }

        public string Build()
        {
            var sb = new StringBuilder(10);
            sb.AppendLine(_select);
            sb.AppendLine(_from);
            _where.xForEach(where => { sb.AppendLine(where); });
            return sb.ToString();
        }

        public IEnumerable<T> Query(ENUM_DATABASE_TYPE type)
        {
            IEnumerable<T> result = null;
            JDatabaseResolver.Resolve(type).DbExecute((c, t) =>
            {
                result = c.Query<T>(Build(), _parameters.Select(m => m.Value));
            });
            return result;
        }
    }

    /// <summary>
    ///     Join 쿼리 Expression
    /// </summary>
    /// <typeparam name="Ta"></typeparam>
    /// <typeparam name="Tb"></typeparam>
    public class JSqlExpression<Ta, Tb> where Ta : class where Tb : class
    {
        private readonly string _from;
        private readonly List<string> _onAnds = new();
        private readonly string _select;
        private readonly List<string> _wheres = new();

        public JSqlExpression(string select, string from)
        {
            _select = select;
            _from = from;
        }

        public JSqlExpression<Ta, Tb> On(Expression<Func<Ta, Tb, object>> expression)
        {
            var xsb = new XStringBuilder();
            xsb.Append("ON ");
            dynamic body = expression.Body;
            xsb.Append($"{body.Operand.Left.Expression.Name}.{body.Operand.Left.Member.Name}");
            xsb.Append($"{SqlExpressionUtil.xExpressionToString(body.Operand.NodeType)}");
            xsb.Append($"{body.Operand.Right.Expression.Name}.{body.Operand.Right.Member.Name}");
            var sql = string.Empty;
            xsb.Release(out sql);
            _onAnds.Add(sql);
            return this;
        }

        public JSqlExpression<Ta, Tb> OnAnd(Expression<Func<Ta, Tb, object>> expression)
        {
            var xsb = new XStringBuilder();
            xsb.Append("AND ");
            dynamic body = expression.Body;
            xsb.Append($"{body.Operand.Left.Expression.Name}.{body.Operand.Left.Member.Name}");
            xsb.Append($"{SqlExpressionUtil.xExpressionToString(body.Operand.NodeType)}");
            xsb.Append($"{body.Operand.Right.Expression.Name}.{body.Operand.Right.Member.Name}");
            var sql = string.Empty;
            xsb.Release(out sql);
            _onAnds.Add(sql);
            return this;
        }

        public JSqlExpression<Ta, Tb> Where(Expression<Func<Ta, Tb, object>> expression)
        {
            var xsb = new XStringBuilder();
            xsb.Append("WHERE ");
            dynamic body = expression.Body;
            if (body.NodeType == ExpressionType.Convert)
            {
                if (body.Operand.NodeType == ExpressionType.Equal)
                {
                    xsb.Append($"{body.Operand.Left.Expression.Name}.{body.Operand.Left.Member.Name}");
                    xsb.Append($"{SqlExpressionUtil.xExpressionToString(body.Operand.NodeType)}");
                    xsb.Append($"{body.Operand.Right.Value}");
                    var sql = string.Empty;
                    xsb.Release(out sql);
                    _onAnds.Add(sql);
                }
                else
                {
                    var args = body.Operand.Arguments as ReadOnlyCollection<Expression>;
                    var commandNodeType = args.Last().NodeType;
                    if (commandNodeType == ExpressionType.Constant)
                        xsb.Append(SqlExpressionUtil.SqlLike(args));
                    else if (commandNodeType == ExpressionType.NewArrayInit) xsb.Append(SqlExpressionUtil.SqlIn(args));
                    var sql = string.Empty;
                    xsb.Release(out sql);
                    _onAnds.Add(sql);
                }
            }

            return this;
        }

        public JSqlExpression<Ta, Tb> And(Expression<Func<Ta, Tb, object>> expression)
        {
            var xsb = new XStringBuilder();
            xsb.Append("AND ");
            dynamic body = expression.Body;
            if (body.NodeType == ExpressionType.Convert)
            {
                if (body.Operand.NodeType == ExpressionType.Equal)
                {
                    xsb.Append($"{body.Operand.Left.Expression.Name}.{body.Operand.Left.Member.Name}");
                    xsb.Append($"{SqlExpressionUtil.xExpressionToString(body.Operand.NodeType)}");
                    xsb.Append($"{body.Operand.Right.Value}");
                    var sql = string.Empty;
                    xsb.Release(out sql);
                    _onAnds.Add(sql);
                }
                else
                {
                    var args = body.Operand.Arguments as ReadOnlyCollection<Expression>;
                    var commandNodeType = args.Last().NodeType;
                    if (commandNodeType == ExpressionType.Constant)
                        xsb.Append(SqlExpressionUtil.SqlLike(args));
                    else if (commandNodeType == ExpressionType.NewArrayInit) xsb.Append(SqlExpressionUtil.SqlIn(args));
                    var sql = string.Empty;
                    xsb.Release(out sql);
                    _onAnds.Add(sql);
                }
            }

            return this;
        }

        public string Build()
        {
            var xsb = new XStringBuilder();
            xsb.AppendLine(_select);
            xsb.AppendLine(_from);
            _onAnds.xForEach(_onAnd => { xsb.AppendLine(_onAnd); });
            _wheres.xForEach(_where => { xsb.AppendLine(_where); });

            var sql = string.Empty;
            xsb.Release(out sql);
            return sql;
        }
    }

    public class Sql
    {
        public static bool In(object o, params object[] dest)
        {
            return true;
        }

        public static bool Like(object o, object obj)
        {
            return true;
        }
    }

    internal class SqlExpressionUtil
    {
        public static string SqlLike(dynamic args)
        {
            var sql = string.Empty;
            foreach (var arg in args)
            {
                var dyarg = arg;
                if (arg.NodeType == ExpressionType.MemberAccess)
                {
                    var name = dyarg.Member.Name;
                    sql = sql + $"{name} LIKE (";
                }
                else if (arg.NodeType == ExpressionType.Constant)
                {
                    sql = sql + $"{GetDbValue(dyarg.Value)})";
                }
            }

            return sql;
        }

        public static string SqlIn(dynamic args)
        {
            var sql = string.Empty;
            foreach (var arg in args)
            {
                var dyarg = arg;
                if (arg.NodeType == ExpressionType.Convert)
                {
                    sql = sql + $"{dyarg.Operand.Member.Name} IN (";
                }
                else if (arg.NodeType == ExpressionType.NewArrayInit)
                {
                    var inlist = new List<object>();
                    foreach (var expression in dyarg.Expressions)
                    foreach (var valueExpression in expression.Expressions)
                        inlist.Add(GetDbValue(valueExpression.Value));

                    sql = sql + $"{string.Join(", ", inlist)})";
                }
            }

            return sql;
        }

        public static string xExpressionToString(ExpressionType eptype)
        {
            switch (eptype)
            {
                case ExpressionType.Equal: return "=";
                case ExpressionType.NotEqual: return "<>";
            }

            return string.Empty;
        }

        public static object GetDbValue(object o)
        {
            if (o.GetType() == typeof(string)) return $"'{o}'";

            return o;
        }
    }
}