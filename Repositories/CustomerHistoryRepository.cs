using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Models.Models;

namespace Repositories
{
    public class CustomerHistoryRepository : IFormRepositories<CustomerHistory>
    {
        public async Task<IEnumerable<CustomerHistory>> GetAllAsync()
        {
            return await CustomerHistoryDAO.Instance.GetAllAsync(); // Correct: await the async method
        }

        public async Task<CustomerHistory> GetByIdAsync(int id)
        {
            return await CustomerHistoryDAO.Instance.GetByIdAsync(id); // Correct: await the async method
        }

        public async Task<CustomerHistory> AddAsync(CustomerHistory entity)
        {
            return await CustomerHistoryDAO.Instance.AddAsync(entity); // Call the base AddAsync
        }

        public async Task<CustomerHistory> UpdateAsync(CustomerHistory entity)
        {
            return await CustomerHistoryDAO.Instance.UpdateAsync(entity); // Correct: await and return
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await CustomerHistoryDAO.Instance.DeleteAsync(id); // Correct: await and return
        }
    }
}
