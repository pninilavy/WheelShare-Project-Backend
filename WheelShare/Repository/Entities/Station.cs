using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class Station
    {
        public int Id { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string Address { get; set; }   
        public virtual ICollection<Vehicle> Vehicles { get;set; }

        
    }
}
