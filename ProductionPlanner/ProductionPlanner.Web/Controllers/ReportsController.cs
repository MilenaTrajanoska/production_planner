using Microsoft.AspNetCore.Mvc;
using ProductionPlanner.Domain;
using ProductionPlanner.Domain.ViewModels;
using ProductionPlanner.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ProductionPlanner.Web.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IOrderService orderService;
        private readonly IInMemoryCacheService inMemoryCacheService;

        public ReportsController(
            IOrderService orderService,
            IInMemoryCacheService inMemoryCacheService
            )
        {
            this.orderService = orderService;
            this.inMemoryCacheService = inMemoryCacheService;
        }

        public IActionResult Index()
        {

            List<DateTime> datesWeekly = this.LastWeekDates();
            DateTime minDate = this.orderService.GetAllOrders().Select(order => order.StartDate).Min();
            DateTime maxDate = this.orderService.GetAllOrders().Select(order => order.StartDate).Max();
            Diagram diagram = new Diagram();
            diagram = inMemoryCacheService.GetDiagram(diagram, minDate, maxDate, CacheKeys.Diagram_Monthly);
            return View(diagram);
            //GetDiagram
        }

        public IActionResult MonthReports()
        {
            List<DateTime> datesMonthly = this.LastMonthDates();
            DateTime minDate = datesMonthly.FirstOrDefault();
            DateTime maxDate = datesMonthly.LastOrDefault();
            //   Diagram diagram = this.GetDiagram(minDate, maxDate);
            return View();
        }

        public IActionResult YearReports()
        {
            List<DateTime> datesYearly = this.YearDates(2021);
            DateTime minDate = datesYearly.FirstOrDefault();
            DateTime maxDate = datesYearly.LastOrDefault();
            //    Diagram diagram = this.GetDiagram(minDate, maxDate);
            return View();
        }

        private List<DateTime> LastWeekDates()
        {
            var lastSunday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            var lastMonday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek - 6);
            return new List<DateTime> { lastMonday, lastSunday };
        }

        private List<DateTime> LastMonthDates()
        {
            var dateNow = DateTime.Now;
            var dateLastMonth = DateTime.Now.AddMonths(-1);
            //var daysInMonth = DateTime.DaysInMonth(dateNow.Year, dateNow.Month);
            //var daysInLastMonth = DateTime.DaysInMonth(dateLastMonth.Year, dateLastMonth.Month);
            var firstOfMonth = DateTime.Now.AddDays(-(int)dateNow.Day + 1).AddMonths(-1);
            var lastOfMonth = DateTime.Now.AddDays(-(int)dateNow.Day);
            return new List<DateTime> { firstOfMonth, lastOfMonth };
        }

        private List<DateTime> YearDates(int year)
        {
            var yearsBack = DateTime.Now.Year - year;
            var firstOfYear = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfYear + 1).AddYears(-yearsBack);
            var lastOfYear = DateTime.Today.AddDays(-1).AddYears(-yearsBack);
            return new List<DateTime> { firstOfYear, lastOfYear };
        }

    }
}
