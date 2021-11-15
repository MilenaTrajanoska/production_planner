using ProductionPlanner.Domain.ViewModels;
using ProductionPlanner.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ProductionPlanner.Service.Implementation
{
    public class DiagramService : IDiagramService
    {
        private readonly IScheduleReliabilityCalculationService _scheduleReliabilityCalculationService;
        private readonly IThroughputCalculationService _throughputCalculationService;
        private readonly IWorkContentDistributionCalculationService _workContentDistributionCalculationService;
        private readonly ILogisticOperatingCurveCalculationService _logisticOperatingCurveCalculationService;
        private readonly IThroughputTimeDistributionCalculationService _throughputTimeDistributionCalculationService;
        private readonly ICompanyService _companyService;
        private readonly IOrderService _orderService;
        private readonly ICalculationService _calculationService;

        public DiagramService(
            IScheduleReliabilityCalculationService scheduleReliabilityCalculationService,
            IThroughputCalculationService throughputCalculationService,
            IWorkContentDistributionCalculationService workContentDistributionCalculationService,
            ILogisticOperatingCurveCalculationService logisticOperatingCurveCalculationService,
            IThroughputTimeDistributionCalculationService throughputTimeDistributionCalculationService,
            ICompanyService companyService,
            IOrderService orderService,
            ICalculationService calculationService
            )
        {
            _scheduleReliabilityCalculationService = scheduleReliabilityCalculationService;
            _throughputCalculationService = throughputCalculationService;
            _workContentDistributionCalculationService = workContentDistributionCalculationService;
            _logisticOperatingCurveCalculationService = logisticOperatingCurveCalculationService;
            _throughputTimeDistributionCalculationService = throughputTimeDistributionCalculationService;
            _companyService = companyService;
            _orderService = orderService;
            _calculationService = calculationService;
        }
        public Diagram GetDiagram(DateTime minDate, DateTime maxDate)
        {
            minDate = this._orderService.GetAllOrders().Select(order => order.StartDate).Min();
            maxDate = this._orderService.GetAllOrders().Select(order => order.StartDate).Max();

            ThroughputDiagram throughputDiagramModel = this.GetThroughputDiagram(minDate, maxDate);
            WorkContentDistributionDiagramModel workContentDistributionDiagramModel = this.GetWorkContentDistributionDiagramModel(minDate, maxDate);
            LodisticOperatingCurvesDiagramModel lodisticOperatingCurvesDiagramModel = this.GetLodisticOperatingCurvesDiagramModel(minDate, maxDate);
            ThroughputTimeDistributionDiagramModel throughputTimeDistributionDiagramModel = this.GetThroughputTimeDistributionDiagramModel(minDate, maxDate);
            ScheduleReliabilityOperatingCurveDiagramModel scheduleReliabilityOperatingCurveDiagramModel = this.GetScheduleReliabilityOperatingCurveDiagramModel(minDate, maxDate);
            return new Diagram(throughputDiagramModel, workContentDistributionDiagramModel, lodisticOperatingCurvesDiagramModel, throughputTimeDistributionDiagramModel, scheduleReliabilityOperatingCurveDiagramModel);
        }

        private ThroughputDiagram GetThroughputDiagram(DateTime minDate, DateTime maxDate)
        {
            List<string> labels = _throughputCalculationService.calculateThroughputDiagramXAxis(minDate, maxDate).Select(t => t.ToString("dd.MM")).ToList();
            List<double> input = _throughputCalculationService.calculateInputSeries(minDate, maxDate);
            List<double> output = _throughputCalculationService.calculateOutputSeries(minDate, maxDate);
            List<double> wip = _throughputCalculationService.calculateWIPSeries(minDate, maxDate);
            double tCapacity = _throughputCalculationService.getCapacity(minDate, maxDate);
            return new ThroughputDiagram(labels, input, output, wip, tCapacity);
        }

        private WorkContentDistributionDiagramModel GetWorkContentDistributionDiagramModel(DateTime minDate, DateTime maxDate)
        {
            List<double> wcdClasses = _workContentDistributionCalculationService.getXAxisValues(minDate, maxDate);
            wcdClasses = wcdClasses.Select(cls => Math.Round((Double)cls, 3)).ToList();
            List<double> wcdRel = _workContentDistributionCalculationService.getYAxisValues(minDate, maxDate);
            return new WorkContentDistributionDiagramModel(wcdClasses, wcdRel);
        }

        private LodisticOperatingCurvesDiagramModel GetLodisticOperatingCurvesDiagramModel(DateTime minDate, DateTime maxDate)
        {
            List<double> labels = _logisticOperatingCurveCalculationService.getOutputRateXAxisValues(minDate, maxDate);
            labels = labels.Select(cls => Math.Round((Double)cls, 3)).ToList();
            List<double> outputRate = _logisticOperatingCurveCalculationService.getOutputRateYAxisValues(minDate, maxDate);
            List<double> throughputTime = _logisticOperatingCurveCalculationService.getThroughputTimeYAxisValues(minDate, maxDate);
            double rangeX = _logisticOperatingCurveCalculationService.getOPRangeXAxisValues(minDate, maxDate);
            double rangeY = _logisticOperatingCurveCalculationService.getOPRangeYAxisValues(minDate, maxDate);
            List<double> locCapacity = _logisticOperatingCurveCalculationService.getCapacityYAxisValues(minDate, maxDate);
            List<double> opOperatingPoin = _logisticOperatingCurveCalculationService.getOPOperatingPointYAxisValues(minDate, maxDate);
            double opRangeX = _logisticOperatingCurveCalculationService.getOPRangeXAxisValues(minDate, maxDate);
            double opRangeY = _logisticOperatingCurveCalculationService.getOPRangeYAxisValues(minDate, maxDate);
            double opThroughputTimeX = _logisticOperatingCurveCalculationService.getOPThroughputTimeXAxisValues(minDate, maxDate);
            double opThroughputTimeY = _logisticOperatingCurveCalculationService.getOPThroughputTimeYAxisValues(minDate, maxDate);
            return new LodisticOperatingCurvesDiagramModel(labels, outputRate, throughputTime, rangeX, rangeY, locCapacity, opOperatingPoin, opRangeX, opRangeY, opThroughputTimeX, opThroughputTimeY);
        }

        private ThroughputTimeDistributionDiagramModel GetThroughputTimeDistributionDiagramModel(DateTime minDate, DateTime maxDate)
        {
            List<double> ttdClasses = _throughputTimeDistributionCalculationService.getXAxisValues(minDate, maxDate);
            ttdClasses = ttdClasses.Select(cls => Math.Round((Double)cls, 3)).ToList();
            List<double> ttdRel = _throughputTimeDistributionCalculationService.getYAxisValues(minDate, maxDate);
            return new ThroughputTimeDistributionDiagramModel(ttdClasses, ttdRel);
        }

        private ScheduleReliabilityOperatingCurveDiagramModel GetScheduleReliabilityOperatingCurveDiagramModel(DateTime minDate, DateTime maxDate)
        {
            List<double> labels = _scheduleReliabilityCalculationService.getXAxisScheduleReliability(minDate, maxDate);
            labels = labels.Select(cls => Math.Round((Double)cls, 3)).ToList();
            List<double> scheduleReliability = _scheduleReliabilityCalculationService.getYAxisScheduleReliability(minDate, maxDate);
            List<double> meanWIP = _scheduleReliabilityCalculationService.getYAxisMeanWIP(minDate, maxDate);
            List<double> meanWIP_X = _scheduleReliabilityCalculationService.getXAxisMeanWIP(minDate, maxDate);
            return new ScheduleReliabilityOperatingCurveDiagramModel(labels, scheduleReliability, meanWIP, meanWIP_X);
        }

        public GlobalPerformanceViewModel setGlobalPerformance(DateTime minDate, DateTime maxDate)
        {
            var year = DateTime.Now.Year - 1;
            minDate = new DateTime(year, 1, 1);

            var company = _companyService.GetCompany();

            var performance = new GlobalPerformanceViewModel();
            var num_orders = _orderService.GetAllOrders()
                .Where(o => o.StartDate.Date.CompareTo(minDate.Date) >= 0)
                .ToList()
                .Count;
            performance.CompletedOrders = num_orders;
            maxDate = DateTime.Now;

            if (num_orders != 0)
            {
                minDate = _orderService.GetAllOrders().Select(o => o.StartDate).Min();
                maxDate = _orderService.GetAllOrders().Select(o => o.EndDate).Max();
                performance.AverageWorkContent = _calculationService.calculateAverageWorkContent(minDate, maxDate);
                performance.StdWorkContent = _calculationService.calculateSdWorkContent(minDate, maxDate);
                performance.AverageThroughputTime = _calculationService.getThroughputTimes(minDate, maxDate).Average();
                performance.AverageRange = _calculationService.getListOfWIP(minDate, maxDate).Average() / _calculationService.calculateAverageRout(minDate, maxDate);
                performance.AverageOutputRate = _calculationService.calculateAverageRout(minDate, maxDate);
                performance.MaximumOutputRate = _calculationService.calculateRoutMax(minDate, maxDate) * company.NumberOfWS;
                performance.Capacity = company.NumberOfWS * company.WSCapacity;
                performance.AverageWIPLevel = _calculationService.getListOfWIP(minDate, maxDate).Average();
                performance.WIPIMin = _calculationService.calculateWipiMin(minDate, maxDate);
                performance.WIPRel = _calculationService.calculateWIPRel(minDate, maxDate);
                performance.AverageUtilizationRate = _calculationService.calculateAverageUtilizationGlobal();
                performance.ScheduleReliability = _scheduleReliabilityCalculationService.getYAxisScheduleReliability(minDate, maxDate).Max();
            }
            return performance;
        }

    }
}
