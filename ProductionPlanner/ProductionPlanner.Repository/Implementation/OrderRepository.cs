using Microsoft.EntityFrameworkCore;
using ProductionPlanner.Domain.Models;
using ProductionPlanner.Repository.Data;
using ProductionPlanner.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductionPlanner.Repository.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Order> orders;
        private const string errorMessage = "Can not update an order that is not created yet";

        public OrderRepository(ApplicationDbContext context)
        {
            this.context = context;
            orders = context.Set<Order>();
        }

        public void Delete(Order entity)
        {
            orders.Remove(entity);
        }

        public Order Get(long? id)
        {
            //orders.Include(o=>o.OrderedProduct.)
            throw new NotImplementedException();
        }

        public IEnumerable<Order> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Insert(Order entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Order entity)
        {
            throw new NotImplementedException();
        }
    }
}
