using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Aries.Contabilidad.Models.DTOs;
using Microsoft.Extensions.Logging;

namespace Aries.Contabilidad.Services
{
    /// <summary>
    /// Client-side service that acts as a facade for company operations.
    /// This service will be replaced with API calls in the future.
    /// </summary>
    public class ClientCompanyService : IClientCompanyService
    {
        private readonly HttpClient _httpClient;
        private const string ApiEndpoint = "Company";
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly ILogger<ClientCompanyService> _logger;

        public ClientCompanyService(IHttpClientFactory httpClientFactory, ILogger<ClientCompanyService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("AriesAPI");
            _logger = logger;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };
        }

        public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync()
        {
            try
            {
                _logger.LogInformation("Attempting to get all companies from {Endpoint}", $"{_httpClient.BaseAddress}{ApiEndpoint}/getAll");
                var response = await _httpClient.GetAsync($"{ApiEndpoint}/getAll");
                _logger.LogInformation("Response status code: {StatusCode}", response.StatusCode);
                
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("GetAllCompanies response: {Content}", content);
                return JsonSerializer.Deserialize<IEnumerable<CompanyDto>>(content, _jsonOptions) 
                    ?? Enumerable.Empty<CompanyDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting companies. BaseAddress: {BaseAddress}, Error: {Error}", 
                    _httpClient.BaseAddress, ex.Message);
                throw;
            }
        }

        public async Task<CompanyDto> GetCompanyByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{ApiEndpoint}/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CompanyDto>(content, _jsonOptions);
        }

        public async Task<CompanyDto> CreateCompanyAsync(CompanyDto companyDto)
        {
            try
            {
                _logger.LogInformation("Creating company: {@Company}", companyDto);
                var response = await _httpClient.PostAsJsonAsync($"{ApiEndpoint}/Create", companyDto, _jsonOptions);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Company created successfully: {Content}", content);
                return JsonSerializer.Deserialize<CompanyDto>(content, _jsonOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating company: {@Company}, Error: {Error}", 
                    companyDto, ex.Message);
                throw;
            }
        }

        public async Task<CompanyDto> UpdateCompanyAsync(CompanyDto companyDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"{ApiEndpoint}/{companyDto.Id}", companyDto, _jsonOptions);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CompanyDto>(content, _jsonOptions);
        }

        public async Task DeleteCompanyAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{ApiEndpoint}/Delete/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<CompanyCodeDto> GetNextCompanyCode()
        {
            try
            {
                _logger.LogInformation("Fetching next company code");
                var response = await _httpClient.GetAsync($"{ApiEndpoint}/BuildCode");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Next company code response: {Content}", content);
                return JsonSerializer.Deserialize<CompanyCodeDto>(content, _jsonOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching next company code: {Error}", ex.Message);
                throw;
            }
        }
    }

    public class CompanyCodeDto
    {
        public string Code { get; set; }
    }
} 