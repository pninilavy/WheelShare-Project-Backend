using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class VehicleAvailability
    {
       
        public int Id { get; set; }

        public int VehicleId { get; set; }

        [ForeignKey("VehicleId")]
        public virtual Vehicle? Vehicle { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public VehicleAvailability( int VehicleId, DateTime Date, TimeSpan StartTime, TimeSpan EndTime)
        {
            this.VehicleId = VehicleId;
            this.Date = Date;
            this.StartTime = StartTime;
            this.EndTime = EndTime;
        }

    }
}
