using ProductionPlanner.Domain.Models;
using ProductionPlanner.Repository.Implementation;
using ProductionPlanner.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProductionPlanner.Service.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> orderRepository;

        public OrderService(IRepository<Order> _orderRepository)
        {
            this.orderRepository = _orderRepository;
        }

        public void CreateNewOrder(Order order)
        {
            this.orderRepository.Insert(order);
        }

        public List<Order> GetAllOrders()
        {
            return this.orderRepository.GetAll().ToList();
        }

        public Order GetOrder(long id)
        {
            return this.orderRepository.Get(id);
        }

        public void UpdateExistingOrder(Order order)
        {
            this.orderRepository.Update(order);
        }

        public void DeleteOrder(long id)
        {
            var Order = this.GetOrder(id);
            this.orderRepository.Delete(Order);
        }
    }
}
