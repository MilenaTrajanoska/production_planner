using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProductionPlanner.Domain.Models
{
    public class Material : BaseEntity
    {
        [ForeignKey("PartOfProduct")]
        public long PartOfProductId { get; set; }
        public virtual Product PartOfProduct { get; set; }
        public string MaterialName { get; set; }
        public double UnitsNeeded { get; set; }
        public double CostPerUnit { get; set; }
        public double CostPerProduct { get; set; }
        public int EndingInventory { get; set; }
    }
}
