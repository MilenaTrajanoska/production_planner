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
        private const string errorMessage = "Cannot create more than one company";

        public CompanyService(ICompanyRepository _companyRepository)
        {
            this.companyRepository = _companyRepository;
        }

        public Company GetCompany()
        {
            return this.companyRepository.Get();
        }

        public void UpdateCompany(Company company)
        {
            this.companyRepository.Update(company);
        }
    }
}
