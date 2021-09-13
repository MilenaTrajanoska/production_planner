using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public bool isValid { get; set; }

        [ForeignKey("Instance")]
        private long InstanceId;
        private static Company Instance { get; set; }
        private static readonly object syncLock = new object();

        private Company()
        {
            IsSet = false;
            isValid = false;
        }

        public static Company getInstance()
        {
            if (Instance == null)
            {
                lock(syncLock){
                    if(Instance == null)
                    {
                        Instance = new Company();
                    }
                }
                
            }
            return Instance;
        }

    }
}
