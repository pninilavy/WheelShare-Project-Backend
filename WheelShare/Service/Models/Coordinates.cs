using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.NewFolder
{
    public class Coordinates
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Coordinates(double x,double y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
