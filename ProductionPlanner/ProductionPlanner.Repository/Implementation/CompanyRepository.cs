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
            company.isValid = true;
            company.IsSet = true;
            companies.Add(company);
            context.SaveChanges();
        }

        public Company Get()
        {
            return companies.Where(c => c.isValid).FirstOrDefault();
        }

        //this is a transaction, will complete if all steps are executed successfully, else it will rollback
        public void Update(Company company)
        {
            try
            {
                using var transaction = context.Database.BeginTransaction();
                var presentCompany = companies.Where(c => c.isValid).FirstOrDefault();

                if (presentCompany == null)
                {
                    throw new InvalidOperationException(errorMessage);
                }

                presentCompany.isValid = false;
                companies.Update(presentCompany);
                context.SaveChanges();

                company.Id = 0;
                company.isValid = true;
                company.IsSet = true;
                companies.Add(company);
                context.SaveChanges();
                transaction.Commit();
            }catch
            {
                throw new Exception("Could not update company values.");
            }
            
        }
    }
}
