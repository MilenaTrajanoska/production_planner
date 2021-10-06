using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        private readonly IProductService productService;
        

        public OrderController(IOrderService _orderService, IRepository<ProductHistory> _productHistoryRepository, IProductService _productService)
        {
            orderService = _orderService;
            productHistoryRepository = _productHistoryRepository;
            productService =_productService;
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
                    var oldOrder = orderService.GetAllOrders().Where(o => o.OrderName == order.OrderName).FirstOrDefault();
                    if (oldOrder == null)
                    {
                        order.ProductId = product.Id;
                        order.OrderedProduct = product;
                        orderService.CreateNewOrder(order);
                        ViewBag.Success = "Successfully created order";
                    }
                    else
                    {
                        ViewBag.Error = "An order with the same number already exists.";
                    }
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
                List<string> errors = new List<string>();
                try
                {
                    errors = orderService.ImportOrdersFromExcel(file);
                }catch (Exception e){
                    errors = new List<string>() { "Could not open file." };
                }
                
                if (errors.Count > 0)
                {
                    ViewBag.Error = errors;
                }
                else
                {
                    ViewBag.Messages = new List<string>() { "Successfully uploaded orders." };
                }
            }
            else
            {
                ViewBag.Error = new List<string>() { "Please upload an excel file." };
            }

            return View("Create", new Order());
            
        }

        [HttpGet]
        public IActionResult Edit(long id)
        {
            ViewBag.Products = productService.GetAllProducts().Select(o => new SelectListItem
            {
                Value = o.ProductName,
                Text = o.ProductName
            }).ToList();

            

            var order = orderService.GetOrder(id);
            if(order != null)
            {
                return View(order);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                var product = productHistoryRepository
                    .getEntities()
                    .Include(p=>p.ReferencedProduct)
                    .Where(ph => ph.isValid && ph.ReferencedProduct.ProductName == order.OrderedProduct.ProductName)
                    .FirstOrDefault();

                if (product == null)
                {
                    return NotFound();
                }
                order.OrderedProduct = product;
                order.ProductId = product.Id;
                orderService.UpdateExistingOrder(order);
                ViewBag.Message = "Successfully updated the order";
            }

            return View();
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            var order = orderService.GetOrder(id);
            if (order == null)
            {
                ViewBag.Errors = new List<string>() { "Could not delete a product that does not exist." };
            }
            else
            {
                orderService.DeleteOrder(id);
                ViewBag.Message = "Successfully deleted product.";
            }
            return View("Index", orderService.GetAllOrders());
        }

        [HttpGet]
        public IActionResult Details(long id)
        {
            var order = orderService.GetOrder(id);

            ViewBag.Products = productService.GetAllProducts().Select(o => new SelectListItem
            {
                Value = o.ProductName,
                Text = o.ProductName
            }).ToList();

            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
    }
}
