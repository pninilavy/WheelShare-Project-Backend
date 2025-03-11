using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.Interfaces;

namespace Mock
{
    public class WheelShareContext : DbContext, IContext
    {
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Ride> Rides { get; set; }
        public DbSet<RideParticipant> RideParticipants { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        public async Task Save()
        {
            await SaveChangesAsync();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=DESKTOP-FQ9CF9I;database=WheelShare;trusted_connection=true;TrustServerCertificate=True");
        }

       


    }
}