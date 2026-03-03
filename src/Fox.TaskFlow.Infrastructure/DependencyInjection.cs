//==================================================================================================
// Dependency injection configuration for Infrastructure layer services.
// Registers EF Core, repositories, and services with the DI container.
//==================================================================================================
using Fox.TaskFlow.Application.Interfaces;
using Fox.TaskFlow.Infrastructure.Configuration;
using Fox.TaskFlow.Infrastructure.Data;
using Fox.TaskFlow.Infrastructure.Repositories;
using Fox.TaskFlow.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fox.TaskFlow.Infrastructure;

//==================================================================================================
/// <summary>
/// Extension methods for configuring Infrastructure layer services.
/// </summary>
//==================================================================================================
public static class DependencyInjection
{
    //==============================================================================================
    /// <summary>
    /// Adds Infrastructure layer services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>The service collection for chaining.</returns>
    //==============================================================================================
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddDbContext<TaskFlowDbContext>((serviceProvider, options) =>
        {
            var config = serviceProvider.GetRequiredService<IOptions<TaskFlowConfiguration>>().Value;
            var sqliteOptions = options.UseSqlite(config.Database.ConnectionString);

            if (config.Database.EnableDetailedErrors)
            {
                options.EnableDetailedErrors();
            }

            if (config.Database.EnableSensitiveDataLogging)
            {
                options.EnableSensitiveDataLogging();
            }
        });

        services.AddScoped<ITaskRepository, TaskRepository>();

        services.AddHttpClient();
        services.AddSingleton<IWebhookNotificationService, WebhookNotificationService>();

        return services;
    }
}
