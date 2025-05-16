using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aries.Contabilidad.Core.Data;
using Aries.Contabilidad.Core.Models;

namespace Aries.Contabilidad.Core.Services
{
    public class CoreCompanyService : ICoreCompanyService
    {
        private readonly ICoreCompanyRepository _companyData;

        public CoreCompanyService(ICoreCompanyRepository companyData)
        {
            _companyData = companyData ?? throw new ArgumentNullException(nameof(companyData));
        }

        public async Task<IEnumerable<Company>> GetAllCompaniesAsync()
        {
            var companies = await _companyData.GetAllAsync();
            return companies.Where(c => c.IsActive);
        }

        public async Task<Company> GetCompanyByIdAsync(Guid id)
        {
            var company = await _companyData.GetByIdAsync(id);
            return company?.IsActive == true ? company : null;
        }

        public async Task<Company> CreateCompanyAsync(Company company)
        {
            ValidateCompany(company);
            await ValidateUniqueTaxId(company);

            company.CreatedAt = DateTime.UtcNow;
            company.IsActive = true;

            return await _companyData.AddAsync(company);
        }

        public async Task<Company> UpdateCompanyAsync(Company company)
        {
            ValidateCompany(company);

            var existingCompany = await _companyData.GetByIdAsync(company.Id);
            if (existingCompany == null || !existingCompany.IsActive)
            {
                throw new InvalidOperationException("Empresa no encontrada.");
            }

            await ValidateUniqueTaxId(company, existingCompany.Id);

            company.UpdatedAt = DateTime.UtcNow;
            company.IsActive = true; // Ensure it stays active
            
            var updatedCompany = await _companyData.UpdateAsync(company);
            if (updatedCompany == null)
            {
                throw new InvalidOperationException("Error al actualizar la empresa.");
            }

            return updatedCompany;
        }

        public async Task<bool> DeleteCompanyAsync(Guid id)
        {
            var company = await _companyData.GetByIdAsync(id);
            if (company == null || !company.IsActive)
            {
                return false;
            }

            company.IsActive = false;
            company.UpdatedAt = DateTime.UtcNow;

            await _companyData.UpdateAsync(company);
            return true;
        }

        private void ValidateCompany(Company company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }

            if (string.IsNullOrWhiteSpace(company.Name))
            {
                throw new InvalidOperationException("El nombre de la empresa es requerido.");
            }

            if (string.IsNullOrWhiteSpace(company.TaxId))
            {
                throw new InvalidOperationException("La identificación fiscal es requerida.");
            }
        }

        private async Task ValidateUniqueTaxId(Company company, Guid? excludeId = null)
        {
            if (await _companyData.ExistsByTaxIdAsync(company.TaxId, excludeId))
            {
                throw new InvalidOperationException("Ya existe una empresa con este número de identificación fiscal.");
            }
        }
    }
} 