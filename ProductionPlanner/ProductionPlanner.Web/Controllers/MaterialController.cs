using Microsoft.AspNetCore.Mvc;
using ProductionPlanner.Domain.Models;
using ProductionPlanner.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            }
            return View(); //da se dopise
        }

    }
}