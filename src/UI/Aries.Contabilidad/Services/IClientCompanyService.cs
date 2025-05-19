using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aries.Contabilidad.Models.DTOs;

namespace Aries.Contabilidad.Services
{
    /// <summary>
    /// Defines the contract for client-side company operations.
    /// This interface will be implemented by services that handle API communication in the future.
    /// </summary>
    public interface IClientCompanyService
    {
        Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync();
        Task<CompanyDto> GetCompanyByIdAsync(int id);
        Task<CompanyDto> CreateCompanyAsync(CompanyDto company);
        Task<CompanyDto> UpdateCompanyAsync(CompanyDto company);
        Task DeleteCompanyAsync(int id);
        Task<CompanyCodeDto> GetNextCompanyCode();
    }
} 