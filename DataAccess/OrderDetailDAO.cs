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
                return entityEntry.Entity; // Return the added entity
            }
            catch (DbUpdateException ex)
            {
                // Log the full exception details (using a logging framework)
                Console.WriteLine($"Error in AddAsync: {ex}");
                throw; // Re-throw after logging
            }
        }

        public async Task<OrderDetail> UpdateAsync(OrderDetail entity)
        {
            try
            {
                _context.Set<OrderDetail>().Update(entity); // More efficient than attaching and setting values
                await _context.SaveChangesAsync();
                return entity; // Return the updated entity
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine($"Concurrency error in UpdateAsync: {ex}");
                throw; // Re-throw after logging
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error in UpdateAsync: {ex}");
                throw; // Re-throw after logging
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.Set<OrderDetail>().FindAsync(id);
                if (entity != null)
                {
                    _context.Set<OrderDetail>().Remove(entity);
                    await _context.SaveChangesAsync();
                    return true; // Return true on successful deletion
                }
                return false; // Return false if entity not found
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteAsync: {ex}");
                throw; // Re-throw after logging
            }
        }

        public async Task<IEnumerable<OrderDetail>> GetAllAsync()
        {
            return await _context.Set<OrderDetail>().ToListAsync();
        }

        public async Task<IEnumerable<OrderDetail>> GetAllByOrderIdAsync(int id)
        {
            return await _context.Set<OrderDetail>().Where(o => o.OrderId == id).Include(o => o.Order).ToListAsync();
        }

        public async Task<OrderDetail> GetByIdAsync(int id)
        {
            return await _context.Set<OrderDetail>().FindAsync(id); // Use FindAsync
        }

        // Example of a more complex query (you would add these as needed)
        public async Task<IEnumerable<OrderDetail>> FindByConditionAsync(Expression<Func<OrderDetail, bool>> predicate)
        {
            return await _context.Set<OrderDetail>().Where(predicate).ToListAsync();
        }
    }
}
