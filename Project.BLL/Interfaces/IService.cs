using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.BLL.Interfaces
{
    public interface IService<T> where T : class
    {
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        T Get(int id);
        Task<T> CreateAsync(T entityDTO);
        Task UpdateAsync(T entityDTO);
        Task DeleteAsync(T entityDTO);
    }
}
