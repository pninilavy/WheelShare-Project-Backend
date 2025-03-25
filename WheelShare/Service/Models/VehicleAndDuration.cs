using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Entities;

namespace Service.Models
{
    public class VehicleAndDuration
    {
        public Vehicle Vehicle { get; set; }
        public double Duration { get; set; }
        public VehicleAndDuration(Vehicle Vehicle, double Duration)
        {
            this.Vehicle = Vehicle;
            this.Duration = Duration;
        }
    }
}
