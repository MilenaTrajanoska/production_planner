using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductionPlanner.Domain.Models;
using ProductionPlanner.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductionPlanner.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService _orderService)
        {
            orderService = _orderService;

        }

        //discuss usage
        public IActionResult Index()
        {
            var orders = orderService.GetAllOrders();
            //DA SE DOPRAVI ORDER REPOSITORY!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            return View(orders);
            
        }

        // GET: Order/Create
        public IActionResult Create()
        {
            Order order = new Order();
            return View(order);
        }

        [HttpPost]
        public IActionResult Create(Order order)
        {
            if (ModelState.IsValid)
            {
                orderService.CreateNewOrder(order);
            }
            return View();
        }

        public IActionResult ImportOrdersFromSpreadsheet(IFormFile file)
        {
            List<string> errors = orderService.ImportOrdersFromExcel(file);
            if (errors.Count > 0)
            {
                ViewBag.Messages = errors;
            } else
            {
               ViewBag.Messages = new List<string>(){ "Successfully uploaded orders."};
            }

            return View("Create", new Order());
            
        }
    }
}
