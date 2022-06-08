using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.DAL.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        T Get(int id);
        void Create(T entity);
        void Update(int id, T entity);
        void Delete(T entity);
        void Save();
    }
}
