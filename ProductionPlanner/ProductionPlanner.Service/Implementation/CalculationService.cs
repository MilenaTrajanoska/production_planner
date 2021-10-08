using ProductionPlanner.Domain.Models;
using ProductionPlanner.Repository.Implementation;
using ProductionPlanner.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Statistics;

namespace ProductionPlanner.Service.Implementation
{
    public class CalculationService : ICalculationService
    {
        public static double C_NORM = 0.25;
        public static double T_CALC = 0.006847701;
        public static double ALPHA = 10;
        private readonly IRepository<Order> _orderRepository;
        private readonly ICompanyService _companyService;

        public CalculationService(IRepository<Order> orderRepository, ICompanyService companyService)
        {
            _orderRepository = orderRepository;
            _companyService = companyService;
        }

        public double getAlpha()
        {
            return ALPHA;
        }
        public double calculateAverageUtilizationFromT(double t)
        {
            return (1 - Math.Pow(1 - Math.Pow(t, C_NORM), 1 / C_NORM)) * 100;
        }

        public double calculateAverageUtilizationGlobal()
        {
            return calculateAverageUtilizationFromT(T_CALC);
        }

        public List<double> getWorkContents(DateTime minDate, DateTime maxDate)
        {
            return _orderRepository.GetAll()
                .Where(o => o.StartDate.Date.CompareTo(minDate.Date) >= 0 && o.EndDate.Date.CompareTo(maxDate.Date) <= 0)
                .Select(o => o.getWorkContent())
                .ToList();
        }

        public double calculateAverageWorkContent(DateTime minDate, DateTime maxDate)
        {
            return getWorkContents(minDate, maxDate).Average();
        }

        public double calculateRelativeWorkContent(DateTime minDate, DateTime maxDate)
        {
            var WCavg = calculateAverageWorkContent(minDate, maxDate);
            var WCsd = calculateSdWorkContent(minDate, maxDate);
            if (WCavg == 0)
            {
                return 0;
            }
            return WCsd / WCavg;
        }

        public double calculateSdWorkContent(DateTime minDate, DateTime maxDate)
        {
            var orders = _orderRepository.GetAll()
                .Where(o => o.StartDate.Date.CompareTo(minDate.Date) >= 0 && o.EndDate.Date.CompareTo(maxDate.Date) <= 0)
                .ToList();
            if (orders.Count() == 0)
            {
                return 0;
            }

            double WCavg = calculateAverageWorkContent(minDate, maxDate);
            double wc = orders.Select(o => Math.Sqrt(Math.Pow(WCavg - o.getWorkContent(), 2)))
                .ToList()
                .Sum();

            return wc / orders.Count();
        }

        public double calculateWipiMin(DateTime minDate, DateTime maxDate)
        {
            double WCavg = calculateAverageWorkContent(minDate, maxDate);
            double WCv = calculateRelativeWorkContent(minDate, maxDate);
            Company company = _companyService.GetCompany();
            if (company == null)
            {
                return 0;
            }

            return WCavg * (1 + Math.Pow(WCv, 2)) + company.InterOpTime;
        }
        public double calculateAverageRout(DateTime minDate, DateTime maxDate)
        {
            var orders = _orderRepository.GetAll()
                .Where(o => o.StartDate.Date.CompareTo(minDate.Date)>=0 && o.EndDate.Date.CompareTo(maxDate.Date) <= 0)
                .ToList();
            double sumWC = orders.Select(o => o.getWorkContent()).Sum();
            
            if (orders.Count == 0)
            {
                return 0;
            }
            return sumWC / (maxDate.Subtract(minDate).TotalDays + 1);
        }

        public double getNumberOfWorkStations()
        {
            return _companyService.GetCompany().NumberOfWS;
        }

        public double getWorkstationCapacity()
        {
            return _companyService.GetCompany().WSCapacity;
        }

        public DateTime getMinStartDate()
        {
            return _orderRepository.GetAll()
                .Select(o => o.StartDate)
                .ToList()
                .Min();
        }

