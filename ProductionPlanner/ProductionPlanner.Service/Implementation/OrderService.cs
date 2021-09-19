﻿using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ProductionPlanner.Domain.Exceptions;
using ProductionPlanner.Domain.Models;
using ProductionPlanner.Repository.Implementation;
using ProductionPlanner.Service.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace ProductionPlanner.Service.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> orderRepository;
        private readonly IRepository<ProductHistory> productHistoryRepository;

        public OrderService(IRepository<Order> _orderRepository, IRepository<ProductHistory> _productHistoryRepository)
        {
            this.orderRepository = _orderRepository;
            productHistoryRepository = _productHistoryRepository;
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

        public List<string> ImportOrdersFromExcel(IFormFile file)
        {
            string filePath = $"{Directory.GetCurrentDirectory()}\\files\\{file.FileName}";
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using (FileStream fileStream = File.Create(filePath))
            {
                file.CopyTo(fileStream);

                fileStream.Flush();
            }

            Order order;
            var errorMessages = new List<string>();

            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    reader.Read();
                    while (reader.Read())
                    {
                        try
                        {
                            var productName = reader.GetValue(1).ToString();
                            var product = productHistoryRepository.GetAll()
                                .Where(p => p.ProductName == productName && p.isValid)
                                .FirstOrDefault();
                            if (product == null)
                            {
                                throw new ProductNotFoundException(String.Format("Product with name {0} could not be found.", productName));
                            }
                            order = new Order
                            {
                                OrderName = reader.GetValue(0).ToString(),
                                ProductId = product.Id,
                                ProductVersion = product.Version,
                                OrderedProduct = product,
                                Quantity = Convert.ToInt32(reader.GetValue(2).ToString()),
                                StartDate = DateTime.Parse(reader.GetValue(3).ToString()),
                                EndDate = DateTime.Parse(reader.GetValue(4).ToString())
                            };
                            CreateNewOrder(order);
                        }catch(Exception e)
                        {
                            errorMessages.Add(e.Message);
                        }
                    }
                }
            }

            return errorMessages;
        }
    }
}
