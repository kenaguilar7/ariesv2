using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aries.Contabilidad.Core.Models;

namespace Aries.Contabilidad.Core.Services
{
    public interface ICoreCompanyService
    {
        Task<IEnumerable<Company>> GetAllCompaniesAsync();
        Task<Company> GetCompanyByIdAsync(Guid id);
        Task<Company> CreateCompanyAsync(Company company);
        Task<Company> UpdateCompanyAsync(Company company);
        Task<bool> DeleteCompanyAsync(Guid id);
    }
} 