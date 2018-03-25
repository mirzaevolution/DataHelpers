using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataHelpers
{
    public interface IRepository<T>
    {
        DataResult<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter = null);
        DataResult<IEnumerator<T>> GetAllByPaging(int pageIndex, int pageSize, Expression<Func<T, bool>> filter = null);
        DataResult<T> Get(Expression<Func<T, bool>> filter);
        DataResult<T> Insert(params T[] items);
        StatusResult Update(T item);
        DataResult<T> Delete(params T[] items);
        DataResult<T> ExecuteSqlCommand(string query, params object[] sqlParameters);
    }
}
