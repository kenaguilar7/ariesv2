using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Aries.Contabilidad;
using Aries.Contabilidad.Services;
using Aries.Contabilidad.Shared;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<IClientCompanyService, ClientCompanyService>();
builder.Services.AddHttpClient("AriesAPI", client => 
{
    client.BaseAddress = new Uri(builder.Configuration["ApiUrl"] ?? "https://localhost:7001/");
});

await builder.Build().RunAsync();
