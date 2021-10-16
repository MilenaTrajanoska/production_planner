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
        private readonly IOrderService orderService;
        private readonly IInMemoryCacheService inMemoryCacheService;

        public ReportsController(
            IOrderService orderService,
            IInMemoryCacheService inMemoryCacheService,
            IThroughputCalculationService _throughputCalculationService,
            ILogisticOperatingCurveCalculationService _logisticOperatingCurveCalculationService,
            IWorkContentDistributionCalculationService _workContentDistributionCalculationService,
            IThroughputTimeDistributionCalculationService _throughputTimeDistributionCalculationService,
            IScheduleReliabilityCalculationService _scheduleReliabilityCalculationService
            )
        {
            this.orderService = orderService;
            this.inMemoryCacheService = inMemoryCacheService;
            throughputCalculationService = _throughputCalculationService;
            logisticOperatingCurveCalculationService = _logisticOperatingCurveCalculationService;
            workContentDistributionCalculationService = _workContentDistributionCalculationService;
            throughputTimeDistributionCalculationService = _throughputTimeDistributionCalculationService;
            scheduleReliabilityCalculationService = _scheduleReliabilityCalculationService;
        }

        public IActionResult Index()
        {
            
            List<DateTime> datesWeekly = this.LastWeekDates();
            DateTime minDate = this.orderService.GetAllOrders().Select(order => order.StartDate).Min();
            DateTime maxDate = this.orderService.GetAllOrders().Select(order => order.StartDate).Max();
            Diagram diagram = this.GetDiagram(minDate, maxDate);
            return View(diagram);
            //GetDiagram
        }

        public IActionResult MonthReports()
        {
            List<DateTime> datesMonthly = this.LastMonthDates();
            DateTime minDate = datesMonthly.FirstOrDefault();
            DateTime maxDate = datesMonthly.LastOrDefault();
            //   Diagram diagram = this.GetDiagram(minDate, maxDate);
            return View();
        }

        public IActionResult YearReports()
        {
            List<DateTime> datesYearly = this.YearDates(2021);
            DateTime minDate = datesYearly.FirstOrDefault();
            DateTime maxDate = datesYearly.LastOrDefault();
            //    Diagram diagram = this.GetDiagram(minDate, maxDate);
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
            //var daysInMonth = DateTime.DaysInMonth(dateNow.Year, dateNow.Month);
            //var daysInLastMonth = DateTime.DaysInMonth(dateLastMonth.Year, dateLastMonth.Month);
            var firstOfMonth = DateTime.Now.AddDays(-(int)dateNow.Day + 1).AddMonths(-1);
            var lastOfMonth = DateTime.Now.AddDays(-(int)dateNow.Day);
            return new List<DateTime> { firstOfMonth, lastOfMonth };
        }

        private List<DateTime> YearDates(int year)
        {
            var yearsBack = DateTime.Now.Year - year;
            var firstOfYear = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfYear + 1).AddYears(-yearsBack);
            var lastOfYear = DateTime.Today.AddDays(-1).AddYears(-yearsBack);
            return new List<DateTime> { firstOfYear, lastOfYear };
        }

        private Diagram GetDiagram(DateTime minDate, DateTime maxDate)
        {
            ThroughputDiagram throughputDiagramModel = this.GetThroughputDiagram(minDate, maxDate);
            WorkContentDistributionDiagramModel workContentDistributionDiagramModel = this.GetWorkContentDistributionDiagramModel(minDate, maxDate);
            LodisticOperatingCurvesDiagramModel lodisticOperatingCurvesDiagramModel = this.GetLodisticOperatingCurvesDiagramModel(minDate, maxDate);
            ThroughputTimeDistributionDiagramModel throughputTimeDistributionDiagramModel = this.GetThroughputTimeDistributionDiagramModel(minDate, maxDate);
            ScheduleReliabilityOperatingCurveDiagramModel scheduleReliabilityOperatingCurveDiagramModel = this.GetScheduleReliabilityOperatingCurveDiagramModel(minDate, maxDate);
            return new Diagram(throughputDiagramModel, workContentDistributionDiagramModel, lodisticOperatingCurvesDiagramModel, throughputTimeDistributionDiagramModel, scheduleReliabilityOperatingCurveDiagramModel);
        }

        private ThroughputDiagram GetThroughputDiagram(DateTime minDate, DateTime maxDate)
        {
            List<double> input = throughputCalculationService.calculateInputSeries(minDate, maxDate);
            List<double> output = throughputCalculationService.calculateOutputSeries(minDate, maxDate);
            List<double> wip = throughputCalculationService.calculateWIPSeries(minDate, maxDate);
            double tCapacity = throughputCalculationService.getCapacity(minDate, maxDate);
            return new ThroughputDiagram(input, output, wip, tCapacity);
        }

        private WorkContentDistributionDiagramModel GetWorkContentDistributionDiagramModel(DateTime minDate, DateTime maxDate)
        {
            List<double> wcdClasses = workContentDistributionCalculationService.getXAxisValues(minDate, maxDate);
            List<double> wcdRel = workContentDistributionCalculationService.getYAxisValues(minDate, maxDate);
            return new WorkContentDistributionDiagramModel(wcdClasses, wcdRel);
        }

        private LodisticOperatingCurvesDiagramModel GetLodisticOperatingCurvesDiagramModel(DateTime minDate, DateTime maxDate)
        {
            List<double> labels = logisticOperatingCurveCalculationService.getOutputRateXAxisValues(minDate, maxDate);
            List<double> outputRate = logisticOperatingCurveCalculationService.getOutputRateYAxisValues(minDate, maxDate);
            List<double> throughputTime = logisticOperatingCurveCalculationService.getThroughputTimeYAxisValues(minDate, maxDate);
            double rangeX = logisticOperatingCurveCalculationService.getOPRangeXAxisValues(minDate, maxDate);
            double rangeY = logisticOperatingCurveCalculationService.getOPRangeYAxisValues(minDate, maxDate);
            List<double> locCapacity = logisticOperatingCurveCalculationService.getCapacityYAxisValues(minDate, maxDate);
            List<double> opOperatingPoin = logisticOperatingCurveCalculationService.getOPOperatingPointYAxisValues(minDate, maxDate);
            double opRangeX = logisticOperatingCurveCalculationService.getOPRangeXAxisValues(minDate, maxDate);
            double opRangeY = logisticOperatingCurveCalculationService.getOPRangeYAxisValues(minDate, maxDate);
            double opThroughputTimeX = logisticOperatingCurveCalculationService.getOPThroughputTimeXAxisValues(minDate, maxDate);
            double opThroughputTimeY = logisticOperatingCurveCalculationService.getOPThroughputTimeYAxisValues(minDate, maxDate);
            return new LodisticOperatingCurvesDiagramModel(labels, outputRate, throughputTime, rangeX, rangeY, locCapacity, opOperatingPoin, opRangeX, opRangeY, opThroughputTimeX, opThroughputTimeY);
        }

        private ThroughputTimeDistributionDiagramModel GetThroughputTimeDistributionDiagramModel(DateTime minDate, DateTime maxDate)
        {
            List<double> ttdClasses = throughputTimeDistributionCalculationService.getXAxisValues(minDate, maxDate); ;
            List<double> ttdRel = throughputTimeDistributionCalculationService.getYAxisValues(minDate, maxDate);
            return new ThroughputTimeDistributionDiagramModel(ttdClasses, ttdRel);
        }

        private ScheduleReliabilityOperatingCurveDiagramModel GetScheduleReliabilityOperatingCurveDiagramModel(DateTime minDate, DateTime maxDate)
        {
            List<double> labels = scheduleReliabilityCalculationService.getXAxisScheduleReliability(minDate, maxDate);
            List<double> scheduleReliability = scheduleReliabilityCalculationService.getYAxisScheduleReliability(minDate, maxDate);
            List<double> meanWIP = scheduleReliabilityCalculationService.getYAxisMeanWIP(minDate, maxDate);
            List<double> meanWIPx = scheduleReliabilityCalculationService.getXAxisMeanWIP(minDate, maxDate);
            return new ScheduleReliabilityOperatingCurveDiagramModel(labels, scheduleReliability, meanWIP);
        }

    }
}
