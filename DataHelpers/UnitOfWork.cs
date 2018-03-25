using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Entity;
namespace DataHelpers
{
    public class UnitOfWork:IDisposable
    {
        private Dictionary<Type, object> _dict = new Dictionary<Type, object>();
        private DbContext _coreContext;
        private bool _disposed;
        public UnitOfWork(DbContext dbContext)
        {
            _coreContext = dbContext;
        }   
        public IRepository<T> Repository<T>()where T:class
        {
            if (_dict.ContainsKey(typeof(T)))
                return _dict[typeof(T)] as IRepository<T>;
            IRepository<T> repository = new GenericRepository<T>(_coreContext);
            _dict[typeof(T)] = repository;
            return repository;
        }
        public StatusResult Save()
        {
            bool success = true;
            List<string> errors = new List<string>();
            try
            {
                _coreContext.SaveChanges();
            }
            catch(Exception ex)
            {
                success = false;
                errors.Add(ex.Message);
            }
            return new StatusResult(success, errors);
        }
        public void Dispose()
        {
            Disposing(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Disposing(bool dispose)
        {
            if (dispose && !_disposed)
            {
                _coreContext.Dispose();
                _disposed = true;
            }
        }
    }
}
