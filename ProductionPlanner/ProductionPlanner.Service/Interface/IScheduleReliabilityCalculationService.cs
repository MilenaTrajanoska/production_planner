using System;
using System.Collections.Generic;
using System.Text;

namespace ProductionPlanner.Service.Interface
{
    public interface IScheduleReliabilityCalculationService
    {
        List<double> getXAxisScheduleReliability(DateTime minDate);
        List<double> getYAxisScheduleReliability(DateTime minDate);
        List<double> getXAxisMeanWIP(DateTime minDate);
        List<double> getYAxisMeanWIP(DateTime minDate);
    }
}
