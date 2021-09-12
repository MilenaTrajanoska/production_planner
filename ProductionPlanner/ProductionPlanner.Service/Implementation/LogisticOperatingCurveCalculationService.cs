using ProductionPlanner.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductionPlanner.Service.Implementation
{
    public class LogisticOperatingCurveCalculationService : ILogisticOperatingCurveCalculationService
    {
        private readonly ICalculationService _calculationService;
        private static double ALPHA = 10.0;   
        private static double T_CALC = 0.006847701;
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

        private double calculateIb(double t, DateTime minDate)
        {
            var WIPImin = _calculationService.calculateWipiMin(minDate);
            var Ua = _calculationService.calculateAverageUtilizationFromT(t);
            return WIPImin * Ua / 100;
        }

        private double calculateIp(double t, DateTime minDate)
        {
            var WIPImin = _calculationService.calculateWipiMin(minDate);
            return WIPImin * ALPHA * t;
        }

        public List<double> getOutputRateXAxisValues(DateTime minDate)
        {
            var Ib = new List<double>();
            T_VALUES_LIST.ForEach(t => Ib.Add(calculateIb(t, minDate)));

            var Ip = new List<double>();
            T_VALUES_LIST.ForEach(t => Ip.Add(calculateIp(t, minDate)));

            if (Ib.Count() != Ip.Count())
            {
                throw new InvalidOperationException();
            }
            return Ib.Zip(Ip, (a, b) => (a + b)).ToList();

        }

        private double calculateRoutMax(double WIPrel, DateTime minDate)
        {
            var Routavg = _calculationService.calculateAverageRout(minDate);
            var WS = _calculationService.getNumberOfWorkStations();

            if (WIPrel > (ALPHA + 1)*100)
            {
                return Routavg / WS;
            }
            var Ua = _calculationService.calculateAverageUtilizationFromT(T_CALC);
            return Routavg * 100 / (WS * Ua);
        }

        

        public List<double> getOutputRateYAxisValues(double WIPrel, DateTime minDate)
        {
            var WS = _calculationService.getNumberOfWorkStations();
            var RoutMax = calculateRoutMax(WIPrel, minDate);
            var result = new List<double>();
            T_VALUES_LIST.ForEach(
                t => result.Add(_calculationService.calculateAverageUtilizationFromT(t)/100*RoutMax*WS)
            );
            return result;
        }

        public List<double> getThroughputTimeXAxisValues(DateTime minDate)
        {
            //it's the same x
            return getOutputRateXAxisValues(minDate);
        }

        private double calculateThroughputTime(double RoutMax, double WS, double WCa, double WCv, double t, DateTime minDate)
        {
            var Ib = calculateIb(t, minDate);
            var Ip = calculateIp(t, minDate);
            var Im = Ib + Ip;
            var Ua = _calculationService.calculateAverageUtilizationFromT(t);
            return Im / (Ua * RoutMax * WS / 100) - WCa / RoutMax * Math.Pow(WCv, 2);
        }
        public List<double> getThroughputTimeYAxisValues(double WIPRel, DateTime minDate)
        {
            var RoutMax = calculateRoutMax(WIPRel, minDate);
            var WS = _calculationService.getNumberOfWorkStations();
            var WCa = _calculationService.calculateAverageWorkContent(minDate);
            var WCv = _calculationService.calculateRelativeWorkContent(minDate);
            var result = new List<double>();
            T_VALUES_LIST.ForEach(
                t => result.Add(calculateThroughputTime(RoutMax, WS, WCa, WCv, t, minDate))
                );
            return result;
        }

        public List<double> getRangeXAxisValues(DateTime minDate)
        {
            return getOutputRateXAxisValues(minDate);
        }

        private double calculateRm(double RoutMax, double WS, double t, DateTime minDate)
        {
            var Ib = calculateIb(t, minDate);
            var Ip = calculateIp(t, minDate);
            var Im = Ib + Ip;
            var Ua = _calculationService.calculateAverageUtilizationFromT(t);
            return Im / (Ua * RoutMax * WS / 100);
            
        }
        public List<double> getRangeYAxisValues(double WIPRel, DateTime minDate)
        {
            var RoutMax = calculateRoutMax(WIPRel, minDate);
            var WS = _calculationService.getNumberOfWorkStations();
            var result = new List<double>();
            T_VALUES_LIST.ForEach(
                t => result.Add(calculateRm(RoutMax, WS,t, minDate))
                );
            return result;
        }

        public List<double> getCapacityXAxisValues(DateTime minDate)
        {
            var result = new List<double>();
            result.Add(0);
            var t = T_VALUES_LIST.ElementAt(T_VALUES_LIST.Count() - 1);
            var Ib = calculateIb(t, minDate);
            var Ip = calculateIp(t, minDate);
            result.Add(Ip + Ib);
            return result;
        }

        public List<double> getCapacityYAxisValues(DateTime minDate)
        {
            var WS = _calculationService.getNumberOfWorkStations();
            var capacity = _calculationService.getWorkstationCapacity();
            return new List<double>()
            {
                WS*capacity,
                WS*capacity
            };
        }

        private List<DateTime> calculateDates(DateTime from)
        {
            DateTime startMin;

            if (from != null)
            {
                startMin = from;
            }
            else
            {
                startMin = _calculationService.getMinStartDate();
            }

            DateTime endMax = _calculationService.getMaxEndDate();

            List<DateTime> result = new List<DateTime>();

            while (startMin.Date.CompareTo(endMax.Date) <= 0)
            {
                result.Add(DateTime.Parse(startMin.ToString()));
                startMin.AddDays(1);
            }
            return result;
        }
        public List<double> getOPOperatingPointXAxisValues(DateTime minDate)
        {
            var dates = calculateDates(minDate);
            var WIPa = _calculationService.getListOfWIP(dates).ToList().Average();
            return new List<double>()
            {
                WIPa,
                WIPa
            };
        }

        public List<double> getOPOperatingPointYAxisValues(DateTime minDate)
        {
            DateTime minStartDate;
            if (minDate != null)
            {
                minStartDate = minDate;
            } else
            {
                minStartDate = _calculationService.getMinStartDate();
            }
            var endDate = _calculationService.getMaxEndDate();

            var daysBetween = endDate.Date.Subtract(minStartDate.Date).TotalDays;
            var dates = calculateDates(minDate);
            var WIPsum = _calculationService.getListOfWIP(dates).ToList().Sum();
            return new List<double>()
            {
                0,
                WIPsum / (daysBetween + 1)
            };
        }

        public double getOPRangeXAxisValues(DateTime minDate)
        {
            var dates = calculateDates(minDate);
            return _calculationService.getListOfWIP(dates).ToList().Average();
        }

        public double getOPRangeYAxisValues(DateTime minDate)
        {
            return _calculationService.calculateAverageRout(minDate);
        }

        public double getOPThroughputTimeXAxisValues(DateTime minDate)
        {
            var dates = calculateDates(minDate);
            return _calculationService.getListOfWIP(dates).ToList().Average();
        }

        public double getOPThroughputTimeYAxisValues(DateTime minDate)
        {
            DateTime startDate;
            if (minDate != null)
            {
                startDate = minDate;
            }
            else
            {
                startDate = _calculationService.getMinStartDate();
            }
            return _calculationService.getThroughputTimes(startDate).Average();
        }
    }
}
