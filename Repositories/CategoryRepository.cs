using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Models.Models;

namespace Repositories
{
    public class CategoryRepository : IFormRepositories<Category>

    {
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await CategoryDAO.Instance.GetAllAsync(); // Correct: await the async method
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await CategoryDAO.Instance.GetByIdAsync(id); // Correct: await the async method
        }

        public async Task<Category> AddAsync(Category entity)
        {
            return await CategoryDAO.Instance.AddAsync(entity); // Call the base AddAsync
        }

        public async Task<Category> UpdateAsync(Category entity)
        {
            return await CategoryDAO.Instance.UpdateAsync(entity); // Correct: await and return
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await CategoryDAO.Instance.DeleteAsync(id); // Correct: await and return
        }
    }
}
