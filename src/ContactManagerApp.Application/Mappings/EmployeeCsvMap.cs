using ContactManagerApp.Domain.Entities;
using CsvHelper.Configuration;

namespace ContactManagerApp.Application.Mappings;

public sealed class EmployeeCsvMap : ClassMap<Employee>
{
    public EmployeeCsvMap()
    {
        Map(m => m.Name).Name("Name");
        Map(m => m.DateOfBirth).Name("Date of birth");
        Map(m => m.Married).Name("Married");
        Map(m => m.Phone).Name("Phone");
        Map(m => m.Salary).Name("Salary");
    }
}