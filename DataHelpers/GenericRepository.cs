using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
namespace DataHelpers
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private DbContext _ctx;
        private DbSet<T> _set;
        public GenericRepository(DbContext dbContext)
        {
            _ctx = dbContext;
            _set = _ctx.Set<T>();
        }

        public DataResult<T> Get(Expression<Func<T, bool>> filter)
        {
            bool success = true;
            List<string> errors = new List<string>();
            T entity = default(T);
            try
            {
                entity = _set.FirstOrDefault(filter);
            }
            catch(Exception ex)
            {
                success = false;
                errors.Add(ex.Message);
            }
            return new DataResult<T>(entity, new StatusResult(success, errors));
        }

        public DataResult<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter = null)
        {
            bool success = true;
            List<string> errors = new List<string>();
            IEnumerable<T> result = null;
            try
            {
                if(filter!=null)
                {
                    result = _set.Where(filter);
                }
                else
                {
                    result = _set;
                }
            }
            catch (Exception ex)
            {
                success = false;
                errors.Add(ex.Message);
            }
            return new DataResult<IEnumerable<T>>(result, new StatusResult(success, errors));
        }
        
        public DataResult<IEnumerator<T>> GetAllByPaging(int pageIndex, int pageSize, Expression<Func<T, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public DataResult<int> Insert(params T[] items)
        {
            bool success = true;
            List<string> errors = new List<string>();
            int count = 0;
            try
            {
                count = items.Count();
                if (items != null && count>0)
                {
                    _set.AddRange(items);
                }
                else
                {
                    success = false;
                    errors.Add("Item cannot be null or empty");
                }
            }
            catch (Exception ex)
            {
                success = false;
                errors.Add(ex.Message);
            }
            return new DataResult<int>(count,new StatusResult(success, errors));

        }

        public StatusResult Update(T item)
        {
            bool success = true;
            List<string> errors = new List<string>();
            try
            {
                if (item != null)
                {
                    if (!_set.Local.Contains(item))
                        _set.Attach(item);
                    _ctx.Entry(item).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    success = false;
                    errors.Add("Item cannot be null");
                }
            }
            catch (Exception ex)
            {
                success = false;
                errors.Add(ex.Message);
            }
            return new StatusResult(success, errors);

        }
        public DataResult<int> Delete(params T[] items)
        {
            bool success = true;
            List<string> errors = new List<string>();
            int count = 0;
            try
            {
                count = items.Count();
                if (items != null && count>0)
                {
                    _set.RemoveRange(items);
                }
                else
                {
                    success = false;
                    errors.Add("Item cannot be null/empty");
                }
            }
            catch (Exception ex)
            {
                success = false;
                errors.Add(ex.Message);
            }
            return new DataResult<int>(count,new StatusResult(success,errors));

        }
        public DataResult<int> ExecuteSqlCommand(string query, params object[] sqlParameters)
        {
            bool success = true;
            List<string> errors = new List<string>();
            int result = 0;
            try
            {
                if(string.IsNullOrEmpty(query))
                {
                    success = false;
                    errors.Add("Query cannot be empty");
                }
                else
                {
                    if(sqlParameters!=null && sqlParameters.Count() > 0)
                    {
                        result = _ctx.Database.ExecuteSqlCommand(query,sqlParameters);
                    }
                    else
                    {
                        result = _ctx.Database.ExecuteSqlCommand(query);
                    }
                }
            }
            catch(Exception ex)
            {
                success = false;
                errors.Add(ex.Message);
            }
            return new DataResult<int>(result, new StatusResult(success, errors));
        }
    }
}
