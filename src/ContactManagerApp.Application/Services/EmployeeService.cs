using ContactManagerApp.Application.DTOs;
using ContactManagerApp.Application.Interfaces;
using ContactManagerApp.Domain.ParserInterfaces;
using ContactManagerApp.Domain.RepositoryInterfaces;
using ContactManagerApp.Shared.Exceptions;

namespace ContactManagerApp.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;
    private readonly ICsvFileParser _parser;

    public EmployeeService(IEmployeeRepository repository, ICsvFileParser parser)
    {
        _repository = repository;
        _parser = parser;
    }

    public async Task<int> ProcessAndSaveAsync(Stream fileStream)
    {
        var employees = await _parser.ParseAsync(fileStream);
        var employeeList = employees.ToList();

        if (!employeeList.Any())
        {
            throw new InvalidFileFormatException("The CSV file contains no valid data rows.");
        }

        await _repository.AddRangeAsync(employeeList);

        return employeeList.Count;
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
    {
        var employees = await _repository.GetAllAsync();
        return employees.Select(e => new EmployeeDto(
              e.Id,
              e.Name,
              e.DateOfBirth,
              e.Married,
              e.Phone,
              e.Salary
          ));
    }

    public async Task UpdateEmployeeAsync(int id, EmployeeUpdateDto updateDto)
    {
        var employee = await _repository.GetByIdAsync(id);

        if (employee == null)
            throw new NotFoundException($"Employee with id {id} not found.");

        employee.Name = updateDto.Name;
        employee.DateOfBirth = updateDto.DateOfBirth;
        employee.Married = updateDto.Married;
        employee.Phone = updateDto.Phone;
        employee.Salary = updateDto.Salary;

        await _repository.UpdateAsync(employee);
    }

    public async Task DeleteEmployeeAsync(int id)
    {
        var employee = await _repository.GetByIdAsync(id);

        if (employee == null)
            throw new NotFoundException($"Employee with id {id} not found.");

        await _repository.DeleteAsync(employee);
    }
}