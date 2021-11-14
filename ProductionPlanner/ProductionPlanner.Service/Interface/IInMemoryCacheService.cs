using ProductionPlanner.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductionPlanner.Service.Interface
{
    public interface IInMemoryCacheService
    {
        public GlobalPerformanceViewModel GetPerformanceViewModel(GlobalPerformanceViewModel performance, DateTime minDate, DateTime maxDate);
        public Diagram GetDiagram(Diagram diagram, DateTime minDate, DateTime maxDate, String key);
    }
}
