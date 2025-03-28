using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class RideParticipant
    {
        public int Id { get; set; }
        public int RideId {  get; set; }

        [ForeignKey("RideId")]
        public virtual Ride? Ride { get; set; }
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
        public string PickupLocation { get; set; }
        public string DropOffLocation { get; set; }
        public double ShareCost {  get; set; }

        public string Status { get; set; }

        public double DriverCost {  get; set; }

        public TimeSpan PickUpTime {  get; set; }

    }
}
