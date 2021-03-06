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
        private DateTime getMinDate(DateTime date)
        {
            return date != null ? date : _calculationService.getMinStartDate();
        }
        private DateTime getMaxDate(DateTime date)
        {
            return date != null ? date : _calculationService.getMaxEndDate();
        }
        public List<double> getXAxisValues(DateTime minDate, DateTime maxDate)
        {
            DateTime startDate = getMinDate(minDate);
            DateTime endDate = getMaxDate(maxDate);

            var workContents = _calculationService.getWorkContents(startDate, endDate);
            double minWC = 0;
            double maxWC = 0;
            if (workContents.Count == 0)
            {
                return new List<double>() { 0 };
            }

            maxWC = workContents.Max();

            var result = new List<double>();
            double prev = 0.0;
            result.Add(prev);
            double current = prev;
            
            while(current < maxWC)
            {
                current = prev + maxWC / 10;
                result.Add(current);
                prev = current;
            }

            if (result.Count > 0 && result[result.Count - 1] != maxWC)
            {
                result.Add(maxWC);
            }

            return result;

        }

        public List<double> getYAxisValues(DateTime minDate, DateTime maxDate)
        {
            DateTime startDate = getMinDate(minDate);
            DateTime endDate = getMaxDate(maxDate);

            var xVals = getXAxisValues(startDate, endDate);
            var workContents = _calculationService.getWorkContents(startDate, endDate);
            var numOrders = workContents.Count;
            var frequencies = new List<double>();

            if (xVals.Count <= 1)
            {
                return new List<double>() { 0 };
            }

            foreach (int val in xVals)
            {
                frequencies.Add(workContents.Where(wc => wc <= val).LongCount());
            }
            var rel = new List<double>();
            rel.Add(0.0);

            for(int i=1; i < frequencies.Count; i++)
            {
                if (numOrders != 0)
                {
                    rel.Add(((frequencies[i] - frequencies[i - 1] )/ numOrders)*100);
                } else
                {
                    rel.Add(0);
                }
                
            }

            return rel;
        }
    }
}
