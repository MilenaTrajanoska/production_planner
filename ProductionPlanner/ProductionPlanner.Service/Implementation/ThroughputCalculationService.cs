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

        public List<DateTime> calculateThroughputDiagramXAxis(DateTime from)
        {
            DateTime startMin;

            if (from != null)
            {
                startMin = from;
            }
            else
            {
                startMin = _calculationService.getMinStartDate();
            }

            DateTime endMax = _calculationService.getMaxEndDate();

            List<DateTime> result = new List<DateTime>();
            
            while(startMin.Date.CompareTo(endMax.Date) <= 0)
            {
                result.Add(DateTime.Parse(startMin.ToString()));
                startMin = startMin.AddDays(1);
            }
            return result;
        }

        public List<double> calculateOutputSeries(List<DateTime> dates)
        {
            var minDate = dates.Min();
            var result = new List<double>();

            dates.ForEach(d => result.Add(_calculationService.calculateOutputForDate(minDate, d)));
            return result;
        }

        public List<double> calculateInputSeries(List<DateTime> dates)
        {
            var minDate = dates.Min();
            var result = new List<double>();

            dates.ForEach(d => result.Add(_calculationService.calculateInputForDate(minDate, d)));
            return result;
        }

        public List<double> calculateWIPSeries(List<DateTime> dates)
        {
            var minDate = dates.Min();
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
