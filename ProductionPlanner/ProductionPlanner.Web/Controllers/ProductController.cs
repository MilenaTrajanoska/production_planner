using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductionPlanner.Domain.Models;
using ProductionPlanner.Domain.ViewModels;
using ProductionPlanner.Repository.Implementation;
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
        private readonly IRepository<MaterialForProduct> repositoryMaterial;

        public ProductController(IProductService _productService, IMaterialService _materialService, IRepository<MaterialForProduct> _repositoryMaterial)
        {
            productService = _productService;
            materialService = _materialService;
            repositoryMaterial = _repositoryMaterial;

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
                var p = productService.GetAllProducts().Where(p => p.ProductName == product.ProductName).FirstOrDefault();
                if (p != null)
                {
                    ViewBag.Errors = new List<string>() { "A product with the same name already exists." };
                    return View(product);
                }
                var prod = productService.CreateNewProduct(product);
                ViewBag.Message = "Succesfully created the product.";
                return RedirectToAction("AddMaterialToProduct", new { id = prod.Id });   
            }

            ViewBag.Errors = new List<string>() { "Could not create  the product.\nPlease check the values you have entered." };
            
            return View(product);

        }

        [HttpGet]
        public IActionResult Edit(long id)
        {
            ViewBag.Materials = materialService.GetAllMaterials().Select(a =>
                new SelectListItem
                {
                    Value = a.MaterialName,
                    Text = a.MaterialName
                }).ToList();
            var product = productService.GetProduct(id);
            if(product != null)
            {
                return View(product);
            }
            return NotFound();   
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                productService.UpdateExistingProduct(product);
                ViewBag.Message = "Successfully updated product";
            }

            return View(product);

        }

        public IActionResult AddMaterialToProduct(long id)
        {
            ViewBag.Materials = materialService.GetAllMaterials().Select(a =>
                new SelectListItem
                {
                    Value = a.MaterialName,
                    Text = a.MaterialName
                }).ToList();
            var product = productService.GetProduct(id);
            if (product == null)
            {
                ViewBag.MaterialError = "The product does not exist";
                return View("Index", productService.GetAllProducts());
            }

            return View(new AddMaterialToProduct()
            {
                ProductId = product.Id, SelectedProduct = product
            }) ;
        }

        [HttpPost]
        public IActionResult AddMaterialToProduct([Bind("ProductId", "MaterialName", "Quantity")] AddMaterialToProduct product)
        {
            ViewBag.Materials = materialService.GetAllMaterials().Select(a =>
                 new SelectListItem
                 {
                     Value = a.MaterialName,
                     Text = a.MaterialName
                 }).ToList();

            if (ModelState.IsValid)
            {
                var product1 = productService.GetProduct(product.ProductId);
                var material1 = materialService.GetAllMaterials().Where(m => m.MaterialName == product.MaterialName).FirstOrDefault();
                if(product1!=null && material1 != null)
                {
                    product.SelectedProduct = product1;
                    var relation = new MaterialForProduct()
                    {
                        Material = material1,
                        MaterialId = material1.Id,
                        Product = product1,
                        ProductId = product1.Id,
                        Quantity = product.Quantity

                    };
                    product1.MaterialForProduct.Add(relation);
                    //productService.UpdateExistingProduct(product1);
                    repositoryMaterial.Insert(relation);
                    product.SelectedProduct = product1;
                    ViewBag.SuccessMaterialAdded = "Successfuly added the material";
                }
                else
                {
                    ViewBag.ErrorMaterialAdded = "Could not add the material";
                }
            }
            return View(product);

        }

        [HttpPost]
        public IActionResult DeleteMaterialFromProduct(long productId, long materialId)
        {
            var material = materialService.GetMaterial(materialId);
            var product = productService.GetProduct(productId);
            if (material == null || product == null)
            {
                ViewBag.Errors = new List<string>() { "Could not remove material from product." };
            } else
            {
                var relation = repositoryMaterial.getEntities().Where(e => e.MaterialId == materialId && e.ProductId == productId).FirstOrDefault();
                repositoryMaterial.Delete(relation);
                ViewBag.Message = "Successfully removed material from product";
            }

            return View("Details", product);
        }

        [HttpGet]
        public IActionResult Details(long id)
        {
            var product = productService.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            var product = productService.GetProduct(id);
            if (product == null)
            {
                ViewBag.Errors = new List<string>() { "Could not delete a product that does not exist." };
            } 
            else
            {
                productService.DeleteProduct(id);
                ViewBag.Message = "Successfully deleted product.";
            }
            return View("Index", productService.GetAllProducts());
        }

    }
}
