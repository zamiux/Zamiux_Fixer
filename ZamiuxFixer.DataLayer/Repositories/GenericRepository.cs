using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZamiuxFixer.DataLayer.Contract;

namespace ZamiuxFixer.DataLayer.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class 
    {
        #region Constructor
        private ZamiuxFixerDbContext _context;
        public GenericRepository(ZamiuxFixerDbContext context)
        {
                _context = context;
        }
        #endregion
        void IGenericRepository<T>.Add(T entity)
        {
            _context.Add(entity);
        }

        void IGenericRepository<T>.Delete(T entity)
        {
            _context.Remove(entity);
        }

        public T GetValue(long id)
        {
            return _context.Set<T>().Find(id); 
        }


        void IGenericRepository<T>.Save()
        {
            _context.SaveChanges();
        }

        void IGenericRepository<T>.Update(T entity)
        {
            _context.Update(entity);
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }
    }
}
