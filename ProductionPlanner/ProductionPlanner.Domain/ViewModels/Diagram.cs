using System;
using System.Collections.Generic;
using System.Text;

namespace ProductionPlanner.Domain.ViewModels
{
    public class Diagram
    {
        public ThroughputDiagram ThroughputDiagram { get; set; }
        public WorkContentDistributionDiagramModel WorkContentDistributionDiagramModel { get; set; }
        public LodisticOperatingCurvesDiagramModel LodisticOperatingCurvesDiagramModel { get; set; }
        public ThroughputTimeDistributionDiagramModel ThroughputTimeDistributionDiagramModel { get; set; }
        public ScheduleReliabilityOperatingCurveDiagramModel ScheduleReliabilityOperatingCurveDiagramModel { get; set; }

        public Diagram(
             ThroughputDiagram _throughputDiagram,
             WorkContentDistributionDiagramModel _workContentDistributionDiagramModel,
             LodisticOperatingCurvesDiagramModel _lodisticOperatingCurvesDiagramModel,
             ThroughputTimeDistributionDiagramModel _throughputTimeDistributionDiagramModel,
             ScheduleReliabilityOperatingCurveDiagramModel _scheduleReliabilityOperatingCurveDiagramModel
            )
        {
            this.ThroughputDiagram = _throughputDiagram;
            this.WorkContentDistributionDiagramModel = _workContentDistributionDiagramModel;
            this.LodisticOperatingCurvesDiagramModel = _lodisticOperatingCurvesDiagramModel;
            this.ThroughputTimeDistributionDiagramModel = _throughputTimeDistributionDiagramModel;
            this.ScheduleReliabilityOperatingCurveDiagramModel = _scheduleReliabilityOperatingCurveDiagramModel;
        }
    }
}
