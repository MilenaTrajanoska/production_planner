using System;
using System.Collections.Generic;

namespace ProductionPlanner.Service.Interface
{
    public interface ICalculationService
    {
        public List<double> getWorkContents(DateTime minDate);
        double calculateAverageWorkContent(DateTime minDate);
        double calculateSdWorkContent(DateTime minDate);
        double calculateRelativeWorkContent(DateTime minDate);
        double calculateWipiMin(DateTime minDate);
        double calculateAverageUtilizationFromT(double t);
        double calculateAverageRout(DateTime minDate);
        double getNumberOfWorkStations();
        double getWorkstationCapacity();
        DateTime getMinStartDate();
        DateTime getMaxEndDate();
        public double calculateInputForDate(DateTime from, DateTime to);
        public double calculateOutputForDate(DateTime from, DateTime to);
        double getWIPForDate(DateTime startDate, DateTime endDate);
        public List<double> getListOfWIP(List<DateTime> dates);
        public List<double> getThroughputTimes(DateTime minDate);
    }
}
