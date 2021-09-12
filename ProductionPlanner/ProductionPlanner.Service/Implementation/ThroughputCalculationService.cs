using ProductionPlanner.Domain.Models;
using ProductionPlanner.Domain.ViewModels;
using ProductionPlanner.Repository.Implementation;
using ProductionPlanner.Repository.Interface;
using ProductionPlanner.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductionPlanner.Service.Implementation
{
    public class ThroughputCalculationService : IThroughputCalculationService
    {
        private IRepository<Order> _orderRepository;
        
        public ThroughputCalculationService(IRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public List<DateTime> calculateThroughputDiagramXAxis(DateTime from)
        {
            var orders = _orderRepository.GetAll();
            DateTime startMin = DateTime.MinValue;

            if (from != null)
            {
                startMin = from;
            }
            else
            {
                startMin = orders.Select(o => o.StartDate).Min().AddDays(-1);
            }
             
            DateTime endMax = orders.Select(o => o.EndDate).Max();
            List<DateTime> result = new List<DateTime>();
            
            while(startMin.Date.CompareTo(endMax.Date) <= 0)
            {
                result.Add(DateTime.Parse(startMin.ToString()));
                startMin.AddDays(1);
            }
            return result;
        }

        public List<double> calculateOutputSeries(List<DateTime> dates)
        {
            var minDate = dates.Min();
            var result = new List<double>();

            dates.ForEach(d => result.Add(calculateOutputForDate(minDate, d)));
            return result;
        }

        public List<double> calculateInputSeries(List<DateTime> dates)
        {
            var minDate = dates.Min();
            var result = new List<double>();

            dates.ForEach(d => result.Add(calculateInputForDate(minDate, d)));
            return result;
        }

        public List<double> calculateWIPSeries(List<DateTime> dates)
        {
            var minDate = dates.Min();
            var result = new List<double>();

            dates.ForEach(d => result.Add(calculateWIPForDate(minDate, d)));
            return result;
        }
        public double getCapacity(DateTime minDate, DateTime maxDate)
        {
            var daysBetween = maxDate.Date.Subtract(minDate.Date).TotalSeconds;
            return Company.getInstance().NumberOfWS * Company.getInstance().WSCapacity * daysBetween;
        }
        private double calculateInputForDate(DateTime from, DateTime to)
        {
            return _orderRepository.GetAll()
                .Where(o => o.StartDate.Date.CompareTo(from.Date) >= 0
                            && o.StartDate.Date.CompareTo(to.Date)<=0)
                .Select(o => o.getWorkContent())
                .ToList()
                .Sum();
        }

        private double calculateOutputForDate(DateTime from, DateTime to)
        {
            return _orderRepository.GetAll()
                .Where(o => o.StartDate.Date.CompareTo(from.Date) >= 0 
                            && o.EndDate.Date.CompareTo(to.Date) <= 0)
                .Select(o => o.getWorkContent())
                .ToList()
                .Sum();
        }

        private double calculateWIPForDate(DateTime from, DateTime to)
        {
            var cumulativeInput = calculateInputForDate(from, to);
            var cumulativeOutput = calculateOutputForDate(from, to);

            return cumulativeInput - cumulativeOutput;
        }
    }
}
