using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProductionPlanner.Domain.Models
{
    public class Company
    {
        [Key]
        public long Id { get; set; }
        public int NumberOfWS { get; set; }
        public double WSCapacity { get; set; }
        public double InterOpTime { get; set; }
        public double TransportantionAndStorageTime { get; set; }
        public int BeginningOfWIP { get; set; }
        public bool IsSet { get; set; }

        [ForeignKey("Instance")]
        public long InstanceId;
        public static Company Instance { get; set; }

        private Company()
        {
            IsSet = false;
        }

        public static Company getInstance()
        {
            if (Instance == null)
            {
                Instance = new Company();
            }
            return Instance;
        }

    }
}
