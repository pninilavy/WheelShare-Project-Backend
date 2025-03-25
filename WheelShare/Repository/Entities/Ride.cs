using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Repository.Entities
{
    public class Ride
    {
        public int Id { get; set; }
        public int DriveId { get; set; }

        [ForeignKey("DriveId")]
        public virtual User? Driver { get; set; }
        public int? VehicleId { get; set; }

        [ForeignKey("VehicleId")]
        public virtual Vehicle ? Vehicle { get; set; }
        public int? SourceStationID { get; set; }

        [ForeignKey("SourceStationID")]
        public virtual Station? SourceStation { get; set; }
        public int ? DestinationStationID { get; set; }

        [ForeignKey("DestinationStationID")]
        public virtual Station? DestinationStation { get; set; }

        public string SourceAddress { get; set; }
        public string DestinationAddress { get; set; }
        public DateTime Date{ get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; }
        public double TotalCost { get; set; }
        public bool SharedRide { get; set; }
        public int NumSeats { get; set; }

    }
}
