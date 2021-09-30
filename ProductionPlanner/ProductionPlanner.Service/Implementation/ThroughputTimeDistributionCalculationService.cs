using ProductionPlanner.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductionPlanner.Service.Implementation
{
    public class ThroughputTimeDistributionCalculationService : IThroughputTimeDistributionCalculationService
    {
        private readonly ICalculationService _calculationService;
        public ThroughputTimeDistributionCalculationService(ICalculationService calculationService)
        {
            _calculationService = calculationService;
        }
        public List<double> getXAxisValues(DateTime minDate, DateTime maxDate)
        {

            DateTime startDate;
            if (minDate == null)
            {
                startDate = _calculationService.getMinStartDate();
            }
            else
            {
                startDate = minDate;
            }
            DateTime endDate;
            if (maxDate == null)
            {
                endDate = _calculationService.getMaxEndDate();
            }
            else
            {
                endDate = maxDate;
            }

            var throughputTimes = _calculationService.getThroughputTimes(startDate, endDate);
            var maxTT = throughputTimes.Max();

            var result = new List<double>();
            double prev = 0.0;
            result.Add(prev);
            double current = prev;

            while (current < maxTT)
            {
                current = prev + maxTT / 10;
                result.Add(current);
                prev = current;
            }

            if (result[result.Count - 1] < maxTT)
            {
                result[result.Count - 1] = maxTT;
            }

            return result;
        }

        public List<double> getYAxisValues(DateTime minDate, DateTime maxDate)
        {
            DateTime startDate;
            if (minDate == null)
            {
                startDate = _calculationService.getMinStartDate();
            }
            else
            {
                startDate = minDate;
            }

            DateTime endDate;
            if (maxDate == null)
            {
                endDate = _calculationService.getMaxEndDate();
            }
            else
            {
                endDate = maxDate;
            }

            var xVals = getXAxisValues(startDate, endDate);
            var throughputTimes = _calculationService.getThroughputTimes(startDate, endDate);
            var numOrders = throughputTimes.Count;
            var frequencies = new List<double>();
            foreach (var val in xVals)
            {
                frequencies.Add(throughputTimes.Where(tt => tt <= val).LongCount());
            }
            var rel = new List<double>();
            rel.Add(0.0);

            for (int i = 1; i < frequencies.Count; i++)
            {
                rel.Add((frequencies[i] - frequencies[i - 1] / numOrders));
            }

            return rel;
        }
    }
}
