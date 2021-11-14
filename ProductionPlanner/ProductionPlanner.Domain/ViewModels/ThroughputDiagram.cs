using System;
using System.Collections.Generic;
using System.Text;

namespace ProductionPlanner.Domain.ViewModels
{
    public class ThroughputDiagram
    {
        public List<string> Labels { get; set; }
        public List<double> Input { get; set; }
        public List<double> Output { get; set; }
        public List<double> WIP { get; set; }
        public double Capacity { get; set; }

        public ThroughputDiagram(List<string> labels, List<double> input, List<double> output, List<double> wip, double cap)
        {
            this.Labels = labels;
            this.Input = input;
            this.Output = output;
            this.WIP = wip;
            this.Capacity = cap;
        }
    }
}
