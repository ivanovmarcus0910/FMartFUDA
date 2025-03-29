using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace Repositories
{
    public class OrderDetailRepository : IFormRepositories<OrderDetail>
    {
        public async Task<IEnumerable<OrderDetail>> GetAllAsync()
        {
            return await OrderDetailDAO.Instance.GetAllAsync();
        }

        public async Task<IEnumerable<OrderDetail>> GetAllByOrderIdAsync(int orderId)
        {
            return await OrderDetailDAO.Instance.GetAllByOrderIdAsync(orderId);
        }

        public async Task<OrderDetail> GetByIdAsync(int id)
        {
            throw new NotImplementedException("Use GetByIdAsync(int orderDetailId, int orderId) instead.");
        }

        public async Task<OrderDetail> GetByIdAsync(int orderDetailId, int orderId)
        {
            return await OrderDetailDAO.Instance.GetByIdAsync(orderDetailId, orderId);
        }

        public async Task<OrderDetail> AddAsync(OrderDetail entity)
        {
            return await OrderDetailDAO.Instance.AddAsync(entity);
        }

        public async Task<OrderDetail> UpdateAsync(OrderDetail entity)
        {
            return await OrderDetailDAO.Instance.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException("Use DeleteAsync(int orderDetailId, int orderId) instead.");
        }

        public async Task<bool> DeleteAsync(int orderDetailId, int orderId)
        {
            return await OrderDetailDAO.Instance.DeleteAsync(orderDetailId, orderId);
        }

        public async Task<int> GetMaxOrderDetailIdAsync(int orderId)
        {
            return await OrderDetailDAO.Instance.GetMaxOrderDetailIdAsync(orderId);
        }
    }
}
