using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Entities;

namespace Service.Models
{
    public class StationAndDuration
    {
        public Station Station { get; set; }
        public double Duration { get; set; }
        public StationAndDuration(Station Station, double Duration)
        {
            this.Station = Station;
            this.Duration = Duration;
        }
    }
}
