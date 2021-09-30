using System;
using System.Collections.Generic;

namespace ProductionPlanner.Service.Interface
{
    public interface ILogisticOperatingCurveCalculationService
    {
        List<double> getOutputRateXAxisValues(DateTime minDate, DateTime maxDate);
        List<double> getThroughputTimeXAxisValues(DateTime minDate, DateTime maxDate);
        List<double> getRangeXAxisValues(DateTime minDate, DateTime maxDate);
        List<double> getCapacityXAxisValues(DateTime minDate, DateTime maxDate);
        List<double> getOPOperatingPointXAxisValues(DateTime minDate, DateTime maxDate);
        double getOPRangeXAxisValues(DateTime minDate, DateTime maxDate);
        double getOPThroughputTimeXAxisValues(DateTime minDate, DateTime maxDate);
        List<double> getOutputRateYAxisValues(DateTime minDate, DateTime maxDate);
        List<double> getThroughputTimeYAxisValues(DateTime minDate, DateTime maxDate);
        List<double> getRangeYAxisValues(DateTime minDate, DateTime maxDate);
        List<double> getCapacityYAxisValues(DateTime minDate, DateTime maxDate);
        List<double> getOPOperatingPointYAxisValues(DateTime minDate, DateTime maxDate);
        double getOPRangeYAxisValues(DateTime minDate, DateTime maxDate);
        double getOPThroughputTimeYAxisValues(DateTime minDate, DateTime maxDate);
    }
}
