using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Models.Models;

namespace Repositories
{
    public class OrderDetailRepository : IFormRepositories<OrderDetail>
    {
        public async Task<IEnumerable<OrderDetail>> GetAllAsync()
        {
            return await OrderDetailDAO.Instance.GetAllAsync(); // Correct: await the async method
        }

        public async Task<IEnumerable<OrderDetail>> GetAllByOrderIdAsync(int id)
        {
            return await OrderDetailDAO.Instance.GetAllByOrderIdAsync(id); // Correct: await the async method
        }

        public async Task<OrderDetail> GetByIdAsync(int id)
        {
            return await OrderDetailDAO.Instance.GetByIdAsync(id); // Correct: await the async method
        }

        public async Task<OrderDetail> AddAsync(OrderDetail entity)
        {
            return await OrderDetailDAO.Instance.AddAsync(entity); // Call the base AddAsync
        }

        public async Task<OrderDetail> UpdateAsync(OrderDetail entity)
        {
            return await OrderDetailDAO.Instance.UpdateAsync(entity); // Correct: await and return
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await OrderDetailDAO.Instance.DeleteAsync(id); // Correct: await and return
        }
    }
}
