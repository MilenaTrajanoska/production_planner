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
        public virtual Product OrderedProduct { get; set; }
        public int Quantity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
