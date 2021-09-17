using System;
using System.Collections.Generic;
using System.Text;

namespace ProductionPlanner.Domain.Models
{
    public class MaterialForProduct : BaseEntity
    {
        public long ProductId { get; set; }
        public Product Product { get; set; }
        public long MaterialId { get; set; }
        public Material Material { get; set; }
    }
}
