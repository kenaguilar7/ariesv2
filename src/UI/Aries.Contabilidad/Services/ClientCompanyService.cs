using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aries.Contabilidad.Core.Models;
using Aries.Contabilidad.Core.Services;

namespace Aries.Contabilidad.Services
{
    /// <summary>
    /// Client-side service that acts as a facade for company operations.
    /// This service will be replaced with API calls in the future.
    /// </summary>
    public class ClientCompanyService : IClientCompanyService
    {
        private readonly ICoreCompanyService _coreService;

        public ClientCompanyService(ICoreCompanyService coreService)
        {
            _coreService = coreService ?? throw new ArgumentNullException(nameof(coreService));
        }

        public async Task<IEnumerable<Company>> GetAllCompaniesAsync()
        {
            // In the future, this will make an HTTP GET request
            return await _coreService.GetAllCompaniesAsync();
        }

        public async Task<Company> GetCompanyByIdAsync(Guid id)
        {
            // In the future, this will make an HTTP GET request
            return await _coreService.GetCompanyByIdAsync(id);
        }

        public async Task<Company> CreateCompanyAsync(Company company)
        {
            // In the future, this will make an HTTP POST request
            return await _coreService.CreateCompanyAsync(company);
        }

        public async Task<Company> UpdateCompanyAsync(Company company)
        {
            // In the future, this will make an HTTP PUT request
            return await _coreService.UpdateCompanyAsync(company);
        }

        public async Task<bool> DeleteCompanyAsync(Guid id)
        {
            // In the future, this will make an HTTP DELETE request
            return await _coreService.DeleteCompanyAsync(id);
        }
    }
} 