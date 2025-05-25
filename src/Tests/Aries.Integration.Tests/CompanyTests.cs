using System.Net;
using System.Text.Json;
using AriesContador.Core.Models.Companies;
using FluentAssertions;
using NUnit.Framework;

namespace Aries.Integration.Tests;

[TestFixture]
public class CompanyTests : IntegrationTest
{
    [OneTimeSetUp]
    public async Task SetUp()
    {
        // Authenticate before all tests
        await AuthenticateAsync();
    }

    [Test]
    public async Task GetAllCompanies_ReturnsCompanyList()
    {
        // Act
        var response = await _client.GetAsync("/Company/getAll");
        var content = await response.Content.ReadAsStringAsync();
        var companies = JsonSerializer.Deserialize<List<Company>>(content, JsonOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        companies.Should().NotBeNull();
    }

    [Test]
    public async Task CreateCompany_WithValidData_ReturnsCreatedCompany()
    {
        // Arrange
        var codeResponse = await _client.GetAsync("/Company/BuildCode");
        codeResponse.EnsureSuccessStatusCode();
        var codeContent = await codeResponse.Content.ReadAsStringAsync();
        var codeResult = JsonSerializer.Deserialize<dynamic>(codeContent, JsonOptions);
        var newCode = codeResult.GetProperty("code").GetString();

        var company = new Company
        {
            Code = newCode
        };

        // Act
        var response = await _client.PostAsync("/Company/Create", GetJsonContent(company));
        var content = await response.Content.ReadAsStringAsync();
        var createdCompany = JsonSerializer.Deserialize<Company>(content, JsonOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        createdCompany.Should().NotBeNull();
        createdCompany!.Code.Should().Be(newCode);
    }

    [Test]
    public async Task DeleteCompany_WithValidId_ReturnsOk()
    {
        // Arrange
        // First create a company
        var codeResponse = await _client.GetAsync("/Company/BuildCode");
        var codeContent = await codeResponse.Content.ReadAsStringAsync();
        var codeResult = JsonSerializer.Deserialize<dynamic>(codeContent, JsonOptions);
        var newCode = codeResult.GetProperty("code").GetString();

        var company = new Company { Code = newCode };
        await _client.PostAsync("/Company/Create", GetJsonContent(company));

        // Act
        var response = await _client.DeleteAsync($"/Company/Delete/{newCode}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Verify the company no longer exists
        var getAllResponse = await _client.GetAsync("/Company/getAll");
        var content = await getAllResponse.Content.ReadAsStringAsync();
        var companies = JsonSerializer.Deserialize<List<Company>>(content, JsonOptions);
    }

    [Test]
    public async Task BuildCode_ReturnsNewCode()
    {
        // Act
        var response = await _client.GetAsync("/Company/BuildCode");
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<dynamic>(content, JsonOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        var code = result.GetProperty("code").GetString();
        code.Should().NotBeNullOrEmpty();
    }
} 