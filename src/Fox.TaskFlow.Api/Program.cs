//==================================================================================================
// API application entry point with full Fox.*Kit integration and localization.
// Configures ResultKit, ValidationKit, MapKit, ConfigKit, EF Core, localization, and all services.
//==================================================================================================
using System.Globalization;
using Fox.ConfigKit;
using Fox.ConfigKit.Validation;
using Fox.TaskFlow.Application;
using Fox.TaskFlow.Application.Configuration;
using Fox.TaskFlow.Infrastructure;
using Fox.TaskFlow.Infrastructure.Configuration;
using Fox.TaskFlow.Infrastructure.Data;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConfigKit<ValidationConfiguration>("Validation")
    .InRange(c => c.TitleMinLength, 1, 100, "Title minimum length must be between 1 and 100")
    .InRange(c => c.TitleMaxLength, 10, 1000, "Title maximum length must be between 10 and 1000")
    .InRange(c => c.DescriptionMinLength, 1, 100, "Description minimum length must be between 1 and 100")
    .InRange(c => c.DescriptionMaxLength, 100, 10000, "Description maximum length must be between 100 and 10000")
    .ValidateOnStartup();

builder.Services.AddConfigKit<TaskFlowConfiguration>("TaskFlow")
    .NotEmpty(c => c.Database.ConnectionString, "Database connection string is required")
    .InRange(c => c.Database.CommandTimeoutSeconds, 1, 600, "Database command timeout must be between 1 and 600 seconds")
    .When(c => c.Webhook.Enabled, b =>
    {
        b.NotEmpty(c => c.Webhook.Url, "Webhook URL is required when webhooks are enabled")
         .InRange(c => c.Webhook.MaxRetries, 0, 10, "Webhook max retries must be between 0 and 10")
         .InRange(c => c.Webhook.RetryDelayMs, 100, 30000, "Webhook retry delay must be between 100 and 30000 milliseconds")
         .InRange(c => c.Webhook.TimeoutSeconds, 1, 300, "Webhook timeout must be between 1 and 300 seconds");
    })
    .ValidateOnStartup();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddHttpContextAccessor();

builder.Services.AddLocalization();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { new CultureInfo("en"), new CultureInfo("hu") };
    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TaskFlowDbContext>();
    await dbContext.Database.MigrateAsync();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();

app.UseRequestLocalization();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

