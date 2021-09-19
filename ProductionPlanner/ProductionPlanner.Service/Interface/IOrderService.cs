using Microsoft.AspNetCore.Http;
using ProductionPlanner.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductionPlanner.Service.Interface
{
    public interface IOrderService
    {
        void CreateNewOrder(Order order);
        List<Order> GetAllOrders();
        Order GetOrder(long id);
        void UpdateExistingOrder(Order order);
        void DeleteOrder(long id);
        public List<string> ImportOrdersFromExcel(IFormFile file);
    }
}
