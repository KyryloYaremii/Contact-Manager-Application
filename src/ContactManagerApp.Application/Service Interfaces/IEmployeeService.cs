using Microsoft.AspNetCore.Mvc;
using ContactManagerApp.Application.DTOs;

namespace ContactManagerApp.Application.Interfaces;

public interface IEmployeeService
{
    Task<int> ProcessAndSaveAsync(Stream fileStream);
    Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
    Task UpdateEmployeeAsync(int id, EmployeeUpdateDto updateDto);
    Task DeleteEmployeeAsync(int id);
}