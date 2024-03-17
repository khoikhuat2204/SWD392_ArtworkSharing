using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.BaseRepository
{
    public interface IBaseRepository<T>
    {
        IQueryable<T> GetAll();
        bool Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
