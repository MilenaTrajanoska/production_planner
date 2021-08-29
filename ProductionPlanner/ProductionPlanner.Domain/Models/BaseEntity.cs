using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProductionPlanner.Domain.Models
{
    public class BaseEntity
    {
        [Key]
        public long Id { get; set; }
    }
}
