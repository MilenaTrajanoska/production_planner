using System;
using System.Collections.Generic;
using System.Text;

namespace ProductionPlanner.Service.Interface
{
    public interface IScheduleReliabilityCalculationService
    {
        List<double> getXAxisScheduleReliability(DateTime minDate, DateTime maxDate);
        List<double> getYAxisScheduleReliability(DateTime minDate, DateTime maxDate);
        List<double> getXAxisMeanWIP(DateTime minDate, DateTime maxDate);
        List<double> getYAxisMeanWIP(DateTime minDate, DateTime maxDate);
    }
}
