using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Models.Models;

namespace Repositories
{
    public class CustomerRepository : IFormRepositories<Customer>
    {
        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await CustomerDAO.Instance.GetAllAsync(); // Correct: await the async method
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await CustomerDAO.Instance.GetByIdAsync(id); // Correct: await the async method
        }

        public async Task<Customer> AddAsync(Customer entity)
        {
            return await CustomerDAO.Instance.AddAsync(entity); // Call the base AddAsync
        }

        public async Task<Customer> UpdateAsync(Customer entity)
        {
            return await CustomerDAO.Instance.UpdateAsync(entity); // Correct: await and return
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await CustomerDAO.Instance.DeleteAsync(id); // Correct: await and return
        }
    }
}
