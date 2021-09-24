using Microsoft.AspNetCore.Mvc;
using ProductionPlanner.Domain.ViewModels;
using ProductionPlanner.Service.Implementation;
using ProductionPlanner.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductionPlanner.Web.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IThroughputCalculationService throughputCalculationService;
        private readonly ILogisticOperatingCurveCalculationService logisticOperatingCurveCalculationService;
        private readonly IWorkContentDistributionCalculationService workContentDistributionCalculationService;
        private readonly IThroughputTimeDistributionCalculationService throughputTimeDistributionCalculationService;
        private readonly IScheduleReliabilityCalculationService scheduleReliabilityCalculationService;

        //private readonly ThroughputCalculationService throughputCalculationService;
        //private readonly LogisticOperatingCurveCalculationService logisticOperatingCurveCalculationService;
        //private readonly WorkContentDistributionCalculationService workContentDistributionCalculationService;
        //private readonly ThroughputTimeDistributionCalculationService throughputTimeDistributionCalculationService;
        //private readonly ScheduleReliabilityCalculationService scheduleReliabilityCalculationService;

        public ReportsController(
            IThroughputCalculationService _throughputCalculationService,
            ILogisticOperatingCurveCalculationService _logisticOperatingCurveCalculationService,
            IWorkContentDistributionCalculationService _workContentDistributionCalculationService,
            IThroughputTimeDistributionCalculationService _throughputTimeDistributionCalculationService,
            IScheduleReliabilityCalculationService _scheduleReliabilityCalculationService
            )
        {
            throughputCalculationService = _throughputCalculationService;
            logisticOperatingCurveCalculationService = _logisticOperatingCurveCalculationService;
            workContentDistributionCalculationService = _workContentDistributionCalculationService;
            throughputTimeDistributionCalculationService = _throughputTimeDistributionCalculationService;
            scheduleReliabilityCalculationService = _scheduleReliabilityCalculationService;
        }

        public IActionResult Index()
        {
            DateTime minDate = DateTime.Today.AddDays((int)-7);
            List<DateTime> datesWeekly = throughputCalculationService.calculateThroughputDiagramXAxis(minDate);

            //ViewBag.date = d.ToString();
           
            List<double> input = throughputCalculationService.calculateInputSeries(datesWeekly);
            List<double> output = throughputCalculationService.calculateOutputSeries(datesWeekly);
            List<double> wip = throughputCalculationService.calculateWIPSeries(datesWeekly);
            double capacity = throughputCalculationService.getCapacity(minDate, DateTime.Today);

            ThroughputDiagram throughputDiagramModel = new ThroughputDiagram(input,output,wip,capacity);

            return View(throughputDiagramModel);
        }

        public IActionResult MonthReports()
        {
            return View();
        }

        public IActionResult YearReports()
        {
            return View();
        }

    }
}
