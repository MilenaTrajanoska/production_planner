using Microsoft.Extensions.Caching.Memory;
using ProductionPlanner.Domain;
using ProductionPlanner.Domain.ViewModels;
using ProductionPlanner.Service.Interface;
using System;

namespace ProductionPlanner.Service.Implementation
{
    public class InMemoryCacheService : IInMemoryCacheService
    {
        private static IMemoryCache _cache;
        private readonly IDiagramService _diagramService;

        private TimeSpan cacheExpirationPeriod = TimeSpan.FromDays(30);
        private TimeSpan cacheExpirationPeriodYear = TimeSpan.FromDays(365);

        public InMemoryCacheService(
            IMemoryCache cache,
            IDiagramService diagramService
            )
        {
            _diagramService = diagramService;
            _cache = cache;
        }

        public GlobalPerformanceViewModel GetPerformanceViewModel(GlobalPerformanceViewModel performance, DateTime minDate, DateTime maxDate)
        {
            if (!_cache.TryGetValue(CacheKeys.GlobalPerformance, out performance))
            {
                _cache.Remove(CacheKeys.GlobalPerformance);
                // Key not in cache, so get data.
                performance = _diagramService.setGlobalPerformance(_diagramService.getMinDateOfOrders(), _diagramService.getMaxDateOfOrders());

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(cacheExpirationPeriod);

                // Save data in cache.
                _cache.Set(CacheKeys.GlobalPerformance, performance, cacheEntryOptions);
            }

            return performance;
        }

        public void clearMonthlyKeys()
        {
            _cache.Remove(CacheKeys.Diagram_Monthly);
            _cache.Remove(CacheKeys.GlobalPerformance);
        }

        public void clearYearlyKeys()
        {
            _cache.Remove(CacheKeys.Diagram_Yearly);
        }

        public Diagram GetDiagram(Diagram diagram, DateTime minDate, DateTime maxDate, String key)
        {
            if (!_cache.TryGetValue(key, out diagram))
            {
                _cache.Remove(key);
                diagram = _diagramService.GetDiagram(_diagramService.getMinDateOfOrders(), _diagramService.getMaxDateOfOrders());
                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration( key == CacheKeys.Diagram_Monthly ? 
                    cacheExpirationPeriod : cacheExpirationPeriodYear);

                // Save data in cache.
                _cache.Set(key, diagram, cacheEntryOptions);
            }
            return diagram;
        }

    }
}
