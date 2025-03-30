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
    public class ProductDAO : SingletonBase<ProductDAO>
    {
        public async Task<Product> AddAsync(Product entity)
        {
            try
            {
                var entityEntry = await _context.Set<Product>().AddAsync(entity);
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

        public async Task<Product> UpdateAsync(Product entity)
        {
            try
            {
                _context.Set<Product>().Update(entity); // More efficient than attaching and setting values
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
                var entity = await _context.Set<Product>().FindAsync(id);
                if (entity != null)
                {
                    _context.Set<Product>().Remove(entity);
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

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            using (var context = new FmartFudaContext()) 
            {
                return await context.Set<Product>().Include(st => st.Category).ToListAsync();
            }
            
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            using (var context = new FmartFudaContext())
            {
                return await context.Set<Product>().FindAsync(id);
            }
        }

        // Example of a more complex query (you would add these as needed)
        public async Task<IEnumerable<Product>> FindByConditionAsync(Expression<Func<Product, bool>> predicate)
        {
            return await _context.Set<Product>().Where(predicate).ToListAsync();
        }
    }
}
