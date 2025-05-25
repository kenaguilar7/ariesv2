using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AriesContador.Core.Services;
using AriesContador.Data;
using Aries.WebAPI;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Aries.Integration.Tests;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the real database context
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IConnectionString));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add test database connection
            services.AddScoped<IConnectionString>(provider => new TestConnectionString());
        });

        return base.CreateHost(builder);
    }
}

public class TestConnectionString : IConnectionString
{
    public string MySQLDefault => "Server=localhost;port=3306;User id=kenneth; pwd=1234; Database=aries; Allow User Variables=True";
} 