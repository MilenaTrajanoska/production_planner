using Microsoft.EntityFrameworkCore;
using ProductionPlanner.Domain.Models;
using ProductionPlanner.Repository.Data;
using ProductionPlanner.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProductionPlanner.Repository.Implementation
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<Product> _products;
        private DbSet<ProductHistory> _productHistories;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
            _productHistories = context.ProductHistories;
            _products = context.Products;
        }

        // _context.SaveChanges() performs a transaction. If at least one statement fails, all statements are not executed.
        public void Delete(Product entity)
        {
            entity.IsValid = false;
            _products.Update(entity);
            var historiesForProduct = _productHistories.Where(h => h.ProductId == entity.Id && h.isValid).ToList();
            historiesForProduct.ForEach(h => h.isValid = false);
            foreach (ProductHistory ph in historiesForProduct)
            {
                _productHistories.Update(ph);
            }
            _context.SaveChanges();
        }

        public Product Get(long? id)
        {
            return _products.
                Include(p => p.MaterialForProduct)
                .Include("MaterialForProduct.Material")
                .Where(p => p.Id == id && p.IsValid)
                .FirstOrDefault();
        }

        public IEnumerable<Product> GetAll()
        {
            return _products
                .Where(p => p.IsValid)
                .Include(p => p.MaterialForProduct)
                .Include("MaterialForProduct.Material")
                .ToList();
        }

        // _context.SaveChanges() performs a transaction. If at least one statement fails, all statements are not executed.
        public Product Insert(Product product)
        {
            var prod = product;
            try
            {
                product.IsValid = true;
                product.LastModified = DateTime.Now;

                using var transaction = _context.Database.BeginTransaction();

                ProductHistory productHistory = new ProductHistory(product);
                var p = _products.Add(product);
                _context.SaveChanges();

                prod = p.Entity;
                productHistory.ProductId = prod.Id;
                _productHistories.Add(productHistory);
                _context.SaveChanges();

                // Commit transaction if all commands succeed, transaction will auto-rollback
                // when disposed if either commands fails
                transaction.Commit();
            }catch(Exception)
            {
                throw new Exception("Error creating product");
            }

            return prod;
        }

        // _context.SaveChanges() performs a transaction. If at least one statement fails, all statements are not executed.
        public void Update(Product product)
        {
            product.LastModified = DateTime.Now;

            var history = _productHistories
                .Where(e => e.ProductId == product.Id && e.Version == product.CurrentVersion && e.isValid)
                .FirstOrDefault();

            product.CurrentVersion += 1;

            if (history != null)
            {
                history.isValid = false;
                history.ModificationDate = DateTime.Now;
                _productHistories.Update(history);
            }
            else
            {
                throw new Exception("No history existed for the modified product!");
            }

            var newHistory = new ProductHistory(product);
            _productHistories.Add(newHistory);

            _products.Update(product);
            _context.SaveChanges();
        }
    }
}
