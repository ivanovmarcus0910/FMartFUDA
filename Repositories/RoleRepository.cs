using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Models.Models;

namespace Repositories
{
    public class RoleRepository: IFormRepositories<Role>
    {
        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await RoleDAO.Instance.GetAllAsync(); // Correct: await the async method
        }

        public async Task<Role> GetByIdAsync(int id)
        {
            return await RoleDAO.Instance.GetByIdAsync(id); // Correct: await the async method
        }

        public async Task<Role> AddAsync(Role entity)
        {
            return await RoleDAO.Instance.AddAsync(entity); // Call the base AddAsync
        }

        public async Task<Role> UpdateAsync(Role entity)
        {
            return await RoleDAO.Instance.UpdateAsync(entity); // Correct: await and return
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await RoleDAO.Instance.DeleteAsync(id); // Correct: await and return
        }
    }
}
