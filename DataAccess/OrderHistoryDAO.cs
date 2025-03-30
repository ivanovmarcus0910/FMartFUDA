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
    public class OrderHistoryDAO : SingletonBase<OrderHistoryDAO>
    {
        public async Task<OrderHistory> AddAsync(OrderHistory entity)
        {
            try
            {
                var entityEntry = await _context.Set<OrderHistory>().AddAsync(entity);
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
        public async Task<OrderHistory> UpdateAsync(OrderHistory entity)
        {
            try
            {
                _context.Set<OrderHistory>().Update(entity); // More efficient than attaching and setting values
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
                var entity = await _context.Set<OrderHistory>().FindAsync(id);
                if (entity != null)
                {
                    _context.Set<OrderHistory>().Remove(entity);
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


        public async Task<IEnumerable<OrderHistory>> GetAllAsync()
        {
            return await _context.OrderHistories.Include(ch => ch.Employee).ToListAsync();
        }
        public async Task<OrderHistory> GetByIdAsync(int id)
        {
            return await _context.Set<OrderHistory>().FindAsync(id); // Use FindAsync
        }

        // Example of a more complex query (you would add these as needed)
        public async Task<IEnumerable<OrderHistory>> FindByConditionAsync(Expression<Func<OrderHistory, bool>> predicate)
        {
            return await _context.Set<OrderHistory>().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<OrderHistory>> GetFilteredHistoryAsync(string actionType, DateOnly? startDate, DateOnly? endDate)
        {
            var query = _context.OrderHistories.Include(ch => ch.Employee).AsQueryable();

            if (!string.IsNullOrEmpty(actionType) && actionType != "All")
            {
                query = query.Where(h => h.ActionType == actionType);
            }

            if (startDate.HasValue)
            {
                query = query.Where(h => h.ActionDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(h => h.ActionDate <= endDate.Value);
            }

            return await query.ToListAsync();
        }
    }
}
