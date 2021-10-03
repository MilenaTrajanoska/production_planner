using Microsoft.AspNetCore.Mvc;
using ProductionPlanner.Domain.Models;
using ProductionPlanner.Service.Interface;
using System.Collections.Generic;

namespace ProductionPlanner.Web.Controllers
{
    public class MaterialController : Controller
    {
        private readonly IMaterialService materialService;

        public MaterialController(IMaterialService _materialService)
        {
            materialService = _materialService;
        }


        public IActionResult Index()
        {
            var material = new Material();
            return View(material);
        }

        [HttpPost]
        public IActionResult Index(Material material)
        {
            if (ModelState.IsValid)
            {
                materialService.CreateNewMaterial(material);
                ViewBag.SuccessMaterial = "Successfully added new material";

            }
            else
            {
                ViewBag.ErrorMaterial = "Could not add new material";
            }
            return View(material); 
        } 
        
        public IActionResult AllMaterials()
        {
            var materials = materialService.GetAllMaterials();
            return View(materials);
        }


        [HttpGet]
        public IActionResult Edit(long id)
        {
            var material = materialService.GetMaterial(id);
            if (material == null)
            {
                ViewBag.Errors = new List<string>() { "Could not find the requested material." };
                return View("AllMaterials", materialService.GetAllMaterials());
            }
            else
            {
                return View(material);
            }
    }

        [HttpPost]
        public IActionResult Edit(Material material)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = new List<string>() { "There were errors trying to update the material." };
            }
            else
            {
                materialService.UpdateExistingMaterial(material);
                ViewBag.Message = "Successfully updated the material.";
            }
            return View(material);
        }

    }
}