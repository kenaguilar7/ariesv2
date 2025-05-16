using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aries.Contabilidad.Core.Models;

namespace Aries.Contabilidad.Core.Data
{
    public class CoreCompanyRepository : ICoreCompanyRepository
    {
        private readonly List<Company> _companies;

        public CoreCompanyRepository()
        {
            _companies = new List<Company>();
        }

        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            return await Task.FromResult(_companies.ToList());
        }

        public async Task<Company> GetByIdAsync(Guid id)
        {
            return await Task.FromResult(_companies.FirstOrDefault(c => c.Id == id));
        }

        public async Task<Company> AddAsync(Company company)
        {
            _companies.Add(company);
            return await Task.FromResult(company);
        }

        public async Task<Company> UpdateAsync(Company company)
        {
            var existingCompany = _companies.FirstOrDefault(c => c.Id == company.Id);
            if (existingCompany != null)
            {
                existingCompany.Name = company.Name;
                existingCompany.TaxId = company.TaxId;
                existingCompany.Address = company.Address;
                existingCompany.Phone = company.Phone;
                existingCompany.Email = company.Email;
                existingCompany.Website = company.Website;
                existingCompany.UpdatedAt = company.UpdatedAt;
                existingCompany.IsActive = company.IsActive;
            }
            return await Task.FromResult(existingCompany);
        }

        public async Task<bool> ExistsByTaxIdAsync(string taxId, Guid? excludeId = null)
        {
            var query = _companies.Where(c => c.TaxId == taxId && c.IsActive);
            
            if (excludeId.HasValue)
            {
                query = query.Where(c => c.Id != excludeId.Value);
            }

            return await Task.FromResult(query.Any());
        }
    }
} 