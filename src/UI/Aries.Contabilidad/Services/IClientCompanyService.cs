using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aries.Contabilidad.Core.Models;

namespace Aries.Contabilidad.Services
{
    /// <summary>
    /// Defines the contract for client-side company operations.
    /// This interface will be implemented by services that handle API communication in the future.
    /// </summary>
    public interface IClientCompanyService
    {
        Task<IEnumerable<Company>> GetAllCompaniesAsync();
        Task<Company> GetCompanyByIdAsync(Guid id);
        Task<Company> CreateCompanyAsync(Company company);
        Task<Company> UpdateCompanyAsync(Company company);
        Task<bool> DeleteCompanyAsync(Guid id);
    }
} 