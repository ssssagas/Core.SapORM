using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core.SapOrm.Attributes;

namespace Core.SapOrm
{
    public class ZeusCoreImplementor
    {
        public async Task<int> InsertAsync<T>(IDbConnection connection, T entity, IDbTransaction transaction = null, int? commandTimeout = default(int?)) where T : class
        {
            var sql = string.Empty;
            if (entity != null)
                sql = SqlGenerator.GetInsertSql(GetTableName(entity), GetPropertyList(entity));
            else
                throw new ArgumentException("Insert method entity can not be null");
            return await connection.ExecuteAsync(sql, entity, transaction, commandTimeout, CommandType.Text);
        }
        public async Task<int> InsertListAsync<T>(IDbConnection connection, List<T> list, IDbTransaction transaction = null, int? commandTimeout = default(int?)) where T : class
        {
            var sql = string.Empty;
            if (list != null && list.Count > 0)
                sql = SqlGenerator.GetInsertSql(GetTableName(list.FirstOrDefault()), GetPropertyList(list.FirstOrDefault()));
            else
                throw new ArgumentException("Insert method entity can not be null");
            return await connection.ExecuteAsync(sql, list, transaction, commandTimeout, CommandType.Text);
        }
        public async Task<T> GetByIdAsync<T>(IDbConnection connection, long id, ISort sort, IDbTransaction transaction = null, int? commandTimeout = default(int?)) where T : class, new()
        {
            var entity = new T();
            var list = new List<FieldPredicate> { new FieldPredicate { Operator = Operator.Eq, PropertyName = "Id", Value = id } };
            var sql = SqlGenerator.GetSelectSql(GetTableName(entity), list, sort, null, null);
            var entities = await connection.QueryAsync<T>(sql, list.GetFieldPredicateValue(), transaction: transaction, commandTimeout: commandTimeout, commandType: CommandType.Text);
            return entities.FirstOrDefault();
        }
        public async Task<IEnumerable<T>> GetListAsync<T>(IDbConnection connection, List<FieldPredicate> list, ISort sort, IDbTransaction transaction = null, int? commandTimeout = default(int?)) where T : class, new()
        {
            var entity = new T();
            var sql = SqlGenerator.GetSelectSql(GetTableName(entity), list, sort, null, null);
            return await connection.QueryAsync<T>(sql, list.GetFieldPredicateValue(), transaction: transaction, commandTimeout: commandTimeout, commandType: CommandType.Text);
        }
        public async Task<IEnumerable<T>> GetPageListAsync<T>(IDbConnection connection, List<FieldPredicate> list, int page, int pageSize, ISort sort, IDbTransaction transaction = null, int? commandTimeout = default(int?)) where T : class, new()
        {
            var entity = new T();
            var sql = SqlGenerator.GetSelectSql(GetTableName(entity), list, sort, page, pageSize);
            return await connection.QueryAsync<T>(sql, list.GetFieldPredicateValue(), transaction: transaction, commandTimeout: commandTimeout, commandType: CommandType.Text);
        }

        public async Task<int> UpdateAsync<T>(IDbConnection connection, T entity, IDbTransaction transaction = null, int? commandTimeout = default(int?)) where T : class
        {
            var sql = string.Empty;
            var propertyList = GetPropertyList(entity);
            if (propertyList.Where(s => s.ToLower().Equals("id")).Count() == 0)
                throw new ArgumentException("Update method must have id");
            if (entity != null)
                sql = SqlGenerator.GetUpdateSql(GetTableName(entity), propertyList);
            else
                throw new ArgumentException("Insert method entity can not be null");
            return await connection.ExecuteAsync(sql, entity, transaction, commandTimeout, CommandType.Text);
        }
        public async Task<int> DeleteAsync<T>(IDbConnection connection, long id, IDbTransaction transaction = null, int? commandTimeout = default(int?)) where T : class, new()
        {
            var entity = new T();
            var sql = SqlGenerator.GetUpdateSql(GetTableName(entity), new List<string> { "IsDelete" });
            return await connection.ExecuteAsync(sql, new { Id = id, IsDelete = true }, transaction, commandTimeout, CommandType.Text);
        }
        public async Task<int> GetCountAsync<T>(IDbConnection connection, List<FieldPredicate> list, IDbTransaction transaction = null, int? commandTimeout = default(int?)) where T : class, new()
        {
            var entity = new T();
            var sql = SqlGenerator.GetCountSql(GetTableName(entity), list);
            var res = await connection.QueryAsync<int>(sql, list.GetFieldPredicateValue(), transaction: transaction, commandTimeout: commandTimeout, commandType: CommandType.Text);
            return res.FirstOrDefault();
        }


        #region private
        /// <summary>
        /// 获取泛型表名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        private string GetTableName<T>(T t)
        {
            var tableName = t.GetTableName();
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException("TableName can not be null");
            return tableName;
        }
        /// <summary>
        /// 获取属性Name集合
        /// </summary>
        /// <returns></returns>
        private List<string> GetPropertyList<T>(T t)
        {
            var properties = t.GetType().GetProperties();
            if (properties == null || properties.Count() == 0)
                throw new ArgumentException("Entity property can not be null");
            var propertyList = new List<string>();
            foreach (var propertyInfo in properties)
            {
                propertyList.Add(propertyInfo.Name);
            }
            return propertyList;
        }
        #endregion
    }
}
