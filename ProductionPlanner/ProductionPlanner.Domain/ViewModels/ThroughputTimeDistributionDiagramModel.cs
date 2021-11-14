using System;
using System.Collections.Generic;
using System.Text;

namespace ProductionPlanner.Domain.ViewModels
{
    public class ThroughputTimeDistributionDiagramModel
    {
        public List<double> Classes { get; set; }
        public List<double> Rel { get; set; }

        public ThroughputTimeDistributionDiagramModel(List<double> classes, List<double> rel)
        {
            this.Classes = classes;
            this.Rel = rel;
        }
    }
}
