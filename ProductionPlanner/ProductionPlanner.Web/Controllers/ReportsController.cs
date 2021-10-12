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
            DateTime minDate = datesWeekly.FirstOrDefault();
            DateTime maxDate = datesWeekly.LastOrDefault();
            Diagram diagram = this.GetDiagram(minDate, maxDate);
            return View();
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
            var firstOfMonth = DateTime.Now.AddDays(-(int)dateNow.Day  + 1).AddMonths(-1);
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
            List<double> outputRate = logisticOperatingCurveCalculationService.getOutputRateXAxisValues(minDate, maxDate);
            List<double> throughputTime = logisticOperatingCurveCalculationService.getThroughputTimeXAxisValues(minDate, maxDate);
            double range = logisticOperatingCurveCalculationService.getOPRangeXAxisValues(minDate, maxDate);
            List<double> locCapacity = logisticOperatingCurveCalculationService.getCapacityXAxisValues(minDate, maxDate);
            List<double> opOperatingPoin = logisticOperatingCurveCalculationService.getOPOperatingPointXAxisValues(minDate, maxDate);
            double opRange = logisticOperatingCurveCalculationService.getOPRangeXAxisValues(minDate, maxDate);
            double opThroughputTime = logisticOperatingCurveCalculationService.getOPThroughputTimeXAxisValues(minDate, maxDate);
            return new LodisticOperatingCurvesDiagramModel(outputRate, throughputTime, range, locCapacity, opOperatingPoin, opRange, opThroughputTime);
        }

        private ThroughputTimeDistributionDiagramModel GetThroughputTimeDistributionDiagramModel(DateTime minDate, DateTime maxDate)
        {
            List<double> ttdClasses = throughputTimeDistributionCalculationService.getXAxisValues(minDate, maxDate); ;
            List<double> ttdRel = throughputTimeDistributionCalculationService.getYAxisValues(minDate, maxDate);
            return new ThroughputTimeDistributionDiagramModel(ttdClasses, ttdRel);
        }

        private ScheduleReliabilityOperatingCurveDiagramModel GetScheduleReliabilityOperatingCurveDiagramModel(DateTime minDate, DateTime maxDate)
        {
            List<double> scheduleReliability = scheduleReliabilityCalculationService.getXAxisScheduleReliability(minDate, maxDate);
            List<double> meanWIP = scheduleReliabilityCalculationService.getXAxisMeanWIP(minDate, maxDate);
            return new ScheduleReliabilityOperatingCurveDiagramModel(scheduleReliability, meanWIP);
        }

    }
}
