using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Aries.Contabilidad;
using Aries.Contabilidad.Core.Data;
using Aries.Contabilidad.Core.Services;
using Aries.Contabilidad.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register data layer as singleton since it's managing in-memory data

// Register client service that depends on core service
builder.Services.AddScoped<IClientCompanyService, ClientCompanyService>();
builder.Services.AddScoped<ICoreCompanyService, CoreCompanyService>();
builder.Services.AddScoped<ICoreCompanyRepository, CoreCompanyRepository>();

await builder.Build().RunAsync();
