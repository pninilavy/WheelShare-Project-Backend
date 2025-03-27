using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class Vehicle
    {       
        public int Id { get; set; }
        public string LicensePlate { get; set; }
        public int Seats { get; set; }
        public int StationID { get; set; }

        [ForeignKey("StationID")]
        public virtual Station?Station { get; set; }
        public double CostPerHour { get; set; }
        public virtual ICollection<VehicleAvailability>? VehicleAvailabilities { get; set; }

    }
}
