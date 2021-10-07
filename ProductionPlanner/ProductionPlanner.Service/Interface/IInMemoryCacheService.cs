using ProductionPlanner.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductionPlanner.Service.Interface
{
    public interface IInMemoryCacheService
    {
        public GlobalPerformanceViewModel GetPerformanceViewModel(GlobalPerformanceViewModel performance);
    }
}
