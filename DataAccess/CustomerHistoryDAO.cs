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
    public class CustomerHistoryDAO : SingletonBase<CustomerHistoryDAO>
    {
        public async Task<CustomerHistory> AddAsync(CustomerHistory entity)
        {
            try
            {
                var entityEntry = await _context.Set<CustomerHistory>().AddAsync(entity);
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
        public async Task<CustomerHistory> UpdateAsync(CustomerHistory entity)
        {
            try
            {
                _context.Set<CustomerHistory>().Update(entity); // More efficient than attaching and setting values
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
                var entity = await _context.Set<Employee>().FindAsync(id);
                if (entity != null)
                {
                    _context.Set<Employee>().Remove(entity);
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


        public async Task<IEnumerable<CustomerHistory>> GetAllAsync()
        {
            //return await _context.Set<Employee>().ToListAsync();
            return await _context.CustomerHistories
               //.Include(st => st.Category)
               .ToListAsync();
        }
        public async Task<CustomerHistory> GetByIdAsync(int id)
        {
            return await _context.Set<CustomerHistory>().FindAsync(id); // Use FindAsync
        }

        // Example of a more complex query (you would add these as needed)
        public async Task<IEnumerable<CustomerHistory>> FindByConditionAsync(Expression<Func<CustomerHistory, bool>> predicate)
        {
            return await _context.Set<CustomerHistory>().Where(predicate).ToListAsync();
        }
    }
}

