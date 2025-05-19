using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Aries.Contabilidad;
using Aries.Contabilidad.Services;
using Aries.Contabilidad.Configuration;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Load configuration
var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();

// Configure logging
builder.Logging.SetMinimumLevel(LogLevel.Information);

// Register services
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IClientCompanyService, ClientCompanyService>();

// Configure HttpClient with environment-specific settings

var apiBaseUrl = "http://localhost:5000/";
builder.Services.AddHttpClient("AriesAPI", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
    client.Timeout = TimeSpan.FromSeconds(apiSettings?.Timeout ?? 30);
})
.AddHttpMessageHandler<AuthenticationHeaderHandler>();

// Add authentication message handler
builder.Services.AddScoped<AuthenticationHeaderHandler>();

// Make ApiSettings available for dependency injection
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

await builder.Build().RunAsync();
