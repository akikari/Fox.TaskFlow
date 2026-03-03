//==================================================================================================
// Blazor application entry point with HttpClient configuration for API communication.
// Configures services and components for interactive server-side rendering.
//==================================================================================================
using System.Text.Json;
using Fox.TaskFlow.Blazor.Components;
using Fox.TaskFlow.Blazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddSingleton<LanguageState>();
builder.Services.AddTransient<LanguageHeaderHandler>();

builder.Services.AddHttpClient(nameof(TaskService), client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "https://localhost:5001");
})
.AddHttpMessageHandler<LanguageHeaderHandler>();

builder.Services.AddScoped<TaskService>();

builder.Services.AddSingleton(new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
