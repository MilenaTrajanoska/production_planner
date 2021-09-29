using System;
using System.Collections.Generic;
using System.Text;

namespace ProductionPlanner.Domain.ViewModels
{
    public class LodisticOperatingCurvesDiagramModel
    {
        private List<double> locCapacity;

        public List<double> OutputRate { get; set; }
        public List<double> ThroughputTime { get; set; }
        public double Range { get; set; }
        public List<double> Capacity { get; set; }
        public List<double> OPOperatingPoin { get; set; }
        public double OPRange { get; set; }
        public double OPThroughputTime { get; set; }

        public LodisticOperatingCurvesDiagramModel(List<double> outputRate, List<double> throughputTime, double range, List<double> locCapacity, List<double> opOperatingPoin, double opRange, double opThroughputTime)
        {
            this.OutputRate = outputRate;
            this.ThroughputTime = throughputTime;
            this.Range = range;
            this.Capacity = locCapacity;
            this.OPOperatingPoin = opOperatingPoin;
            this.OPRange = opRange;
            this.OPThroughputTime = opThroughputTime;
        }
    }
}
