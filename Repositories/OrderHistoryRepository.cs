using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Models.Models;

namespace Repositories
{
    public class OrderHistoryRepository : IFormRepositories<OrderHistory>
    {
        public async Task<IEnumerable<OrderHistory>> GetAllAsync()
        {
            return await OrderHistoryDAO.Instance.GetAllAsync(); // Correct: await the async method
        }

        public async Task<OrderHistory> GetByIdAsync(int id)
        {
            return await OrderHistoryDAO.Instance.GetByIdAsync(id); // Correct: await the async method
        }

        public async Task<OrderHistory> AddAsync(OrderHistory entity)
        {
            return await OrderHistoryDAO.Instance.AddAsync(entity); // Call the base AddAsync
        }

        public async Task<OrderHistory> UpdateAsync(OrderHistory entity)
        {
            return await OrderHistoryDAO.Instance.UpdateAsync(entity); // Correct: await and return
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await OrderHistoryDAO.Instance.DeleteAsync(id); // Correct: await and return
        }

        public async Task<IEnumerable<OrderHistory>> GetFilteredHistoryAsync(string actionType, DateOnly? startDate, DateOnly? endDate)
        {
            return await OrderHistoryDAO.Instance.GetFilteredHistoryAsync(actionType, startDate, endDate);
        }
    }
}
