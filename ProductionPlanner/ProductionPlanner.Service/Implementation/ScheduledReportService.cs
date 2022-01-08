using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductionPlanner.Domain;
using ProductionPlanner.Domain.ViewModels;
using ProductionPlanner.Service.Interface;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductionPlanner.Service.Implementation
{
    public class ScheduledReportService : IHostedService
    {
        private IServiceScopeFactory _scopeFactory;
        private Timer _timerMonth;
        private Timer _timerYear;
        private TimeSpan _maxInterval = TimeSpan.FromDays(49);

        public ScheduledReportService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            TimeSpan interval = TimeSpan.FromDays(30);
            //calculate time to run the first time & delay to set the timer
            //DateTime.Today gives time of midnight 00.00
            var today = DateTime.Today;
            var nextRunTime = new DateTime(today.Year, today.Month, 1);
            nextRunTime = nextRunTime.AddMonths(1);
            var curTime = DateTime.Now;
            var firstInterval = nextRunTime.Subtract(curTime);

            //Action action = () =>
            //{
            //    var t1 = Task.Delay(0);
            //    t1.Wait();
            //    //remove inactive accounts at expected time
            //    ExecuteDataCalculationMonthlyTask(null);
            //now schedule it to be called every 24 hours for future
            // timer repeates call to RemoveScheduledAccounts every 24 hours.
            _timerMonth = new Timer(
                ExecuteDataCalculationMonthlyTask,
                null,
                TimeSpan.Zero,
                firstInterval
            );
            //};

            TimeSpan intervalYear = TimeSpan.FromDays(365);
            //calculate time to run the first time & delay to set the timer
            //DateTime.Today gives time of midnight 00.00
            var nextRunTimeYear = new DateTime(today.Year, 1, 1);
            nextRunTimeYear = nextRunTimeYear.AddYears(1);

            var secondInterval = nextRunTimeYear.Subtract(curTime);

            //Action actionYear = () =>
            //{
            //    var t1 = Task.Delay(0);
            //    t1.Wait();
            //remove inactive accounts at expected time
            //ExecuteDataCalculationAnnualTask(null);
            //now schedule it to be called every 24 hours for future
            // timer repeates call to RemoveScheduledAccounts every 24 hours.
            if (secondInterval.CompareTo(_maxInterval) >= 0)
            {
                secondInterval = _maxInterval;
            }
            _timerYear = new Timer(
                ExecuteDataCalculationAnnualTask,
                null,
                TimeSpan.Zero,
                secondInterval
            );
            //};

            // no need to await this call here because this task is scheduled to run much much later.
            //Task.Run(action);
            //Task.Run(actionYear);

            return Task.CompletedTask;
        }

        private void ExecuteDataCalculationMonthlyTask(object state)
        {
            var today = DateTime.Now;
            var month = new DateTime(today.Year, today.Month, 1);
            var first = month.AddMonths(-1);
            var last = month.AddDays(-1);
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var _inMemoryCacheService = scope.ServiceProvider.GetRequiredService<IInMemoryCacheService>();
                    _inMemoryCacheService.clearMonthlyKeys();

                    Diagram _d = new Diagram();
                    _d = _inMemoryCacheService.GetDiagram(_d, first, last, CacheKeys.Diagram_Monthly);
                    GlobalPerformanceViewModel _p = new GlobalPerformanceViewModel();
                    _p = _inMemoryCacheService.GetPerformanceViewModel(_p, first, last);
                }
            }
            catch (Exception e) { }

        }

        private void ExecuteDataCalculationAnnualTask(object state)
        {
            var today = DateTime.Now;

            if (today.Month != 1 || today.Day != 1)
            {
                return;
            }

            var year = new DateTime(today.Year, 1, 1);
            var first = year.AddYears(-1);
            var last = year.AddDays(-1);
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var _inMemoryCacheService = scope.ServiceProvider.GetRequiredService<IInMemoryCacheService>();
                    _inMemoryCacheService.clearYearlyKeys();

                    Diagram _d = new Diagram();
                    _d = _inMemoryCacheService.GetDiagram(_d, first, last, CacheKeys.Diagram_Yearly);
                }
            }
            catch { }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timerMonth?.Change(Timeout.Infinite, 0);
            _timerYear?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}
