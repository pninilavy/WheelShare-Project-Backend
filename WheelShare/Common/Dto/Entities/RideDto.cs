using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Entities;
using static System.Collections.Specialized.BitVector32;

namespace Common.Dto.Entities
{
    public class RideDto
    {
       
        public int Id { get; set; }
        public int DriveId { get; set; }

        [ForeignKey("DriveId")]
        public virtual User? Driver { get; set; }
     
        public int? SourceStationID { get; set; }

        
        public int? DestinationStationID { get; set; }

        

        public string SourceAddress { get; set; }
        public string DestinationAddress { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; }
        public double TotalCost { get; set; }
        public bool SharedRide { get; set; }
        public int NumSeats { get; set; }
        public RideDto(Ride ride)
        {
            this.Id = ride.Id;
            this.DriveId = ride.DriveId;
            this.Driver = ride.Driver;
            this.SourceAddress = ride.SourceAddress;
            this.DestinationAddress = ride.DestinationAddress;
            this.Date = ride.Date;
            this.StartTime = ride.StartTime;
            this.EndTime = ride.EndTime;
            this.Status = ride.Status;
            this.TotalCost = ride.TotalCost;
            this.SourceStationID = ride.SourceStationID;
            this.DestinationStationID = ride.DestinationStationID;
        }
    }
}
