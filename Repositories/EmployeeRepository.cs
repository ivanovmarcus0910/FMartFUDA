using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
namespace Repositories
{
    public class EmployeeRepository : IFormRepositories<Employee>
    {
        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await EmployeeDAO.Instance.GetAllAsync(); // Correct: await the async method
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            return await EmployeeDAO.Instance.GetByIdAsync(id); // Correct: await the async method
        }

        public async Task<Employee> AddAsync(Employee entity)
        {
            return await EmployeeDAO.Instance.AddAsync(entity); // Call the base AddAsync
        }

        public async Task<Employee> UpdateAsync(Employee entity)
        {
            return await EmployeeDAO.Instance.UpdateAsync(entity); // Correct: await and return
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await EmployeeDAO.Instance.DeleteAsync(id); // Correct: await and return
        }
    }
}
