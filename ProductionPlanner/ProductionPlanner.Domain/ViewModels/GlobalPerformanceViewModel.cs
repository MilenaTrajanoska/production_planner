using System;
using System.Collections.Generic;
using System.Text;

namespace ProductionPlanner.Domain.ViewModels
{
    public class GlobalPerformanceViewModel
    {
        public int CompletedOrders { get; set; }
        public double AverageWorkContent { get; set; }
        public double StdWorkContent { get; set; }
        public double AverageThroughputTime { get; set; }
        public double AverageRange { get; set; }
        public double AverageOutputRate { get; set; }
        public double MaximumOutputRate { get; set; }
        public double Capacity { get; set; }
        public double AverageWIPLevel { get; set; }
        public double WIPIMin { get; set; }
        public double WIPRel { get; set; }
        public double AverageUtilizationRate { get; set; }
        public double ScheduleReliability { get; set; }
    }
}
