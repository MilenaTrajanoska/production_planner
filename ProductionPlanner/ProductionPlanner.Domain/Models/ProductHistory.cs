using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductionPlanner.Domain.Models
{
    public class ProductHistory : BaseEntity
    {
        public String ProductName { get; set; }
        public double InProcessTime { get; set; }
        public double SetUpTime { get; set; }
        public double PreProcessingWaitTime { get; set; }
        public double PostProcessingWaitTime { get; set; }
        public double SellingPrice { get; set; }
        public double TotalMaterialCost { get; set; }
        public double WagePerHour { get; set; }
        public double VariableOHPerDLHour { get; set; }
        public double InterestRate { get; set; }
        public double FixedOHPerYear { get; set; }
        public int Version { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        [ForeignKey("ReferencedProduct")]
        public long ProductId { get; set; }
        public virtual Product ReferencedProduct { get; set; }

        public ProductHistory()
        {
            Version = 1;
        }
    }
}
