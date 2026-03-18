using ContactManagerApp.Domain.ParserInterfaces;
using ContactManagerApp.Domain.RepositoryInterfaces;
using ContactManagerApp.Infrastructure.Parsers;
using ContactManagerApp.Infrastructure.Persistence;
using ContactManagerApp.Infrastructure.Repositories;
using ContactManagerApp.Shared.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ContactManagerApp.Infrastructure.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        var appSettings = configuration.GetSection(ApplicationSettings.SectionName).Get<ApplicationSettings>();

        if (appSettings == null || string.IsNullOrEmpty(appSettings.ConnectionString))
        {
            throw new InvalidOperationException("Connection string is not configured.");
        }

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(appSettings.ConnectionString);
        });

        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<ICsvFileParser, CsvFileParser>();

        return services;
    }
}
