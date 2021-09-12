using System;
using System.Collections.Generic;
using System.Text;

namespace ProductionPlanner.Service.Interface
{
    public interface IThroughputCalculationService
    {
        List<DateTime> calculateThroughputDiagramXAxis(DateTime from);
        List<double> calculateOutputSeries(List<DateTime> dates);
        List<double> calculateInputSeries(List<DateTime> dates);
        List<double> calculateWIPSeries(List<DateTime> dates);
        double getCapacity(DateTime minDate, DateTime maxDate);
    }
}
