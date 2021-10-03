using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProductionPlanner.Domain.Models
{
    public class Material : BaseEntity
    {
        public ICollection<MaterialForProduct> MaterialForProduct { get; set; }
        [Required(ErrorMessage = "The Material Name is Required")]
        public string MaterialName { get; set; }
        public double UnitsNeeded { get; set; }
        [Required(ErrorMessage = "The Cost Per Unit is Required")]
        [Range(0, Double.MaxValue)]
        public double CostPerUnit { get; set; }
        public double CostPerProduct { get; set; }
        public int EndingInventory { get; set; }
    }
}
