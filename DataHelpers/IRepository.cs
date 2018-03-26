using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataHelpers
{
    public interface IRepository<T>
    {
        DataResult<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter = null);
        DataResult<IEnumerable<T>> GetAllByPaging(int pageIndex, int pageSize, Expression<Func<T, bool>> filter = null);
        DataResult<T> Get(Expression<Func<T, bool>> filter);
        DataResult<int> Insert(params T[] items);
        StatusResult Update(T item);
        DataResult<int> Delete(params T[] items);
        DataResult<int> Delete(Expression<Func<T,bool>> filter);
        DataResult<bool> Exists(Expression<Func<T, bool>> filter);
        DataResult<int> ExecuteSqlCommand(string query, params object[] sqlParameters);
    }
}
