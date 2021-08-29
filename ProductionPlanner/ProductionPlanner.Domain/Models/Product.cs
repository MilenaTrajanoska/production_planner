using System;

namespace ProductionPlanner.Domain.Models
{
    public class Product : BaseEntity
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
        public DateTime LastModified { get; set; }
        public int CurrentVersion { get; set; }
        public bool IsValid { get; set; }

        public Product()
        {
            CurrentVersion = 1;
        }

    }
}
