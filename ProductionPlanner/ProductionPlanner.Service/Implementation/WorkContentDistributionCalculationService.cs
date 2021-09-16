using ProductionPlanner.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProductionPlanner.Service.Implementation
{
    public class WorkContentDistributionCalculationService : IWorkContentDistributionCalculationService
    {
        private readonly ICalculationService _calculationService;

        public WorkContentDistributionCalculationService(ICalculationService calculationService)
        {
            _calculationService = calculationService;
        }
        public List<double> getXAxisValues(DateTime minDate)
        {
            DateTime startDate;
            if(minDate == null)
            {
                startDate = _calculationService.getMinStartDate();
            }
            else
            {
                startDate = minDate;
            }

            var workContents = _calculationService.getWorkContents(startDate);
            var minWC = workContents.Min();
            var maxWC = workContents.Max();

            var result = new List<double>();
            double prev = 0.0;
            result.Add(prev);
            double current = prev;
            
            while(current <= maxWC)
            {
                current = prev + minWC / 10;
                result.Add(current);
                prev = current;
            }

            return result;

        }

        public List<double> getYAxisValues(DateTime minDate)
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

            var xVals = getXAxisValues(startDate);
            var workContents = _calculationService.getWorkContents(startDate);
            var numOrders = workContents.Count;
            var frequencies = new List<double>();
            foreach (var val in xVals)
            {
                frequencies.Add(workContents.Where(wc => wc <= val).LongCount());
            }
            var rel = new List<double>();
            rel.Add(0.0);

            for(int i=1; i<frequencies.Count; i++)
            {
                rel.Add((frequencies[i] - frequencies[i - 1] / numOrders));
            }

            return rel;
        }
    }
}
