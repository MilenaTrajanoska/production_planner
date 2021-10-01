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
        private readonly IRepository<ProductHistory> productHistoryRepository;
        public ProductService(IRepository<Product> _productRepository, IRepository<ProductHistory> _productHistoryRepository)
        {
            productRepository = _productRepository;
            productHistoryRepository = _productHistoryRepository;
        }

        public Product CreateNewProduct(Product product)
        {
            product.IsValid = true;
            product.LastModified = DateTime.Now;

            ProductHistory productHistory = new ProductHistory(product);
            var p = this.productRepository.Insert(product);
            productHistory.ProductId = p.Id;
            productHistoryRepository.Insert(productHistory);

            return p;
        }

        public List<Product> GetAllProducts()
        {
            return this.productRepository.getEntities()
                .Where(p => p.IsValid)
                .Include(p => p.MaterialForProduct)
                .Include("MaterialForProduct.Material")
                .ToList();

        }

        public Product GetProduct(long id)
        {
            return this.productRepository.getEntities()
                .Include(p => p.MaterialForProduct)
                .Include("MaterialForProduct.Material")
                .Where(p=>p.Id==id && p.IsValid)
                .FirstOrDefault();
        }

        public void UpdateExistingProduct(Product product)
        {
            product.CurrentVersion += 1;
            product.LastModified = DateTime.Now;

            var history = productHistoryRepository.getEntities()
                .Where(e => e.ProductId == product.Id && e.Version == product.CurrentVersion && e.isValid)
                .FirstOrDefault();
            if(history != null)
            {
                history.isValid = false;
                history.ModificationDate = DateTime.Now;
                productHistoryRepository.Update(history);
            }

            var newHistory = new ProductHistory(product);
            productHistoryRepository.Insert(newHistory);

            this.productRepository.Update(product);
        }

        public void DeleteProduct(long id)
        {
            var product = this.GetProduct(id);
            product.IsValid = false;
            productRepository.Update(product);
        }
    }
}
