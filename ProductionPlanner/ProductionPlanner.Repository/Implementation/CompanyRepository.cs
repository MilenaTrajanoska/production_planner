using Microsoft.EntityFrameworkCore;
using ProductionPlanner.Domain.Models;
using ProductionPlanner.Repository.Interface;
using ProductionPlanner.Repository.Data;


namespace ProductionPlanner.Repository.Implementation
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Company> companies;

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
          return companies.FirstOrDefaultAsync().Result;
        }

        public void Update(Company company)
        {
            companies.Update(company);
            context.SaveChanges();
        }
    }
}
