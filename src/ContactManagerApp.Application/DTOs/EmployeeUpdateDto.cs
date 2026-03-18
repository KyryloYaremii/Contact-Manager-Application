namespace ContactManagerApp.Application.DTOs;

public record EmployeeUpdateDto(
    string Name, 
    DateTime DateOfBirth, 
    bool Married, 
    string Phone, 
    decimal Salary);