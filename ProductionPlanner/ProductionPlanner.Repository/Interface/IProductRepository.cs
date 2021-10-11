using Microsoft.EntityFrameworkCore;
using ProductionPlanner.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductionPlanner.Repository.Interface
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        Product Get(long? id);
        Product Insert(Product entity);
        void Update(Product entity);
        void Delete(Product entity);
    }
}
