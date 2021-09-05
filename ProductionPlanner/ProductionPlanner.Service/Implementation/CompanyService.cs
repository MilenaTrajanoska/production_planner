using ProductionPlanner.Domain.Models;
using ProductionPlanner.Repository.Interface;
using ProductionPlanner.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductionPlanner.Service.Implementation
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository companyRepository;

        public CompanyService(ICompanyRepository _companyRepository)
        {
            this.companyRepository = _companyRepository;
        }
        public void CreateNewCompany(Company company)
        {
            this.companyRepository.Create(company);
        }

        public Company GetCompany()
        {
            return this.companyRepository.Get();
        }
    }
}
