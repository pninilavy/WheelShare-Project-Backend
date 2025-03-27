using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public int RideId { get; set; }

        [ForeignKey("RideId")]
        public virtual Ride? Ride { get; set; }
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
        public double Amount { get; set; }
        public bool Status { get; set; }


    }
}
