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

        public void CreateNewProduct(Product product)
        {
            this.productRepository.Insert(product);
        }

        public List<Product> GetAllProducts()
        {
            return this.productRepository.GetAll().ToList();
        }

        public Product GetProduct(long id)
        {
            return this.productRepository.Get(id);
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
