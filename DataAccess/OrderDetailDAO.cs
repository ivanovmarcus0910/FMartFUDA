using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace DataAccess
{
    public class OrderDetailDAO : SingletonBase<OrderDetailDAO>
    {
        public async Task<OrderDetail> AddAsync(OrderDetail entity)
        {
            try
            {
                var entityEntry = await _context.Set<OrderDetail>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return entityEntry.Entity;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error in AddAsync: {ex}");
                throw;
            }
        }

        public async Task<OrderDetail> UpdateAsync(OrderDetail entity)
        {
            try
            {
                _context.Set<OrderDetail>().Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine($"Concurrency error in UpdateAsync: {ex}");
                throw;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error in UpdateAsync: {ex}");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int orderDetailId, int orderId)
        {
            try
            {
                var entity = await _context.Set<OrderDetail>()
                    .FirstOrDefaultAsync(o => o.OrderDetailId == orderDetailId && o.OrderId == orderId);

                if (entity != null)
                {
                    _context.Set<OrderDetail>().Remove(entity);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteAsync: {ex}");
                throw;
            }
        }

        public async Task<IEnumerable<OrderDetail>> GetAllAsync()
        {
            return await _context.Set<OrderDetail>().ToListAsync();
        }

        public async Task<IEnumerable<OrderDetail>> GetAllByOrderIdAsync(int orderId)
        {
            return await _context.Set<OrderDetail>()
                .Include(o => o.Product)
                .Where(o => o.OrderId == orderId)
                .Include(o => o.Order)
                .ToListAsync();
        }

        public async Task<OrderDetail> GetByIdAsync(int orderDetailId, int orderId)
        {
            return await _context.Set<OrderDetail>()
                .FirstOrDefaultAsync(o => o.OrderDetailId == orderDetailId && o.OrderId == orderId);
        }

        public async Task<int> GetMaxOrderDetailIdAsync(int orderId)
        {
            return await _context.OrderDetails.Where(od => od.OrderId == orderId).MaxAsync(od => (int?)od.OrderDetailId) ?? 0;
        }

        public async Task<IEnumerable<OrderDetail>> FindByConditionAsync(Expression<Func<OrderDetail, bool>> predicate)
        {
            return await _context.Set<OrderDetail>().Where(predicate).ToListAsync();
        }
    }

}
