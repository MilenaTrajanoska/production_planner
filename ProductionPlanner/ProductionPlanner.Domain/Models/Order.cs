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
        [Required(ErrorMessage = "The order number is required")]
        public string OrderName { get; set; }
        public virtual ProductHistory OrderedProduct { get; set; }
        [Required(ErrorMessage = "The quantity of the ordered products is required")]
        public int Quantity { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "The start date of processing is required")]
        public DateTime StartDate { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "The end date of processing is required")]
        public DateTime EndDate { get; set; }
        public bool IsValid { get; set; }

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
