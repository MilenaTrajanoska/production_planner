using System;
using System.Collections.Generic;
using System.Text;

namespace ProductionPlanner.Domain.ViewModels
{
    public class ScheduleReliabilityOperatingCurveDiagramModel
    {
        public List<double> Labels { get; set; }
        public List<double> ScheduleReliability { get; set; }
        public List<double> MeanWIP { get; set; }

        public ScheduleReliabilityOperatingCurveDiagramModel(List<double> labels, List<double> scheduleReliability, List<double> meanWIP)
        {
            this.Labels = labels;
            this.ScheduleReliability = scheduleReliability;
            this.MeanWIP = meanWIP;
        }
    }
}
