using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProductionPlanner.Domain.Models
{
    public class Order : BaseEntity
    {
        [ForeignKey("OrderedProduct")]
        public long ProductId { get; set; }
        public int ProductVersion { get; set; }
        public virtual ProductHistory OrderedProduct { get; set; }
        public int Quantity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public double getWorkContent()
        {
            return Quantity * OrderedProduct.InProcessTime + OrderedProduct.SetUpTime;
        }

        public double getThroughputTime()
        {
            return Convert.ToInt32(EndDate.Date.Subtract(StartDate.Date).TotalDays);
        }
    }
}
