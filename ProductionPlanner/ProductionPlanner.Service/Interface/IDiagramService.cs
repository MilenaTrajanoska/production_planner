using ProductionPlanner.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductionPlanner.Service.Interface
{
    public interface IDiagramService
    {
        public Diagram GetDiagram(DateTime minDate, DateTime maxDate);
        public GlobalPerformanceViewModel setGlobalPerformance(DateTime min, DateTime max);
    }
}
