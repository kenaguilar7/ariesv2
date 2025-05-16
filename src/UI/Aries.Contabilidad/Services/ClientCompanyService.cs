using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Aries.Contabilidad.Models.DTOs;

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

        public ClientCompanyService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("AriesAPI");
        }

        public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<CompanyDto>>($"{ApiEndpoint}/getAll") 
                ?? Enumerable.Empty<CompanyDto>();
        }

        public async Task<CompanyDto> GetCompanyByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<CompanyDto>($"{ApiEndpoint}/{id}");
        }

        public async Task<CompanyDto> CreateCompanyAsync(CompanyDto companyDto)
        {
            var response = await _httpClient.PostAsJsonAsync(ApiEndpoint, companyDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CompanyDto>();
        }

        public async Task<CompanyDto> UpdateCompanyAsync(CompanyDto companyDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"{ApiEndpoint}/{companyDto.Id}", companyDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CompanyDto>();
        }

        public async Task DeleteCompanyAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{ApiEndpoint}/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
} 