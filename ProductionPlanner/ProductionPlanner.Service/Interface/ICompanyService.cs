using ProductionPlanner.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductionPlanner.Service.Interface
{
    public interface ICompanyService
    {
        Company GetCompany();
        public void UpdateCompany(Company company);
    }
}
