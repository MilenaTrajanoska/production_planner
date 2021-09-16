using ProductionPlanner.Domain.Models;
using ProductionPlanner.Repository.Implementation;
using ProductionPlanner.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductionPlanner.Service.Implementation
{
    public class CalculationService : ICalculationService
    {
        private static double C_NORM = 0.25;
        private readonly IRepository<Order> _orderRepository;
        private readonly ICompanyService _companyService;

        public CalculationService(IRepository<Order> orderRepository, ICompanyService companyService)
        {
            _orderRepository = orderRepository;
            _companyService = companyService;
        }

        public double calculateAverageUtilizationFromT(double t)
        {
            return 1 - Math.Pow(1 - Math.Pow(t, C_NORM), 1 / C_NORM);
        }

        public List<double> getWorkContents(DateTime minDate)
        {
            return _orderRepository.GetAll()
                .Where(o => o.StartDate.CompareTo(minDate) >= 0)
                .Select(o => o.getWorkContent())
                .ToList();
        }

        public double calculateAverageWorkContent(DateTime minDate)
        {
            return getWorkContents(minDate).Average();
        }

        public double calculateRelativeWorkContent(DateTime minDate)
        {
            var WCavg = calculateAverageWorkContent(minDate);
            var WCsd = calculateSdWorkContent(minDate);
            if (WCavg == 0)
            {
                return 0;
            }
            return WCsd / WCavg;
        }

        public double calculateSdWorkContent(DateTime minDate)
        {
            var orders = _orderRepository.GetAll()
                .Where(o => o.StartDate.CompareTo(minDate) >= 0)
                .ToList();
            if (orders.Count() == 0)
            {
                return 0;
            }

            double WCavg = calculateAverageWorkContent(minDate);
            double wc = orders.Select(o => Math.Sqrt(WCavg - o.getWorkContent()))
                .ToList()
                .Sum();

            return wc / orders.Count();
        }

        public double calculateWipiMin(DateTime minDate)
        {
            double WCavg = calculateAverageWorkContent(minDate);
            double WCv = calculateRelativeWorkContent(minDate);
            Company company = _companyService.GetCompany();
            if (company == null)
            {
                return 0;
            }

            return WCavg * (1 + Math.Pow(WCv, 2)) + company.InterOpTime;
        }
        public double calculateAverageRout(DateTime minDate)
        {
            var orders = _orderRepository.GetAll()
                .Where(o => o.StartDate.Date.CompareTo(minDate.Date)>=0)
                .ToList();
            var sumWC = orders.Select(o => o.getWorkContent()).Sum();
            var startMinDate = orders.Select(o => o.StartDate.Date).Min();
            var endMaxDate = orders.Select(o => o.EndDate.Date).Max();
            return sumWC / (endMaxDate.Subtract(startMinDate).TotalDays + 1);
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
            return input -output;
        }

        public List<double> getListOfWIP(List<DateTime> dates)
        {
            var minDate = getMinStartDate();
            var result = new List<double>();
            dates.ForEach(d => result.Add(getWIPForDate(minDate, d)));
            return result;
        }

        private double getDaysBetween(DateTime d1, DateTime d2)
        {
            return d2.Date.Subtract(d1.Date).TotalDays;
        }

        public List<double> getThroughputTimes(DateTime minDate)
        {
            return _orderRepository.GetAll()
                    .Where(o => o.StartDate.Date.CompareTo(minDate.Date) >= 0)
                    .Select(o => o.getThroughputTime())
                    .ToList();
        }
    }
}
