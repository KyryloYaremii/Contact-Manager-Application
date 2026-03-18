namespace ContactManagerApp.Application.DTOs;

public record EmployeeDto(
    int Id,
    string Name,
    DateTime DateOfBirth,
    bool Married,
    string Phone,
    decimal Salary);