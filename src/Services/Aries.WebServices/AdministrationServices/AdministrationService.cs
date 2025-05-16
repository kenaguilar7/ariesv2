﻿using Aries.WebServices.FinancialServices;
using AriesContador.Core;
using AriesContador.Core.Models.Companies;
using AriesContador.Core.Models.Users;
using AriesContador.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aries.WebServices.AdministrationServices
{
    public class AdministrationService : IAdministrationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountService _accountService;

        public AdministrationService(IUnitOfWork unitOfWork, IAccountService accountService)
        {
            _unitOfWork = unitOfWork;
            this._accountService = accountService;
        }

        public async Task CreateCompany(Company company)
        {
            company.Code = await GetCompanyConsecutive(); 
            company.CreatedAt = DateTime.Now;
            company.UpdateAt = DateTime.Now;
            var accounts = await _accountService.GetAccounts(company.CopyFrom);
            foreach (var item in accounts)
            {
                item.CompanyId = company.Code; 
            }

            company.Account = accounts; 

            await _unitOfWork.CompanyRepository.AddAsync(company);
        }

        public void CreateUser(User usuario)
        {
            _unitOfWork.UserRepository.Add(usuario);
        }

        public async Task DeleteCompany(Company company)
        {
            await _unitOfWork.CompanyRepository.Remove(company);
        }

        public Company FindByCode(string code)
        {
            throw new NotImplementedException();
        }

        public User FinUserById(int id)
        {
            var user = _unitOfWork.UserRepository.GetById(id);
            return user;
        }

        public async Task<IEnumerable<Company>> GetAllCompanies()
        {
            var output = await _unitOfWork.CompanyRepository.GetAll();
            return output;
        }

        public IEnumerable<Company> GetAllInactiveCompanies()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAllInactiveUsers()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAllUsers()
        {
            var output = _unitOfWork.UserRepository.GetAll();
            return output;
        }

        public async Task<string> GetCompanyConsecutive()
        {
            var lastCompany = await _unitOfWork.CompanyRepository.LatestCode();
            return "C" + (int.Parse(lastCompany.Substring(1, 3)) + 1).ToString("000"); 
        }

        public void InactivateUser(User usuario)
        {
            throw new NotImplementedException();
        }

        public void UpdateCompany(Company compania)
        {
            _unitOfWork.CompanyRepository.Update(compania);
        }

        public void UpdateUser(User usuario)
        {
            _unitOfWork.UserRepository.Update(usuario);
        }
    }
}
