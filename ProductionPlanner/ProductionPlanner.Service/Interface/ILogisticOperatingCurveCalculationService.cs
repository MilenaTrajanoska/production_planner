using System;
using System.Collections.Generic;

namespace ProductionPlanner.Service.Interface
{
    public interface ILogisticOperatingCurveCalculationService
    {
        List<double> getOutputRateXAxisValues(DateTime minDate);
        List<double> getThroughputTimeXAxisValues(DateTime minDate);
        List<double> getRangeXAxisValues(DateTime minDate);
        List<double> getCapacityXAxisValues(DateTime minDate);
        List<double> getOPOperatingPointXAxisValues(DateTime minDate);
        double getOPRangeXAxisValues(DateTime minDate);
        double getOPThroughputTimeXAxisValues(DateTime minDate);
        List<double> getOutputRateYAxisValues(double WIPrel, DateTime minDate);
        List<double> getThroughputTimeYAxisValues(double WIPrel, DateTime minDate);
        List<double> getRangeYAxisValues(double WIPrel, DateTime minDate);
        List<double> getCapacityYAxisValues(DateTime minDate);
        List<double> getOPOperatingPointYAxisValues(DateTime minDate);
        double getOPRangeYAxisValues(DateTime minDate);
        double getOPThroughputTimeYAxisValues(DateTime minDate);
    }
}
