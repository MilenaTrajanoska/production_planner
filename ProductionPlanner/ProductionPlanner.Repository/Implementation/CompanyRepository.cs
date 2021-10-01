using Microsoft.EntityFrameworkCore;
using ProductionPlanner.Domain.Models;
using ProductionPlanner.Repository.Interface;
using ProductionPlanner.Repository.Data;
using System.Linq;
using System;

namespace ProductionPlanner.Repository.Implementation
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Company> companies;
        private const string errorMessage = "Can not update company that is not created yet";

        public CompanyRepository(ApplicationDbContext context)
        {
            this.context = context;
            companies = context.Set<Company>();
        }

        public void Create(Company company)
        {
            companies.Add(company);
            context.SaveChanges();
        }

        public Company Get()
        {
            return companies.Where(c => c.isValid).FirstOrDefault();
        }

        public void Update(Company company)
        {
            var presentCompany = companies.Where(c => c.isValid).FirstOrDefault();
            
            if (presentCompany == null)
            {
                throw new InvalidOperationException(errorMessage);
            }

            presentCompany.isValid = false;
            companies.Update(presentCompany);
            company.isValid = true;
            companies.Add(company);
            context.SaveChanges();
        }
    }
}
