using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.SapOrm
{
    public static class SqlGenerator
    {
        public static string GetInsertSql(string tableName, List<string> propertyName)
        {
            var values = new StringBuilder();
            var propertys = new StringBuilder();
            foreach (var item in propertyName)
            {
                propertys.Append($"\"{item}\",");
                values.Append($"?{item}?,");
            }
            return $"INSERT INTO {tableName} ({propertys.ToString().TrimEnd(',')}) VALUES ({values.ToString().TrimEnd(',')})";
        }

        public static string GetSelectSql(string tableName, List<FieldPredicate> predicates, ISort sort, int? page, int? pageSize)
        {
            var sql = new StringBuilder();
            sql.Append($"SELECT * FROM {tableName}");
            sql.Append(predicates.GetSqlWhere());
            if (sort != null)
                sql.Append($" ORDER BY {sort.PropertyName} " + (sort.Ascending ? "ASC" : "DESC"));
            else
                sql.Append($" ORDER BY \"Id\" DESC");
            if (page.HasValue && pageSize.HasValue)
                sql.Append($" LIMIT {pageSize.Value} OFFSET {(page.Value - 1) * pageSize.Value}");
            return sql.ToString();
        }

        public static string GetUpdateSql(string tableName, List<string> propertyName)
        {
            var sets = new StringBuilder();
            foreach (var item in propertyName)
            {
                if (!item.ToLower().Equals("id"))
                    sets.Append($"\"{item}\" = ?{item}?,");
            }
            return $"UPDATE {tableName} SET {sets.ToString().TrimEnd(',')} WHERE \"Id\" = ?Id?";
        }
        public static string GetCountSql(string tableName, List<FieldPredicate> predicates)
        {
            var sql = new StringBuilder();
            sql.Append($"SELECT Count(*) FROM {tableName}");
            sql.Append(predicates.GetSqlWhere());
            return sql.ToString();
        }

        /// <summary>
        /// 根据where对象 生成where sql
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetSqlWhere(this List<FieldPredicate> list)
        {
            var sql = new StringBuilder();
            sql.Append(" WHERE \"IsDelete\"=0");
            if (list != null && list.Count > 0)
            {
                list.ForEach(s =>
                {
                    sql.Append($" AND \"{s.PropertyName}\" {ComparePredicateHelper.GetOperatorString(s.Operator, s.Not)} ?{s.PropertyName}?");
                });
            }
            return sql.ToString();
        }
    }
}
