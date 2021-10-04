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
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Order> orders;
        
        public OrderRepository(ApplicationDbContext context)
        {
            this.context = context;
            orders = context.Set<Order>();
        }

        public void Delete(Order entity)
        {
            entity.IsValid = false;
            orders.Update(entity);
            context.SaveChanges();
        }

        public Order Get(long? id)
        {
            return orders
                .Include(o => o.OrderedProduct)
                .Include("OrderedProduct.ReferencedProduct")
                .Include("OrderedProduct.ReferencedProduct.MaterialForProduct")
                .Include("OrderedProduct.ReferencedProduct.MaterialForProduct.Material")
                .SingleOrDefault(o => o.Id == id && o.IsValid);
        }

        public IEnumerable<Order> GetAll()
        {
            return orders
                 .Include(o => o.OrderedProduct)
                 .Include("OrderedProduct.ReferencedProduct")
                 .Include("OrderedProduct.ReferencedProduct.MaterialForProduct")
                 .Include("OrderedProduct.ReferencedProduct.MaterialForProduct.Material")
                 .Where(o => o.IsValid)
                 .ToList();
        }

        public Order Insert(Order entity)
        {
            var order = orders.Add(entity);
            context.SaveChanges();
            return order.Entity;
        }

        public void Update(Order entity)
        {
            orders.Update(entity);
            context.SaveChanges();
        }
    }
}
