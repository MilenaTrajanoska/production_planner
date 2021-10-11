using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductionPlanner.Domain;
using ProductionPlanner.Service.Interface;
using ProductionPlanner.Domain.ViewModels;
using ProductionPlanner.Domain.Models;
using System.Collections.Generic;

namespace ProductionPlanner.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICompanyService _companyService;
        private readonly IInMemoryCacheService _inMemoryCacheService;

        public HomeController(
            ILogger<HomeController> logger, 
            ICompanyService companyService,
            IInMemoryCacheService inMemoryCacheService
            )
        {
            _logger = logger;
            _companyService = companyService;
            _inMemoryCacheService = inMemoryCacheService;
        }

        public IActionResult Index()
        {

            var company = _companyService.GetCompany();
            ViewBag.Company = company;

            GlobalPerformanceViewModel performance = new GlobalPerformanceViewModel();

            performance = _inMemoryCacheService.GetPerformanceViewModel(performance);

            ViewBag.PerformanceFeatures = performance;
            return View();
        }

        [HttpGet]
        public IActionResult Edit()
        {
            var company = _companyService.GetCompany();
            return View(company);
        }

        [HttpPost]
        public IActionResult Edit(int NumberOfWS, double WSCapacity, double InterOpTime, double TransportantionAndStorageTime)
        {
            var company = Company.getInstance();
            if (ModelState.IsValid)
            {
                try
                {
                    
                    company.NumberOfWS = NumberOfWS;
                    company.WSCapacity = WSCapacity;
                    company.InterOpTime = InterOpTime;
                    company.TransportantionAndStorageTime = TransportantionAndStorageTime;
                      
                    _companyService.UpdateCompany(company);
                    ViewBag.Message = "Successfully updated the company";
                }catch
               {
                    ViewBag.Errors = new List<string>() { "Could not update the company values.\nPlease try again later"};
               }
                
            }
            return View(company);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
