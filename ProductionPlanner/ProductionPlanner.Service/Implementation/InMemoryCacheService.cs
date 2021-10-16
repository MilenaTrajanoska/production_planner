using Microsoft.Extensions.Caching.Memory;
using ProductionPlanner.Domain;
using ProductionPlanner.Domain.ViewModels;
using ProductionPlanner.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProductionPlanner.Service.Implementation
{
    public class InMemoryCacheService : IInMemoryCacheService
    {
        private static IMemoryCache _cache;
        private readonly ICompanyService _companyService;
        private readonly ICalculationService _calculationService;
        private readonly IOrderService _orderService;
        private readonly IScheduleReliabilityCalculationService _scheduleReliabilityCalculationService;
        private TimeSpan cacheExpirationPeriod = TimeSpan.FromHours(6);
        public InMemoryCacheService(
            IMemoryCache cache,
            ICompanyService companyService,
            ICalculationService calculationService,
            IOrderService orderService,
            IScheduleReliabilityCalculationService scheduleReliabilityCalculationService
            )
        {
            _companyService = companyService;
            _calculationService = calculationService;
            _orderService = orderService;
            _scheduleReliabilityCalculationService = scheduleReliabilityCalculationService;
            _cache = cache;
        }

        private GlobalPerformanceViewModel setGlobalPerformance()
        {
            var year = DateTime.Now.Year - 1;
            DateTime minDate = new DateTime(year, 1, 1);

            var company = _companyService.GetCompany();

            var performance = new GlobalPerformanceViewModel();
            var num_orders = _orderService.GetAllOrders()
                .Where(o => o.StartDate.Date.CompareTo(minDate.Date) >= 0)
                .ToList()
                .Count;
            performance.CompletedOrders = num_orders;
            DateTime maxDate = DateTime.Now;

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

        public  GlobalPerformanceViewModel GetPerformanceViewModel(GlobalPerformanceViewModel performance)
        {
            if (!_cache.TryGetValue(CacheKeys.GlobalPerformance, out performance))
            {
                // Key not in cache, so get data.
                performance = setGlobalPerformance();

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(cacheExpirationPeriod);

                // Save data in cache.
                _cache.Set(CacheKeys.GlobalPerformance, performance, cacheEntryOptions);
            }

            return performance;
        }

        public Diagram GetDiagram(Diagram diagram, String key)
        {
            if (!_cache.TryGetValue(key, out diagram))
            {
                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(cacheExpirationPeriod);

                // Save data in cache.
                _cache.Set(key, diagram, cacheEntryOptions);
            }
            return diagram;
        }


    }
}
