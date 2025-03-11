using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository.Entities;

namespace Repository.Interfaces
{
    public interface IContext
    {
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Ride> Rides { get; set; }
        public DbSet<RideParticipant> RideParticipants { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public Task Save();
    }
}
