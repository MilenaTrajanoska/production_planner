using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductionPlanner.Domain.Models;
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
        private readonly IMaterialService materialService;

        public ProductController(IProductService _productService, IMaterialService _materialService)
        {
            productService = _productService;
            materialService = _materialService;

        }
        public IActionResult Index()
        {
            var products = productService.GetAllProducts();
            return View(products);
        }

        //to be discussed
        public IActionResult Create()
        {
            ViewBag.Materials = materialService.GetAllMaterials().Select(a => 
                new SelectListItem
                {
                    Value = a.MaterialName,
                    Text = a.MaterialName
                } ).ToList();
            var product = new Product();
            return View(product);
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                productService.CreateNewProduct(product);
            }
            return View();

        }
    }
}
