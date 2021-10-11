using ProductionPlanner.Domain.Exceptions;
using ProductionPlanner.Domain.Models;
using ProductionPlanner.Repository.Interface;
using ProductionPlanner.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ProductionPlanner.Service.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Product CreateNewProduct(Product product)
        {
            var p = _productRepository.Insert(product);

            return p;
        }

        public List<Product> GetAllProducts()
        {
            return _productRepository.GetAll().ToList();

        }

        public Product GetProduct(long id)
        {
            return _productRepository.Get(id);
        }

        public void UpdateExistingProduct(Product product)
        {
            _productRepository.Update(product);
        }

        public void DeleteProduct(long id)
        {
            var product = GetProduct(id);
            if (product != null)
            {
                _productRepository.Delete(product);
            }
            else
            {
                throw new ProductNotFoundException(String.Format("The product with id {0} could not be found", id));
            }
        }
    }
}
