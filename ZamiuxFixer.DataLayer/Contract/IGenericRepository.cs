using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZamiuxFixer.DataLayer.Contract
{
    public interface IGenericRepository<T> where T : class
    {
        public IEnumerable<T> GetAll();
        public T GetValue(long id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Save();
        
    }
}
