using ProductionPlanner.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductionPlanner.Service.Interface
{
    public interface IProductService
    {
        void CreateNewProduct(Product product);
        List<Product> GetAllProducts();
        Product GetProduct(long id);
        void UpdateExistingProduct(Product product);
        void DeleteProduct(long id);
    }
}
