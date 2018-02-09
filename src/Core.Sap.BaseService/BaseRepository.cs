using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Core.SapOrm;
using Dapper;

namespace Core.Sap.ProjectBase
{
    public class BaseRepository : IBaseRepository
    {
        private readonly string _connectionString;
        /// <summary>
        /// 基础连接字符串
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
        protected BaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        /// <summary>
        /// 基础DAPPER方法
        /// </summary>
        /// <returns>The connection async.</returns>
        /// <param name="getData">Get data.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected async Task<T> WithConnectionAsync<T>(Func<IDbConnection, Task<T>> getData)
        {
            try
            {
                using (var connection = new OdbcConnection(_connectionString))
                {
                    try
                    {
                        await connection.OpenAsync();
                        return await getData(connection);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        throw;
                    }
                }
            }
            catch (TimeoutException ex)
            {
                throw new Exception($"{GetType().FullName}.WithConnection() experienced a SQL timeout", ex);
            }
            catch (SqlException ex)
            {
                throw new Exception($"{GetType().FullName}.WithConnection() experienced a SQL exception", ex);
            }
        }


        /// <summary>
        /// 执行事务基础方法
        /// </summary>
        /// <param name="dic">KEY为执行SQL，VALUE为执行参数</param>
        /// <returns></returns>
        protected async Task<bool> ExecuteTransactionAsync(Dictionary<string, object> dic)
        {
            return await WithConnectionAsync(async c =>
            {
                var trans = c.BeginTransaction();
                try
                {
                    foreach (var item in dic)
                    {
                        await c.ExecuteAsync(item.Key, param: item.Value, transaction: trans);
                    }
                    trans.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            });
        }
        /// <summary>
        /// 基础执行SQL方法
        /// </summary>
        /// <returns>The by sql async.</returns>
        /// <param name="sql">Sql.</param>
        /// <param name="p">P.</param>
        protected async Task<int> ExecuteBySqlAsync(string sql, object p = null)
        {
            return await WithConnectionAsync(async c =>
            {
                return await c.ExecuteAsync(sql, param: p);
            });
        }
        /// <summary>
        /// 基础查询SQL方法
        /// </summary>
        /// <returns>The by sql async.</returns>
        /// <param name="sql">Sql.</param>
        /// <param name="p">P.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected async Task<IEnumerable<T>> QueryBySqlAsync<T>(string sql, object p = null)
        {
            return await WithConnectionAsync(async c =>
            {
                return await c.QueryAsync<T>(sql, param: p);
            });
        }
        /// <summary>
        /// 根据ID获取单个对象
        /// </summary>
        /// <returns>The by identifier async.</returns>
        /// <param name="id">Identifier.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<T> GetByIdAsync<T>(long id) where T : class, new()
        {
            return await WithConnectionAsync(async c => { return await c.GetByIdAsync<T>(id); });
        }
        /// <summary>
        /// 获取集合
        /// </summary>
        /// <returns>The list async.</returns>
        /// <param name="predicates">Predicates.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<IEnumerable<T>> GetListAsync<T>(List<FieldPredicate> predicates = null) where T : class, new()
        {
            return await WithConnectionAsync(async c => { return await c.GetListAsync<T>(predicates); });
        }
        /// <summary>
        /// 获取分页集合
        /// </summary>
        /// <returns>The page list async.</returns>
        /// <param name="page">Page.</param>
        /// <param name="pageSize">Page size.</param>
        /// <param name="predicates">Predicates.</param>
        /// <param name="sort">Sort.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<IEnumerable<T>> GetPageListAsync<T>(int page, int pageSize, List<FieldPredicate> predicates = null, ISort sort = null) where T : class, new()
        {
            return await WithConnectionAsync(async c => { return await c.GetPageListAsync<T>(predicates, page, pageSize, sort: sort); });
        }
        /// <summary>
        /// 写入单个对象
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="entity">Entity.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<int> InsertAsync<T>(T entity) where T : class, new()
        {
            return await WithConnectionAsync(async c => { return await c.InsertAsync(entity); });
        }
        /// <summary>
        /// 更新单个对象
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="entity">Entity.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<int> UpdateAsync<T>(T entity) where T : class, new()
        {
            return await WithConnectionAsync(async c => { return await c.UpdateAsync(entity); });
        }
        /// <summary>
        /// 删除对象（假删除（表需存在Is_Delete字段））
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="id">Identifier.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<int> DeleteAsync<T>(long id) where T : class, new()
        {
            return await WithConnectionAsync(async c => { return await c.DeleteAsync<T>(id); });
        }
        /// <summary>
        /// 获取所有合集
        /// </summary>
        /// <returns>The count async.</returns>
        /// <param name="predicates">Predicates.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<int> GetCountAsync<T>(List<FieldPredicate> predicates = null) where T : class, new()
        {
            return await WithConnectionAsync(async c => { return await c.GetCountAsync<T>(predicates); });
        }
    }
}
