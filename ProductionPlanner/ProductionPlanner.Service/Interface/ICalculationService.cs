using System;
using System.Collections.Generic;

namespace ProductionPlanner.Service.Interface
{
    public interface ICalculationService
    {
        public List<double> getWorkContents(DateTime minDate, DateTime maxDate);
        double calculateAverageWorkContent(DateTime minDate, DateTime maxDate);
        double calculateSdWorkContent(DateTime minDate, DateTime maxDate);
        double calculateRelativeWorkContent(DateTime minDate, DateTime maxDate);
        double calculateWipiMin(DateTime minDate, DateTime maxDate);
        double calculateAverageUtilizationFromT(double t);
        double calculateAverageRout(DateTime minDate, DateTime maxDate);
        double getNumberOfWorkStations();
        double getWorkstationCapacity();
        DateTime getMinStartDate();
        DateTime getMaxEndDate();
        public double calculateInputForDate(DateTime from, DateTime to);
        public double calculateOutputForDate(DateTime from, DateTime to);
        double getWIPForDate(DateTime startDate, DateTime endDate);
        public List<double> getListOfWIP(DateTime minDate, DateTime maxDate);
        public List<double> getThroughputTimes(DateTime minDate, DateTime maxDate);
        public double calculateTIO(DateTime minDate, DateTime maxDate);
        public double calculateAverageUtilizationGlobal();
        public double getAlpha();
        public double calculateRoutMax(DateTime minDate, DateTime maxDate);
        public double calculateWIPRel(DateTime minDate, DateTime maxDate);
    }
}
