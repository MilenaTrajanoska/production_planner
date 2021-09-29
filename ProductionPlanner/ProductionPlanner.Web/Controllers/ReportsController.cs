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
            List<DateTime> datesWeekly = this.LastWeekDates();

            //List<double> input = throughputCalculationService.calculateInputSeries(datesWeekly);
            //List<double> output = throughputCalculationService.calculateOutputSeries(datesWeekly);
            //List<double> wip = throughputCalculationService.calculateWIPSeries(datesWeekly);
            //double tCapacity = throughputCalculationService.getCapacity(datesWeekly);
            //ThroughputDiagram throughputDiagramModel = new ThroughputDiagram(input, output, wip, tCapacity);

            //List<double> wcdClasses = workContentDistributionCalculationService.getXAxisValues(datesWeekly);
            //List<double> wcdRel = workContentDistributionCalculationService.getYAxisValues(datesWeekly);
            //WorkContentDistributionDiagramModel workContentDistributionDiagramModel = new WorkContentDistributionDiagramModel(wcdClasses, wcdRel);

            //List<double> outputRate = logisticOperatingCurveCalculationService.getOutputRateXAxisValues(datesWeekly);
            //List<double> throughputTime = logisticOperatingCurveCalculationService.getThroughputTimeXAxisValues(datesWeekly);
            //double range = logisticOperatingCurveCalculationService.getOPRangeXAxisValues(datesWeekly);
            //List<double> locCapacity = logisticOperatingCurveCalculationService.getCapacityXAxisValues(datesWeekly);
            //List<double> opOperatingPoin = logisticOperatingCurveCalculationService.getOPOperatingPointXAxisValues(datesWeekly);
            //double opRange = logisticOperatingCurveCalculationService.getOPRangeXAxisValues(datesWeekly);
            //double opThroughputTime = logisticOperatingCurveCalculationService.getOPThroughputTimeXAxisValues(datesWeekly);
            //LodisticOperatingCurvesDiagramModel lodisticOperatingCurvesDiagramModel = new LodisticOperatingCurvesDiagramModel(outputRate, throughputTime, range, locCapacity, opOperatingPoin, opRange, opThroughputTime);

            //List<double> ttdClasses = throughputTimeDistributionCalculationService.getXAxisValues(datesWeekly);
            //List<double> ttdRel = throughputTimeDistributionCalculationService.getYAxisValues(datesWeekly);
            //ThroughputTimeDistributionDiagramModel throughputTimeDistributionDiagramModel = new ThroughputTimeDistributionDiagramModel(ttdClasses, ttdRel);

            //List<double> scheduleReliability = scheduleReliabilityCalculationService.getXAxisScheduleReliability(datesWeekly);
            //List<double> meanWIP = scheduleReliabilityCalculationService.getXAxisMeanWIP(datesWeekly);
            //ScheduleReliabilityOperatingCurveDiagramModel scheduleReliabilityOperatingCurveDiagramModel = new ScheduleReliabilityOperatingCurveDiagramModel(scheduleReliability, meanWIP);

            //Diagram diagram = new Diagram(throughputDiagramModel, workContentDistributionDiagramModel, lodisticOperatingCurvesDiagramModel, throughputTimeDistributionDiagramModel, scheduleReliabilityOperatingCurveDiagramModel);

            //return View(diagram);
            return View();
        }

        public IActionResult MonthReports()
        {
            List<DateTime> datesMonthly = this.LastMonthDates();
            return View();
        }

        public IActionResult YearReports()
        {
            List<DateTime> datesYearly = this.YearDates(2021);

            return View();
        }

        private List<DateTime> LastWeekDates()
        {
            var lastSunday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            var lastMonday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek - 6);
            return new List<DateTime> { lastMonday, lastSunday };
        }

        private List<DateTime> LastMonthDates()
        {
            var dateNow = DateTime.Now;
            var dateLastMonth = DateTime.Now.AddMonths(-1);
            var daysInMonth = DateTime.DaysInMonth(dateNow.Year, dateNow.Month);
            var daysInLastMonth = DateTime.DaysInMonth(dateLastMonth.Year, dateLastMonth.Month);
            var firstOfMonth = DateTime.Now.AddDays(-(int)dateNow.Day - daysInLastMonth + 1);
            var lastOfMonth = DateTime.Now.AddDays(-(int)daysInMonth + 1);
            return new List<DateTime> { firstOfMonth, lastOfMonth };
        }

        private List<DateTime> YearDates(int year)
        {
            var yearsBack = DateTime.Now.Year - year;
            var firstOfYear = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfYear + 1).AddYears(-yearsBack);
            var lastOfYear = DateTime.Today.AddDays(-1).AddYears(-yearsBack);
            return new List<DateTime> { firstOfYear, lastOfYear };
        }
    }
}
