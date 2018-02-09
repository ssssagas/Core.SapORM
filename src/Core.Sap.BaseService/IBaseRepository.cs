using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.SapOrm;

namespace Core.Sap.ProjectBase
{
    public interface IBaseRepository
    {
        Task<T> GetByIdAsync<T>(long id) where T : class, new();
        Task<IEnumerable<T>> GetListAsync<T>(List<FieldPredicate> predicates = null) where T : class, new();
        Task<IEnumerable<T>> GetPageListAsync<T>(int page, int pageSize, List<FieldPredicate> predicates = null, ISort sort = null) where T : class, new();
        Task<int> InsertAsync<T>(T entity) where T : class, new();
        Task<int> UpdateAsync<T>(T entity) where T : class, new();
        Task<int> DeleteAsync<T>(long id) where T : class, new();
        Task<int> GetCountAsync<T>(List<FieldPredicate> predicates = null) where T : class, new();
    }
}
