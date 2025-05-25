using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using AriesContador.Core.Models.Users;
using NUnit.Framework;

namespace Aries.Integration.Tests;

[TestFixture]
public class IntegrationTest
{
    protected readonly HttpClient _client;
    protected readonly JsonSerializerOptions JsonOptions;

    private readonly TestWebApplicationFactory Factory;

    public IntegrationTest()
    {
        Factory = new TestWebApplicationFactory();
        _client = Factory.CreateClient();
        JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    [OneTimeSetUp]
    public void Setup()
    {
        // Any one-time setup code can go here  
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _client?.Dispose();
        Factory?.Dispose();
    }

    protected async Task AuthenticateAsync()
    {
        _client.DefaultRequestHeaders.Authorization = null;

        var response = await _client.PostAsync("/Auth/login", new StringContent(
            JsonSerializer.Serialize(new Login { UserId = "admin", Password = "admin" }),
            Encoding.UTF8,
            "application/json"));

        response.EnsureSuccessStatusCode();

        var webToken = JsonSerializer.Deserialize<WebToken>(
            await response.Content.ReadAsStringAsync(),
            JsonOptions);

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", webToken?.Token);
    }

    protected StringContent GetJsonContent(object data)
        => new StringContent(
            JsonSerializer.Serialize(data),
            Encoding.UTF8,
            "application/json");
}
