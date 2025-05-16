using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aries.Contabilidad.Core.Models;

namespace Aries.Contabilidad.Core.Services
{
    //public class CoreCompanyService : ICompanyService
    //{
    //    private readonly List<Company> _companies;

    //    public CoreCompanyService()
    //    {
    //        _companies = new List<Company>();
    //    }

    //    public async Task<IEnumerable<Company>> GetAllCompaniesAsync()
    //    {
    //        // Business logic: Only return active companies
    //        return await Task.FromResult(_companies.Where(c => c.IsActive).ToList());
    //    }

    //    public async Task<Company> GetCompanyByIdAsync(Guid id)
    //    {
    //        // Business logic: Only return active company
    //        return await Task.FromResult(_companies.FirstOrDefault(c => c.Id == id && c.IsActive));
    //    }

    //    public async Task<Company> CreateCompanyAsync(Company company)
    //    {
    //        ValidateCompany(company);
    //        ValidateUniqueTaxId(company);

    //        _companies.Add(company);
    //        return await Task.FromResult(company);
    //    }

    //    public async Task<Company> UpdateCompanyAsync(Company company)
    //    {
    //        ValidateCompany(company);

    //        var existingCompany = await GetCompanyByIdAsync(company.Id);
    //        if (existingCompany == null)
    //        {
    //            throw new InvalidOperationException("Empresa no encontrada.");
    //        }

    //        ValidateUniqueTaxId(company, existingCompany.Id);

    //        // Update properties
    //        existingCompany.Name = company.Name;
    //        existingCompany.TaxId = company.TaxId;
    //        existingCompany.Address = company.Address;
    //        existingCompany.Phone = company.Phone;
    //        existingCompany.Email = company.Email;
    //        existingCompany.Website = company.Website;
    //        existingCompany.UpdatedAt = DateTime.UtcNow;

    //        return await Task.FromResult(existingCompany);
    //    }

    //    public async Task<bool> DeleteCompanyAsync(Guid id)
    //    {
    //        var company = _companies.FirstOrDefault(c => c.Id == id);
    //        if (company == null)
    //        {
    //            return await Task.FromResult(false);
    //        }

    //        // Business logic: Soft delete
    //        company.IsActive = false;
    //        company.UpdatedAt = DateTime.UtcNow;
            
    //        return await Task.FromResult(true);
    //    }

    //    private void ValidateCompany(Company company)
    //    {
    //        if (company == null)
    //        {
    //            throw new ArgumentNullException(nameof(company));
    //        }

    //        // Add any additional business validation rules here
    //        if (string.IsNullOrWhiteSpace(company.Name))
    //        {
    //            throw new InvalidOperationException("El nombre de la empresa es requerido.");
    //        }

    //        if (string.IsNullOrWhiteSpace(company.TaxId))
    //        {
    //            throw new InvalidOperationException("La identificación fiscal es requerida.");
    //        }
    //    }

    //    private void ValidateUniqueTaxId(Company company, Guid? excludeId = null)
    //    {
    //        var query = _companies.Where(c => c.TaxId == company.TaxId && c.IsActive);
            
    //        if (excludeId.HasValue)
    //        {
    //            query = query.Where(c => c.Id != excludeId.Value);
    //        }

    //        if (query.Any())
    //        {
    //            throw new InvalidOperationException("Ya existe una empresa con este número de identificación fiscal.");
    //        }
    //    }
    //}
} 