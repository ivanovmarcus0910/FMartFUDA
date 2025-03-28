using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Models.Models;

namespace Repositories
{
    public class ProductRepository : IFormRepositories<Product>

    {
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await ProductDAO.Instance.GetAllAsync(); // Correct: await the async method
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await ProductDAO.Instance.GetByIdAsync(id); // Correct: await the async method
        }

        public async Task<Product> AddAsync(Product entity)
        {
            return await ProductDAO.Instance.AddAsync(entity); // Call the base AddAsync
        }

        public async Task<Product> UpdateAsync(Product entity)
        {
            return await ProductDAO.Instance.UpdateAsync(entity); // Correct: await and return
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await ProductDAO.Instance.DeleteAsync(id); // Correct: await and return
        }
    }
}
