using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Models.Models;
namespace DataAccess
{
    public class EmployeeDAO : SingletonBase<EmployeeDAO>
    {
        public async Task<Employee> AddAsync(Employee entity)
        {
            try
            {
                var entityEntry = await _context.Set<Employee>().AddAsync(entity);
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
        public async Task<Employee> UpdateAsync(Employee entity)
        {
            try
            {
                _context.Set<Employee>().Update(entity); // More efficient than attaching and setting values
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


        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            //return await _context.Set<Employee>().ToListAsync();
            return await _context.Employees
               .Include(st => st.Role)
               .ToListAsync();
        }
        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _context.Set<Employee>().FindAsync(id); // Use FindAsync
        }

        // Example of a more complex query (you would add these as needed)
        public async Task<IEnumerable<Employee>> FindByConditionAsync(Expression<Func<Employee, bool>> predicate)
        {
            return await _context.Set<Employee>().Where(predicate).ToListAsync();
        }
    }
}
