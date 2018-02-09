using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Core.SapOrm
{
    public static class ZeusCore
    {
        private static readonly ZeusCoreImplementor _zeusCoreImplementor = new ZeusCoreImplementor();
        /// <summary>
        /// 插入单条
        /// </summary>
        public static async Task<int> InsertAsync<T>(this IDbConnection connection, T entity, IDbTransaction transaction = null, int? commandTimeout = default(int?)) where T : class
        {
            return await _zeusCoreImplementor.InsertAsync(connection, entity, transaction, commandTimeout);
        }
        /// <summary>
        /// 插入多条
        /// </summary>
        public static async Task<int> InsertListAsync<T>(this IDbConnection connection, List<T> list, IDbTransaction transaction = null, int? commandTimeout = default(int?)) where T : class
        {
            return await _zeusCoreImplementor.InsertAsync(connection, list, transaction, commandTimeout);
        }
        /// <summary>
        /// 获取全部
        /// </summary>
        public static async Task<IEnumerable<T>> GetListAsync<T>(this IDbConnection connection, List<FieldPredicate> predicate = null, ISort sort = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class, new()
        {
            return await _zeusCoreImplementor.GetListAsync<T>(connection, predicate, sort, transaction, commandTimeout);
        }
        /// <summary>
        /// 获取分页List
        /// </summary>
        public static async Task<IEnumerable<T>> GetPageListAsync<T>(this IDbConnection connection, List<FieldPredicate> predicate = null, int page = 1, int pageSize = 10, ISort sort = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class, new()
        {
            return await _zeusCoreImplementor.GetPageListAsync<T>(connection, predicate, page, pageSize, sort, transaction, commandTimeout);
        }
        /// <summary>
        /// 根据ID获取
        /// </summary>
        public static async Task<T> GetByIdAsync<T>(this IDbConnection connection, long id, IDbTransaction transaction = null, int? commandTimeout = null) where T : class, new()
        {
            return await _zeusCoreImplementor.GetByIdAsync<T>(connection, id, null, transaction, commandTimeout);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        public static async Task<int> UpdateAsync<T>(this IDbConnection connection, T entity, IDbTransaction transaction = null, int? commandTimeout = default(int?)) where T : class
        {
            return await _zeusCoreImplementor.UpdateAsync(connection, entity, transaction, commandTimeout);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="connection">Connection.</param>
        /// <param name="id">Identifier.</param>
        /// <param name="transaction">Transaction.</param>
        /// <param name="commandTimeout">Command timeout.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static async Task<int> DeleteAsync<T>(this IDbConnection connection, long id, IDbTransaction transaction = null, int? commandTimeout = default(int?)) where T : class, new()
        {
            return await _zeusCoreImplementor.DeleteAsync<T>(connection, id, transaction, commandTimeout);
        }
        /// <summary>
        /// 获取条数
        /// </summary>
        /// <returns>The count async.</returns>
        /// <param name="connection">Connection.</param>
        /// <param name="list">List.</param>
        /// <param name="transaction">Transaction.</param>
        /// <param name="commandTimeout">Command timeout.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static async Task<int> GetCountAsync<T>(this IDbConnection connection, List<FieldPredicate> list, IDbTransaction transaction = null, int? commandTimeout = default(int?)) where T : class, new()
        {
            return await _zeusCoreImplementor.GetCountAsync<T>(connection, list, transaction, commandTimeout);
        }
    }
}
