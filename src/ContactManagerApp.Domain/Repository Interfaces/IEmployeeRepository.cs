using ContactManagerApp.Domain.Entities;

namespace ContactManagerApp.Domain.RepositoryInterfaces;

public interface IEmployeeRepository
{
    Task AddRangeAsync(IEnumerable<Employee> employees);
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<Employee?> GetByIdAsync(int id);
    Task UpdateAsync(Employee employee);
    Task DeleteAsync(Employee employee);
}