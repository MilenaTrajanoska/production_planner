using System;
using System.Collections.Generic;
using System.Text;

namespace ProductionPlanner.Domain.ViewModels
{
    public class LodisticOperatingCurvesDiagramModel
    {
        public List<double> Labels { get; set; }
        public List<double> OutputRate { get; set; }
        public List<double> ThroughputTime { get; set; }
        public double RangeX { get; set; }
        public double RangeY { get; set; }
        public List<double> Capacity { get; set; }
        public List<double> OPOperatingPoin { get; set; }
        public double OPRangeX { get; set; }
        public double OPRangeY { get; set; }
        public double OPThroughputTimeX { get; set; }
        public double OPThroughputTimeY { get; set; }

        public LodisticOperatingCurvesDiagramModel(List<double> labels, List<double> outputRate, List<double> throughputTime, double rangeX, double rangeY, List<double> locCapacity, List<double> opOperatingPoin, double opRangeX, double opRangeY, double opThroughputTimeX, double opThroughputTimeY)
        {
            this.Labels = labels;
            this.OutputRate = outputRate;
            this.ThroughputTime = throughputTime;
            this.RangeX = rangeX;
            this.RangeY = rangeY;
            this.Capacity = locCapacity;
            this.OPOperatingPoin = opOperatingPoin;
            this.OPRangeX = opRangeX;
            this.OPRangeY = opRangeY;
            this.OPThroughputTimeX = opThroughputTimeX;
            this.OPThroughputTimeY = opThroughputTimeY;
        }
    }
}
