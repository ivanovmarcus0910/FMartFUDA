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
    public class RoleDAO:SingletonBase<RoleDAO>
    {
        public async Task<Role> AddAsync(Role entity)
        {
            try
            {
                var entityEntry = await _context.Set<Role>().AddAsync(entity);
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
        public async Task<Role> UpdateAsync(Role entity)
        {
            try
            {
                _context.Set<Role>().Update(entity); // More efficient than attaching and setting values
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
                var entity = await _context.Set<Role>().FindAsync(id);
                if (entity != null)
                {
                    _context.Set<Role>().Remove(entity);
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


        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            //return await _context.Set<Role>().ToListAsync();
            return await _context.Roles
               .ToListAsync();
        }
        public async Task<Role> GetByIdAsync(int id)
        {
            return await _context.Set<Role>().FindAsync(id); // Use FindAsync
        }

        // Example of a more complex query (you would add these as needed)
        public async Task<IEnumerable<Role>> FindByConditionAsync(Expression<Func<Role, bool>> predicate)
        {
            return await _context.Set<Role>().Where(predicate).ToListAsync();
        }
    }
}
