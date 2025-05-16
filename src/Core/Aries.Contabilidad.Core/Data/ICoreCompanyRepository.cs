using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aries.Contabilidad.Core.Models;

namespace Aries.Contabilidad.Core.Data
{
    public interface ICoreCompanyRepository
    {
        Task<IEnumerable<Company>> GetAllAsync();
        Task<Company> GetByIdAsync(Guid id);
        Task<Company> AddAsync(Company company);
        Task<Company> UpdateAsync(Company company);
        Task<bool> ExistsByTaxIdAsync(string taxId, Guid? excludeId = null);
    }
} 