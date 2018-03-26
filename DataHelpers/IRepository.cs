using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataHelpers
{
    public interface IRepository<T>
    {
        DataResult<IEnumerable<T>> GetAll(
                Expression<Func<T, bool>> filter = null,
                Func<IQueryable<T>, IOrderedQueryable<T>> order = null);
        Task<DataResult<IEnumerable<T>>> GetAllAsync(
        Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> order = null);

        DataResult<IEnumerable<T>> GetAllByPaging
            (   int pageIndex, 
                int pageSize, 
                Expression<Func<T, bool>> filter = null,
                Func<IQueryable<T>, IOrderedQueryable<T>> order = null
            );
        Task<DataResult<IEnumerable<T>>> GetAllByPagingAsync
            (int pageIndex,
                int pageSize,
                Expression<Func<T, bool>> filter = null,
                Func<IQueryable<T>, IOrderedQueryable<T>> order = null
            );


        DataResult<T> Get(Expression<Func<T, bool>> filter);
        Task<DataResult<T>> GetAsync(Expression<Func<T, bool>> filter);

        DataResult<int> Insert(params T[] items);
        Task<DataResult<int>> InsertAsync(params T[] items);

        StatusResult Update(T item);
        Task<StatusResult> UpdateAsync(T item);

        DataResult<int> Delete(params T[] items);
        Task<DataResult<int>> DeleteAsync(params T[] items);
        DataResult<int> Delete(Expression<Func<T,bool>> filter);
        Task<DataResult<int>> DeleteAsync(Expression<Func<T, bool>> filter);

        DataResult<bool> Exists(Expression<Func<T, bool>> filter);
        Task<DataResult<bool>> ExistsAsync(Expression<Func<T, bool>> filter);

        DataResult<int> ExecuteSqlCommand(string query, params object[] sqlParameters);
        Task<DataResult<int>> ExecuteSqlCommandAsync(string query, params object[] sqlParameters);

        DataResult<IEnumerable<T>> SqlQuery(string query, params object[] sqlParameters);
        Task<DataResult<IEnumerable<T>>> SqlQueryAsync(string query, params object[] sqlParameters);
    }
}
