using Microsoft.AspNetCore.Mvc;
using ProductionPlanner.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductionPlanner.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;

        public ProductController(IProductService _productService)
        {
            productService = _productService;
        }
        public IActionResult Index()
        {
            var products = productService.GetAllProducts();
            return View(products);
        }

        //to be discussed
        public IActionResult Create()
        {
            return View();
        }
    }
}
