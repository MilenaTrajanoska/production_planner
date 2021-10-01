using ProductionPlanner.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductionPlanner.Repository.Interface
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetAll();
        Order Get(long? id);
        Order Insert(Order entity);
        void Update(Order entity);
        void Delete(Order entity);
    }
}
