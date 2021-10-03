using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductionPlanner.Domain.Models
{
    public class Product : BaseEntity
    {
        public ICollection<MaterialForProduct> MaterialForProduct { get; set; }
        [Required(ErrorMessage = "The product name is required")]
        public String ProductName { get; set; }
        [Required(ErrorMessage = "The in process time is required")]
        public double InProcessTime { get; set; }
        [Required(ErrorMessage = "The setup time is required")]
        public double SetUpTime { get; set; }
        [Required(ErrorMessage = "The pre processing waiting time is required")]
        public double PreProcessingWaitTime { get; set; }
        [Required(ErrorMessage = "The post processing waiting time is required")]
        public double PostProcessingWaitTime { get; set; }
        [Required(ErrorMessage = "The selling price is required")]
        public double SellingPrice { get; set; }
        [Required(ErrorMessage = "The wage is required")]
        public double WagePerHour { get; set; }
        [Required(ErrorMessage = "The variable OH per DL hour is required")]
        public double VariableOHPerDLHour { get; set; }
        [Required(ErrorMessage = "The product interest rate is required")]
        public double InterestRate { get; set; }
        [Required(ErrorMessage = "The fixed OH per year is required")]
        public double FixedOHPerYear { get; set; }
        public DateTime LastModified { get; set; }
        public int CurrentVersion { get; set; }
        public bool IsValid { get; set; }

        public Product()
        {
            CurrentVersion = 1;
            MaterialForProduct = new List<MaterialForProduct>();
        }

        public Double TotalMaterialCost()
        {
            double cost = 0;
            foreach(var mat in MaterialForProduct)
            {
                cost += mat.Material.CostPerUnit * mat.Quantity;
            }
            return cost;
        }

    }
}
