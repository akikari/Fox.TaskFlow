//==================================================================================================
// Dependency injection configuration for Application layer services.
// Registers services, validators, and ChainKit handlers.
//==================================================================================================
using Fox.ChainKit;
using Fox.TaskFlow.Application.Handlers;
using Fox.TaskFlow.Application.Interfaces;
using Fox.TaskFlow.Application.Services;
using Fox.TaskFlow.Application.Validators;
using Fox.TaskFlow.Domain.Workflows;
using Microsoft.Extensions.DependencyInjection;

namespace Fox.TaskFlow.Application;

//==================================================================================================
/// <summary>
/// Extension methods for configuring Application layer services.
/// </summary>
//==================================================================================================
public static class DependencyInjection
{
    //==============================================================================================
    /// <summary>
    /// Adds Application layer services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    //==============================================================================================
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<LocalizedMessageProvider>();
        services.AddScoped<CreateTaskRequestValidator>();
        services.AddScoped<UpdateTaskRequestValidator>();
        services.AddScoped<TransitionTaskRequestValidator>();

        services.AddTransient<ValidateTransitionHandler>();
        services.AddTransient<ExecuteTransitionHandler>();
        services.AddTransient<WebhookNotificationHandler>();

        services.AddTransient(sp =>
        {
            return new ChainBuilder<TaskTransitionContext>(sp)
                .AddHandler<ValidateTransitionHandler>()
                .AddHandler<ExecuteTransitionHandler>()
                .AddHandler<WebhookNotificationHandler>()
                .Build();
        });

        return services;
    }
}