        public DateTime getMaxEndDate()
        {
            return _orderRepository.GetAll()
                .Select(o => o.EndDate)
                .ToList()
                .Max();
        }
        public double calculateInputForDate(DateTime from, DateTime to)
        {
            return _orderRepository.GetAll()
                .Where(o => o.StartDate.Date.CompareTo(from.Date) >= 0
                            && o.StartDate.Date.CompareTo(to.Date) <= 0)
                .Select(o => o.getWorkContent())
                .ToList()
                .Sum();
        }
        public double calculateOutputForDate(DateTime from, DateTime to)
        {
            return _orderRepository.GetAll()
                .Where(o => o.StartDate.Date.CompareTo(from.Date) >= 0
                            && o.EndDate.Date.CompareTo(to.Date) <= 0)
                .Select(o => o.getWorkContent())
                .ToList()
                .Sum();
        }
        public double getWIPForDate(DateTime startDate, DateTime endDate)
        {
            DateTime start;
            if (startDate == null)
            {
                start = getMinStartDate();
            }
            else
            {
                start = startDate;
            }

            var input = calculateInputForDate(start, endDate);
            var output = calculateOutputForDate(start, endDate);
            return input-output;
        }

        public List<double> getListOfWIP(DateTime minDate, DateTime maxDate)
        {

            var result = new List<double>();
            var dates = calculateDates(minDate, maxDate);
            dates.ForEach(d => result.Add(getWIPForDate(minDate, d)));
            return result;
        }

        private double getDaysBetween(DateTime d1, DateTime d2)
        {
            return d2.Date.Subtract(d1.Date).TotalDays;
        }

        public List<double> getThroughputTimes(DateTime minDate, DateTime maxDate)
        {
            DateTime startDate;
            if(minDate == null)
            {
                startDate = getMinStartDate();
            }
            else
            {
                startDate = minDate;
            }

            DateTime endDate;
            if (maxDate == null)
            {
                endDate = getMaxEndDate();
            }
            else
            {
                endDate = maxDate;
            }
            return _orderRepository.GetAll()
                    .Where(o => o.StartDate.Date.CompareTo(startDate.Date) >= 0 && o.EndDate.Date.CompareTo(endDate.Date) <= 0)
                    .Select(o => o.getThroughputTime())
                    .ToList();
        }

        public double calculateTIO(DateTime minDate, DateTime maxDate)
        {
            var throughputTimes = getThroughputTimes(minDate, maxDate);
            var avgUtilization = calculateAverageUtilizationFromT(T_CALC);
            var avgThroughputTimes = throughputTimes.Average();
            var stdThroughputTimes = Statistics.StandardDeviation(throughputTimes.AsEnumerable());

            return (avgThroughputTimes * avgUtilization / 100) 
                * ((1 + Math.Pow(stdThroughputTimes, 2)) / 2) 
                / (1 - avgUtilization / 100);

        }

        public double calculateRoutMax( DateTime minDate, DateTime maxDate)
        {
            double WIPrel = calculateWIPRel(minDate, maxDate);
            var Routavg = calculateAverageRout(minDate, maxDate);
            var WS = getNumberOfWorkStations();

            if (WIPrel > (ALPHA + 1) * 100)
            {
                return Routavg / WS;
            }
            var Ua = calculateAverageUtilizationFromT(T_CALC);
            return Routavg * 100 / (WS * Ua);
        }

        private List<DateTime> calculateDates(DateTime from, DateTime to)
        {
            DateTime startMin;

            if (from != null)
            {
                startMin = from;
            }
            else
            {
                startMin = getMinStartDate();
            }

            DateTime endMax;
            if (to != null)
            {
                endMax = to;
            }else
            {
                endMax = getMaxEndDate();
            }
            
            List<DateTime> result = new List<DateTime>();

            while (startMin.Date.CompareTo(endMax.Date) <= 0)
            {
                result.Add(DateTime.Parse(startMin.ToString()));
                startMin = startMin.AddDays(1);
            }
            return result;
        }

        public double calculateWIPRel(DateTime minDate, DateTime maxDate)
        {
            var WIPa = getListOfWIP(minDate, maxDate).Average();
            var WIPImin = calculateWipiMin(minDate, maxDate);

            if (WIPImin == 0)
            {
                return 0;
            }
            return WIPa / WIPImin * 100;
        }

    }
}
