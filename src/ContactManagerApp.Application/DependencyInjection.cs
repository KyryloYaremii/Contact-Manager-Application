using ContactManagerApp.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using ContactManagerApp.Application.Services;

namespace ContactManagerApp.Application.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {

        services.AddScoped<IEmployeeService, EmployeeService>();
        return services;
    }
}
