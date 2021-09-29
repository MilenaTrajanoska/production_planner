using Microsoft.EntityFrameworkCore;
using ProductionPlanner.Domain.Models;
using ProductionPlanner.Repository.Implementation;
using ProductionPlanner.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProductionPlanner.Service.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> productRepository;
        public ProductService(IRepository<Product> _productRepository)
        {
            productRepository = _productRepository;
        }

        public Product CreateNewProduct(Product product)
        {
            return this.productRepository.Insert(product);
        }

        public List<Product> GetAllProducts()
        {
            return this.productRepository.getEntities().Include(p => p.MaterialForProduct).Include("MaterialForProduct.Material").ToList();

        }

        public Product GetProduct(long id)
        {
            return this.productRepository.getEntities().Include(p => p.MaterialForProduct).Include("MaterialForProduct.Material").Where(p=>p.Id==id).FirstOrDefault();
        }

        public void UpdateExistingProduct(Product product)
        {
            this.productRepository.Update(product);
        }

        public void DeleteProduct(long id)
        {
            var Product = this.GetProduct(id);
            this.productRepository.Delete(Product);
        }
    }
}
