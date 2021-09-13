using Microsoft.AspNetCore.Mvc;
using ProductionPlanner.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductionPlanner.Web.Controllers
{
    public class OrderController : Controller
    {
        //private readonly IOrderService orderService;
        
        //public OrderController(IOrderService _orderService)
        //{
        //    orderService = _orderService;
            

        //}

        //discuss usage
        public IActionResult Index()
        {
            return View();
        }

        // GET: Order/Create
        public IActionResult Create()
        {
            return View();
        }
    }
}
