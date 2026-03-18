using ContactManagerApp.Domain.Entities;

namespace ContactManagerApp.Domain.ParserInterfaces;

public interface ICsvFileParser
{
    Task<IEnumerable<Employee>> ParseAsync(Stream stream);
}
