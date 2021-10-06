using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductionPlanner.Domain.Models;
using ProductionPlanner.Domain;
using ProductionPlanner.Service.Interface;
using ProductionPlanner.Domain.ViewModels;

namespace ProductionPlanner.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICompanyService _companyService;
        private readonly ICalculationService _calculationService;
        private readonly IOrderService _orderService;
        private readonly IScheduleReliabilityCalculationService _scheduleReliabilityCalculationService;

        public HomeController(
            ILogger<HomeController> logger, 
            ICompanyService companyService,
            ICalculationService calculationService, 
            IOrderService orderService,
            IScheduleReliabilityCalculationService scheduleReliabilityCalculationService
            )
        {
            _logger = logger;
            _companyService = companyService;
            _calculationService = calculationService;
            _orderService = orderService;
            _scheduleReliabilityCalculationService = scheduleReliabilityCalculationService;
        }

        public IActionResult Index()
        {
            var year = DateTime.Now.Year - 1;
            DateTime minDate = new DateTime(year, 1, 1);

            var company = _companyService.GetCompany();
            ViewBag.Company = company;

            var performance = new GlobalPerformanceViewModel();
            var num_orders = _orderService.GetAllOrders()
                .Where(o => o.StartDate.Date.CompareTo(minDate.Date) >= 0)
                .ToList()
                .Count;
            performance.CompletedOrders = num_orders;
            if (num_orders != 0)
            {
                performance.AverageWorkContent = _calculationService.calculateAverageWorkContent(minDate, DateTime.Now);
                performance.StdWorkContent = _calculationService.calculateSdWorkContent(minDate, DateTime.Now);
                performance.AverageThroughputTime = _calculationService.getThroughputTimes(minDate, DateTime.Now).Average();
                performance.AverageRange = _calculationService.getListOfWIP(minDate, DateTime.Now).Average() / _calculationService.calculateAverageRout(minDate, DateTime.Now);
                performance.AverageOutputRate = _calculationService.calculateAverageRout(minDate, DateTime.Now);
                performance.MaximumOutputRate = _calculationService.calculateRoutMax(minDate, DateTime.Now) * company.NumberOfWS;
                performance.Capacity = company.NumberOfWS * company.WSCapacity;
                performance.AverageWIPLevel = _calculationService.getListOfWIP(minDate, DateTime.Now).Average();
                performance.WIPIMin = _calculationService.calculateWipiMin(minDate, DateTime.Now);
                performance.WIPRel = _calculationService.calculateWIPRel(minDate, DateTime.Now);
                performance.AverageUtilizationRate = _calculationService.calculateAverageUtilizationGlobal();
                performance.ScheduleReliability = _scheduleReliabilityCalculationService.getYAxisScheduleReliability(minDate, DateTime.Now).Max();
            }

            ViewBag.PerformanceFeatures = performance;
            return View();
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
