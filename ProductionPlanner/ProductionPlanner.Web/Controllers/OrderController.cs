using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductionPlanner.Domain.Models;
using ProductionPlanner.Repository.Implementation;
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
        private readonly IRepository<ProductHistory> productHistoryRepository;

        public OrderController(IOrderService _orderService, IRepository<ProductHistory> _productHistoryRepository)
        {
            orderService = _orderService;
            productHistoryRepository = _productHistoryRepository;
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
            var allPorducts = productHistoryRepository
                .getEntities()
                .Where(e => e.isValid)
                .Select(e => e.ProductName)
                .ToHashSet();
            ViewBag.ProductNames = allPorducts.Select(a =>
                new SelectListItem
                {
                    Value = a,
                    Text = a
                }).ToList();

            Order order = new Order();
            order.OrderedProduct = new ProductHistory();
            return View(order);
        }

        [HttpPost]
        public IActionResult Create([Bind("OrderedProduct", "Quantity", "OrderName", "StartDate","EndDate", "ProductId")] Order order)
        {
            if (ModelState.IsValid)
            {
                var product = productHistoryRepository.GetAll().Where(p => p.ProductName == order.OrderedProduct.ProductName).FirstOrDefault();
                if (product != null)
                {
                    order.ProductId = product.Id;
                    order.OrderedProduct = product;
                    orderService.CreateNewOrder(order);
                    ViewBag.Success = "Successfully created order";
                    
                }
                else
                {
                    ViewBag.Error = "This product does not exist.";
                }
            }
            return View();
        }

        public IActionResult ImportOrdersFromSpreadsheet(IFormFile file)
        {

            if (file != null)
            {
                List<string> errors = orderService.ImportOrdersFromExcel(file);
                if (errors.Count > 0)
                {
                    ViewBag.Messages = errors;
                }
                else
                {
                    ViewBag.Messages = new List<string>() { "Successfully uploaded orders." };
                }
            }
            else
            {
                ViewBag.Messages = new List<string>() { "Please upload an excel file." };
            }

            return View("Create", new Order());
            
        }
    }
}
