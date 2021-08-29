using ProductionPlanner.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductionPlanner.Repository.Interface
{
    public interface ICompanyRepository
    {
        void Create(Company company);
        Company Get();
        void Update(Company company);
    }
}
