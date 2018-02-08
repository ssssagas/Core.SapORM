using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Zeus.Helper;

namespace Zeus
{
    public static class Predicates
    {
        /// <summary>
        /// 生成where条件对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">字段名称</param>
        /// <param name="op">比较方法</param>
        /// <param name="value">值</param>
        /// <param name="not">like是否需要not</param>
        /// <returns></returns>
        public static FieldPredicate Field<T>(Expression<Func<T, object>> expression, Operator op, object value, bool not = false) where T : class
        {
            PropertyInfo propertyInfo = ReflectionHelper.GetProperty(expression) as PropertyInfo;
            return new FieldPredicate
            {
                PropertyName = propertyInfo.Name,
                Operator = op,
                Value = value,
                Not = not
            };
        }
        /// <summary>
        /// 生成排序对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="ascending"></param>
        /// <returns></returns>
        public static ISort Sort<T>(Expression<Func<T, object>> expression, bool ascending = true)
        {
            PropertyInfo propertyInfo = ReflectionHelper.GetProperty(expression) as PropertyInfo;
            return new Sort
            {
                PropertyName = propertyInfo.Name,
                Ascending = ascending
            };
        }
        /// <summary>
        /// 拼接dapper所需值
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DynamicParameters GetFieldPredicateValue(this List<FieldPredicate> list)
        {
            var parameters = new DynamicParameters();
            if (list != null && list.Count > 0)
                list.ForEach(s =>
                {
                    parameters.Add(s.PropertyName, s.Value);
                });
            return parameters;
        }
    }
    public class FieldPredicate : ComparePredicate
    {
        public object Value { get; set; }
        public string PropertyName { get; set; }
    }

    public enum Operator
    {
        /// <summary>
        /// 等于
        /// </summary>
        Eq,

        /// <summary>
        /// 大于
        /// </summary>
        Gt,

        /// <summary>
        /// 大于等于
        /// </summary>
        Ge,

        /// <summary>
        ///  小于
        /// </summary>
        Lt,

        /// <summary>
        /// 小于等于
        /// </summary>
        Le,

        /// <summary>
        /// Like
        /// </summary>
        Like
    }
    public class ComparePredicate
    {
        public Operator Operator { get; set; }
        public bool Not { get; set; }
    }
    public static class ComparePredicateHelper
    {
        public static string GetOperatorString(Operator operatorStr, bool not)
        {
            switch (operatorStr)
            {
                case Operator.Gt:
                    return ">";
                case Operator.Ge:
                    return ">=";
                case Operator.Lt:
                    return "<";
                case Operator.Le:
                    return "<=";
                case Operator.Like:
                    return not ? "NOT LIKE" : "LIKE";
                default:
                    return "=";
            }
        }
    }
    public interface ISort
    {
        string PropertyName { get; set; }
        bool Ascending { get; set; }
    }
    public class Sort : ISort
    {
        public string PropertyName { get; set; }
        public bool Ascending { get; set; }
    }
}
