using ProductionPlanner.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Statistics;
using MathNet.Numerics.Distributions;


namespace ProductionPlanner.Service.Implementation
{
    public class ScheduleReliabilityCalculationService : IScheduleReliabilityCalculationService
    {
        private readonly ICalculationService _calculationService;
        private readonly double STD_VAL = 0.0001;
        private readonly List<double> T_VALUES = new List<double>()
        {
            0.1,
            1,
            1.1,
            1.2,
            1.22,
            1.24,
            1.26,
            1.28,
            1.29,
            1.3,
            1.31,
            1.32,
            1.33,
            1.34,
            1.35,
            1.36,
            1.37,
            1.38,
            1.39,
            1.4,
            1.41,
            1.42,
            1.43,
            1.44,
            1.46,
            1.48,
            1.5,
            1.52,
            1.54,
            1.56,
            1.57,
            1.67,
            1.77
        };
        public ScheduleReliabilityCalculationService(ICalculationService calculationService)
        {
            _calculationService = calculationService;
        }
        public List<double> getXAxisMeanWIP(DateTime minDate)
        {
            var WIPmActual = calculateWIPmActual(minDate);
            var WIPImin = _calculationService.calculateWipiMin(minDate);
            double res;
            if(WIPmActual > 1.6 * WIPImin)
            {
                res = WIPmActual;
            } 
            else
            {
                res = 0;
            }
            return new List<double>
            {
                res,
                res
            };
        }
        public List<double> getYAxisMeanWIP(DateTime minDate)
        {
            var WIPmActual = calculateWIPmActual(minDate);
            var WIPImin = _calculationService.calculateWipiMin(minDate);
            double res;
            if (WIPmActual > 1.6 * WIPImin)
            {
                res = 100;
            } else
            {
                res = 0;
            }
            return new List<double>()
            {
                0,
                res
            };
        }

        public List<double> getXAxisScheduleReliability(DateTime minDate)
        {
            var WIPImin = _calculationService.calculateWipiMin(minDate);
            var wipAverage = new List<double>();
            var wipBuffer = new List<double>();
            foreach (var t in T_VALUES)
            {
                wipAverage.Add(WIPImin * calculateUtilizationMean(t) / 100);
                wipBuffer.Add(WIPImin * _calculationService.getAlpha() * t);
            }

            return wipAverage.Zip(wipBuffer, (a, b) => (a + b)).ToList();

        }

        

        public List<double> getYAxisScheduleReliability(DateTime minDate)
        {
            var TIO = _calculationService.calculateTIO(minDate);
            var b = calculateUpperTIOBoundry(minDate);
            var a = calculateLowerTIOBoundry(minDate);
            var TIOms = calculateTIOm(minDate);

            var TIOs = Statistics.StandardDeviation(_calculationService.getThroughputTimes(minDate));
            var phiListB = new List<double>();
            var phiListA = new List<double>();

            if (TIOs > 0)
            { 
                phiListB = TIOms.Select(t => cumulativeNormalDistValue((b - t) / TIO)).ToList();
                phiListA = TIOms.Select(t => cumulativeNormalDistValue((a - t) / TIO)).ToList();
            } 
            else
            {
                phiListB = TIOms.Select(t => cumulativeNormalDistValue((b - t) / STD_VAL)).ToList();
                phiListA = TIOms.Select(t => cumulativeNormalDistValue((a - t) / STD_VAL)).ToList();
            }

            return phiListB.Zip(phiListA, (a, b) => (a - b) * 100).ToList();
            
        }

        private double calculateWIPmActual(DateTime minDate)
        {
            var Rm = calculateRm(minDate);
            var WIPRel = _calculationService.calculateWIPRel(minDate);
            var routMax = _calculationService.calculateRoutMax(WIPRel, minDate);
            var WS = _calculationService.getNumberOfWorkStations();

            return Rm * routMax * WS;
        }

        private double calculateRm(DateTime minDate)
        {
            var TIO = _calculationService.calculateTIO(minDate);
            var WCa = _calculationService.calculateAverageWorkContent(minDate);
            var WIPRel = _calculationService.calculateWIPRel(minDate);
            var routMax = _calculationService.calculateRoutMax(WIPRel, minDate);

            if (routMax == 0)
            {
                return 0;
            }
            return TIO + WCa / routMax;
        }
        private double cumulativeNormalDistValue(double variable)
        {
            return Normal.CDF(1, 0, variable);
        }

        private double calculateUtilizationMean(double t)
        {
            return _calculationService.calculateAverageUtilizationFromT(t);
        }

        private double calculateUpperTIOBoundry(DateTime minDate)
        {
            return _calculationService.calculateTIO(minDate) + 1;
        }
        private double calculateLowerTIOBoundry(DateTime minDate)
        {
            return _calculationService.calculateTIO(minDate) - 1;
        }
        private List<double> calculateTIOm(DateTime minDate)
        {
            var TTPms = calculateTTPmean(minDate);
            var WIPRel = _calculationService.calculateWIPRel(minDate);
            var WCa = _calculationService.calculateAverageWorkContent(minDate);
            var routMax = _calculationService.calculateRoutMax(WIPRel, minDate);

            if (TTPms == null || routMax==0)
            {
                return null; 
            }
            return TTPms.Select(t => t - WCa / routMax).ToList();
        }
        private List<double> calculateTTPmean(DateTime minDate)
        {
            var WIPms = getXAxisScheduleReliability(minDate);
            var Um = T_VALUES.Select(t => calculateUtilizationMean(t)).ToList();
            var WIPRel = _calculationService.calculateWIPRel(minDate);
            var routMax = _calculationService.calculateRoutMax(WIPRel, minDate);
            var WS = _calculationService.getNumberOfWorkStations();
            var WCa = _calculationService.calculateAverageWorkContent(minDate);
            var WCv = _calculationService.calculateRelativeWorkContent(minDate);

            if (routMax == 0 || WS == 0)
            {
                return null;
            }

            if(Um.Count != WIPms.Count)
            {
                throw new Exception("A processing error occured!");
            }

            var result = new List<double>();
            for(int i=0; i<WIPms.Count; i++)
            {
                if (Um[i] == 0)
                {
                    result.Add(0);
                }
                else
                {
                    result.Add(WIPms[i] /
                        (Um[i] * routMax * WS) - WCa /
                        routMax * Math.Pow(WCv, 2));
                }
            }

            return result;
        }
    }
}
