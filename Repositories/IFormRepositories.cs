using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IFormRepositories<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);      // Return added entity
        Task<T> UpdateAsync(T entity);   // Return updated entity
        Task<bool> DeleteAsync(int id); // Return true if deleted, false otherwise
    }
}
