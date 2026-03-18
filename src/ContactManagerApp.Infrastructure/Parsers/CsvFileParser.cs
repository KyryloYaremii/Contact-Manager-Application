using ContactManagerApp.Application.Mappings;
using ContactManagerApp.Domain.Entities;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using ContactManagerApp.Domain.ParserInterfaces;

namespace ContactManagerApp.Infrastructure.Parsers;
public class CsvFileParser : ICsvFileParser
{
    public async Task<IEnumerable<Employee>> ParseAsync(Stream stream)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            MissingFieldFound = null,
            TrimOptions = TrimOptions.Trim
        };

        using var reader = new StreamReader(stream, leaveOpen: true);
        using var csv = new CsvReader(reader, config);

        csv.Context.RegisterClassMap<EmployeeCsvMap>();

        var employees = new List<Employee>();
        await foreach (var record in csv.GetRecordsAsync<Employee>())
        {
            employees.Add(record);
        }

        return employees;
    }
}