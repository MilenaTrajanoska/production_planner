using ProductionPlanner.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductionPlanner.Service.Implementation
{
    public class ThroughputCalculationService : IThroughputCalculationService
    {
        private readonly ICalculationService _calculationService;
        
        public ThroughputCalculationService(ICalculationService calculationService)
        {
            _calculationService = calculationService;
        }

        private DateTime getMinDate(DateTime date)
        {
            return date != null ? date : _calculationService.getMinStartDate();
        }
        private DateTime getMaxDate(DateTime date)
        {
            return date != null ? date : _calculationService.getMaxEndDate();
        }

        public List<DateTime> calculateThroughputDiagramXAxis(DateTime from, DateTime to)
        {
            DateTime startMin = getMinDate(from);
            DateTime endDate = getMaxDate(to);

            List<DateTime> result = new List<DateTime>();
            
            while(startMin.Date.CompareTo(endDate.Date) <= 0)
            {
                result.Add(DateTime.Parse(startMin.ToString()));
                startMin = startMin.AddDays(1);
            }
            return result;
        }

        public List<double> calculateOutputSeries(DateTime minDate, DateTime maxDate)
        {
            var dates = calculateThroughputDiagramXAxis(minDate, maxDate);
            var result = new List<double>();

            dates.ForEach(d => result.Add(_calculationService.calculateOutputForDate(minDate, d)));
            return result;
        }

        public List<double> calculateInputSeries(DateTime minDate, DateTime maxDate)
        {
            var dates = calculateThroughputDiagramXAxis(minDate, maxDate);
            var result = new List<double>();

            dates.ForEach(d => result.Add(_calculationService.calculateInputForDate(minDate, d)));
            return result;
        }

        public List<double> calculateWIPSeries(DateTime minDate, DateTime maxDate)
        {
            var dates = calculateThroughputDiagramXAxis(minDate, maxDate);
            var result = new List<double>();

            dates.ForEach(d => result.Add(_calculationService.getWIPForDate(minDate, d)));
            return result;
        }
        public double getCapacity(DateTime minDate, DateTime maxDate)
        {
            var daysBetween = maxDate.Date.Subtract(minDate.Date).TotalDays;
            var WS = _calculationService.getNumberOfWorkStations();
            var capacity = _calculationService.getWorkstationCapacity();

            return WS * capacity * daysBetween;
        }
       
    }
}
