using System;
using System.Collections.Generic;
using System.Text;

namespace ProductionPlanner.Service.Interface
{
    public interface IThroughputCalculationService
    {
        List<DateTime> calculateThroughputDiagramXAxis(DateTime from, DateTime to);
        List<double> calculateOutputSeries(DateTime minDate, DateTime maxDate);
        List<double> calculateInputSeries(DateTime minDate, DateTime maxDate);
        List<double> calculateWIPSeries(DateTime minDate, DateTime maxDate);
        double getCapacity(DateTime minDate, DateTime maxDate);
    }
}
