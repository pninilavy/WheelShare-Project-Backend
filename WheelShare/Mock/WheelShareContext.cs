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
        public DbSet<VehicleAvailability> VehicleAvailabilities { get; set; }

        public async Task Save()
        {
            await SaveChangesAsync();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=DESKTOP-FQ9CF9I;database=WheelShare;trusted_connection=true;TrustServerCertificate=True");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Station>().HasData(
           new Station {Id=-10,  Area = "מרכז", City = "אלעד", Address = "שמעיה 6" },
           new Station { Id = -1, Area = "מרכז", City = "אלעד", Address = "אבן גבירול 8"},
           new Station { Id = -2, Area = "מרכז", City = "אלעד", Address = "רבן יוחנן בן זכאי 97" },
           new Station { Id = -3, Area = "מרכז", City = "בני ברק", Address = "רבי עקיבא 100" },
           new Station { Id = -4, Area = "מרכז", City = "בני ברק", Address = "ז'בוטינסקי 150" },
           new Station { Id = -5, Area = "מרכז", City = "בני ברק", Address = "חזון איש 50" },
           new Station { Id = -6, Area = "מרכז", City = "ירושלים", Address = "יפו 234" },
           new Station { Id = -7, Area = "מרכז", City = "ירושלים", Address = "עזה 29" },
           new Station { Id = -8, Area = "מרכז", City = "ירושלים", Address = "דרך חברון 101" },
           new Station { Id = -9, Area = "מרכז", City = "ירושלים", Address = "הנביאים 54" }

            );
            modelBuilder.Entity<Vehicle>().HasData(
                new Vehicle {Id=-1, LicensePlate = "123-45-678", Seats = 5, StationID = -10, CostPerHour = 12.9 },
                new Vehicle { Id = -2, LicensePlate = "123-45-679", Seats = 5, StationID = -10,CostPerHour = 12.9 },
                new Vehicle { Id = -3, LicensePlate = "123-45-677", Seats = 7, StationID = -10, CostPerHour = 19.9 },
                new Vehicle { Id = -4, LicensePlate = "234-56-789", Seats = 5, StationID = -1, CostPerHour = 12.9 },
                new Vehicle { Id = -5, LicensePlate = "234-56-788", Seats = 7, StationID = -1, CostPerHour = 19.9 },
                new Vehicle { Id = -6, LicensePlate = "345-67-890", Seats = 7, StationID = -2, CostPerHour = 19.9 },
                new Vehicle { Id = -7, LicensePlate = "234-56-789", Seats = 5, StationID = -2, CostPerHour = 12.9 },
                new Vehicle { Id = -8, LicensePlate = "234-56-788", Seats = 7, StationID = -2, CostPerHour = 19.9 },
                new Vehicle { Id = -9, LicensePlate = "111-23-456", Seats = 7, StationID = -3, CostPerHour = 19.9 },
                new Vehicle { Id = -10, LicensePlate = "111-23-567", Seats = 5, StationID = -3, CostPerHour = 12.9 },
                new Vehicle { Id = -11, LicensePlate = "111-23-678", Seats = 5, StationID = -3, CostPerHour = 12.9 },
                new Vehicle { Id = -12, LicensePlate = "111-23-789", Seats = 7, StationID = -3, CostPerHour = 19.9 },
                new Vehicle { Id = -13, LicensePlate = "222-34-567", Seats = 5, StationID = -4, CostPerHour = 12.9 },
                new Vehicle { Id = -14, LicensePlate = "222-34-678", Seats = 5, StationID = -4, CostPerHour = 12.9 },
                new Vehicle { Id = -15, LicensePlate = "222-34-789", Seats = 7, StationID = -4, CostPerHour = 19.9 },
                new Vehicle { Id = -16, LicensePlate = "333-23-678", Seats = 5, StationID = -5, CostPerHour = 12.9 },
                new Vehicle { Id = -17, LicensePlate = "333-34-456", Seats = 7, StationID = -5, CostPerHour = 19.9 },
                new Vehicle { Id = -18, LicensePlate = "333-34-567", Seats = 5, StationID = -5, CostPerHour = 12.9 },
                new Vehicle { Id = -19, LicensePlate = "333-34-678", Seats = 5, StationID = -5, CostPerHour = 12.9 },
                new Vehicle { Id = -20, LicensePlate = "333-34-789", Seats = 7, StationID = -5, CostPerHour = 19.9 },
                new Vehicle { Id = -21, LicensePlate = "444-34-678", Seats = 5, StationID = -6, CostPerHour = 12.9 },
                new Vehicle { Id = -22, LicensePlate = "444-34-789", Seats = 7, StationID = -7, CostPerHour = 19.9 },
                new Vehicle { Id = -23, LicensePlate = "444-23-789", Seats = 5, StationID = -7, CostPerHour = 12.9 },
                new Vehicle { Id = -24, LicensePlate = "444-56-789", Seats = 5, StationID = -7, CostPerHour = 12.9 },
                new Vehicle { Id = -25, LicensePlate = "444-78-789", Seats = 7, StationID = -7, CostPerHour = 19.9 },
                new Vehicle { Id = -26, LicensePlate = "555-23-678", Seats = 5, StationID = -8, CostPerHour = 12.9 },
                new Vehicle { Id = -27, LicensePlate = "555-34-456", Seats = 7, StationID = -8, CostPerHour = 19.9 },
                new Vehicle { Id = -28, LicensePlate = "555-34-567", Seats = 5, StationID = -8, CostPerHour = 12.9 },
                new Vehicle { Id = -29, LicensePlate = "555-34-678", Seats = 5, StationID = -8, CostPerHour = 12.9 },
                new Vehicle { Id = -30, LicensePlate = "555-34-789", Seats = 7, StationID = -8, CostPerHour = 19.9 },
                new Vehicle { Id = -31, LicensePlate = "666-34-567", Seats = 5, StationID = -9, CostPerHour = 12.9 },
                new Vehicle { Id = -32, LicensePlate = "666-34-678", Seats = 5, StationID = -9, CostPerHour = 12.9 },
                new Vehicle { Id = -33, LicensePlate = "666-34-789", Seats = 7, StationID = -9, CostPerHour = 19.9 }
                );



        }




    }
}