using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Models.Models;

namespace Repositories
{
    public class OrderRepository : IFormRepositories<Order>
    {
        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await OrderDAO.Instance.GetAllAsync(); // Correct: await the async method
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await OrderDAO.Instance.GetByIdAsync(id); // Correct: await the async method
        }

        public async Task<Order> AddAsync(Order entity)
        {
            return await OrderDAO.Instance.AddAsync(entity); // Call the base AddAsync
        }

        public async Task<Order> UpdateAsync(Order entity)
        {
            return await OrderDAO.Instance.UpdateAsync(entity); // Correct: await and return
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await OrderDAO.Instance.DeleteAsync(id); // Correct: await and return
        }
    }
}
