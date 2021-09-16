using System;
using System.Collections.Generic;
using System.Text;

namespace ProductionPlanner.Service.Interface
{
    public interface IWorkContentDistributionCalculationService
    {
        List<double> getXAxisValues(DateTime minDate);
        List<double> getYAxisValues(DateTime minDate);
    }
}
