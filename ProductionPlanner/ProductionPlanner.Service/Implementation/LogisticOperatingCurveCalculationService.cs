using ProductionPlanner.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductionPlanner.Service.Implementation
{
    public class LogisticOperatingCurveCalculationService : ILogisticOperatingCurveCalculationService
    {
        public static double C_NORM = 0.25;
        public static double T_CALC = 0.006847701;
        public static double ALPHA = 10;
        private readonly ICalculationService _calculationService;
        private static List<double> T_VALUES_LIST = new List<double>() {
            0.0000001, 
            0.000001,
            0.00001,
            0.0001,
            0.001,
            0.01,
            0.02,
            0.04,
            0.065,
            0.1,
            0.2,
            0.5,
            0.7,
        };
        public LogisticOperatingCurveCalculationService(ICalculationService calculationService)
        {
            _calculationService = calculationService;
        }

        private double calculateIb(double t, DateTime minDate, DateTime maxDate)
        {
            var WIPImin = _calculationService.calculateWipiMin(minDate, maxDate);
            var Ua = _calculationService.calculateAverageUtilizationFromT(t);
            return WIPImin * Ua / 100;
        }

        private double calculateIp(double t, DateTime minDate, DateTime maxDate)
        {
            var WIPImin = _calculationService.calculateWipiMin(minDate, maxDate);
            return WIPImin * ALPHA * t;
        }

        public List<double> getOutputRateXAxisValues(DateTime minDate, DateTime maxDate)
        {
            var Ib = new List<double>();
            T_VALUES_LIST.ForEach(t => Ib.Add(calculateIb(t, minDate, maxDate)));

            var Ip = new List<double>();
            T_VALUES_LIST.ForEach(t => Ip.Add(calculateIp(t, minDate, maxDate)));

            if (Ib.Count() != Ip.Count())
            {
                throw new InvalidOperationException();
            }
            return Ib.Zip(Ip, (a, b) => (a + b)).ToList();

        }

        

        public List<double> getOutputRateYAxisValues(DateTime minDate, DateTime maxDate)
        {
            var WS = _calculationService.getNumberOfWorkStations();
            var RoutMax = _calculationService.calculateRoutMax(minDate, maxDate);
            
            var result = new List<double>();
            if (WS != 0 && RoutMax != 0)
            {
                T_VALUES_LIST.ForEach(
               t => result.Add(_calculationService.calculateAverageUtilizationFromT(t) / 100 * RoutMax * WS)
               );
            }
            else
            {
                T_VALUES_LIST.ForEach(
               t => result.Add(0)
               );
            }
               
            return result;
        }

        public List<double> getThroughputTimeXAxisValues(DateTime minDate, DateTime maxDate)
        {
            //it's the same x
            return getOutputRateXAxisValues(minDate, maxDate);
        }

        private double calculateThroughputTime(double RoutMax, double WS, double WCa, double WCv, double t, DateTime minDate, DateTime maxDate)
        {
            var Ib = calculateIb(t, minDate, maxDate);
            var Ip = calculateIp(t, minDate, maxDate);
            var Im = Ib + Ip;
            var Ua = _calculationService.calculateAverageUtilizationFromT(t);

            if (Ua == 0 || RoutMax == 0 || WS == 0 || WCv == 0) return 0;

            return Im / (Ua * RoutMax * WS / 100) - WCa / RoutMax * Math.Pow(WCv, 2);
        }
        public List<double> getThroughputTimeYAxisValues(DateTime minDate, DateTime maxDate)
        {
            var RoutMax = _calculationService.calculateRoutMax(minDate, maxDate);
            var WS = _calculationService.getNumberOfWorkStations();
            var WCa = _calculationService.calculateAverageWorkContent(minDate, maxDate);
            var WCv = _calculationService.calculateRelativeWorkContent(minDate, maxDate);
            var result = new List<double>();
            T_VALUES_LIST.ForEach(
                t => result.Add(calculateThroughputTime(RoutMax, WS, WCa, WCv, t, minDate, maxDate))
                );
            return result;
        }

        public List<double> getRangeXAxisValues(DateTime minDate, DateTime maxDate)
        {
            return getOutputRateXAxisValues(minDate, maxDate);
        }

        private double calculateRm(double RoutMax, double WS, double t, DateTime minDate, DateTime maxDate)
        {
            var Ib = calculateIb(t, minDate, maxDate);
            var Ip = calculateIp(t, minDate, maxDate);
            var Im = Ib + Ip;
            var Ua = _calculationService.calculateAverageUtilizationFromT(t);

            if (Ua == 0 || RoutMax == 0 || WS == 0) return 0;

            return Im / (Ua * RoutMax * WS / 100);
            
        }
        public List<double> getRangeYAxisValues(DateTime minDate, DateTime maxDate)
        {
            var RoutMax = _calculationService.calculateRoutMax(minDate, maxDate);
            var WS = _calculationService.getNumberOfWorkStations();
            var result = new List<double>();
            T_VALUES_LIST.ForEach(
                t => result.Add(calculateRm(RoutMax, WS,t, minDate, maxDate))
                );
            return result;
        }

        public List<double> getCapacityXAxisValues(DateTime minDate, DateTime maxDate)
        {
            var result = new List<double>();
            result.Add(0);
            var t = T_VALUES_LIST.ElementAt(T_VALUES_LIST.Count() - 1);
            var Ib = calculateIb(t, minDate, maxDate);
            var Ip = calculateIp(t, minDate, maxDate);
            result.Add(Ip + Ib);
            return result;
        }

        public List<double> getCapacityYAxisValues(DateTime minDate, DateTime maxDate)
        {
            var WS = _calculationService.getNumberOfWorkStations();
            var capacity = _calculationService.getWorkstationCapacity();
            return new List<double>()
            {
                WS*capacity,
                WS*capacity
            };
        }

        private DateTime getMinDate(DateTime date)
        {
            return date != null ? date : _calculationService.getMinStartDate();
        }

        private DateTime getMaxDate(DateTime date)
        {
            return date != null ? date : _calculationService.getMaxEndDate();
        }
        private List<DateTime> calculateDates(DateTime from, DateTime to)
        {
            DateTime startMin = getMinDate(from);

            DateTime endMax = getMaxDate(to);

            List<DateTime> result = new List<DateTime>();

            while (startMin.Date.CompareTo(endMax.Date) <= 0)
            {
                result.Add(DateTime.Parse(startMin.ToString()));
                startMin = startMin.AddDays(1);
            }
            return result;
        }
        public List<double> getOPOperatingPointXAxisValues(DateTime minDate, DateTime maxDate)
        {
            var WIPList = _calculationService.getListOfWIP(minDate, maxDate).ToList();
            var WIPa = WIPList.Count != 0 ? WIPList.Average() : 0;
            return new List<double>()
            {
                WIPa,
                WIPa
            };
        }

        public List<double> getOPOperatingPointYAxisValues(DateTime minDate, DateTime maxDate)
        {
            DateTime minStartDate = getMinDate(minDate);
            DateTime endDate = getMaxDate(maxDate);
            
            var daysBetween = endDate.Date.Subtract(minStartDate.Date).TotalDays;
            var WIPsum = _calculationService.getListOfWIP(minDate, maxDate).ToList().Sum();
            return new List<double>()
            {
                0,
                WIPsum / (daysBetween + 1)
            };
        }

        public double getOPRangeXAxisValues(DateTime minDate, DateTime maxDate)
        {
            var opList = _calculationService.getListOfWIP(minDate, maxDate).ToList();
            return opList.Count != 0 ? opList.Average() : 0;
        }

        public double getOPRangeYAxisValues(DateTime minDate, DateTime maxDate)
        {
            return _calculationService.calculateAverageRout(minDate, maxDate);
        }

        public double getOPThroughputTimeXAxisValues(DateTime minDate, DateTime maxDate)
        {
            var opList = _calculationService.getListOfWIP(minDate, maxDate).ToList();
            return opList.Count != 0 ? opList.Average() : 0;
        }

        public double getOPThroughputTimeYAxisValues(DateTime minDate, DateTime maxDate)
        {
            DateTime startDate = getMinDate(minDate);
            DateTime endDate = getMaxDate(maxDate);
            var ttpList = _calculationService.getThroughputTimes(startDate, endDate);

            return ttpList.Count != 0 ? ttpList.Average() : 0;
        }

    }
}
