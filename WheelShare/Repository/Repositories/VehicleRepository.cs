using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.Interfaces;

namespace Repository.Repositories
{
    public class VehicleRepository : IRepository<Vehicle>
    {
        private readonly IContext context;
        public VehicleRepository(IContext context)
        {
            this.context = context;
        }

        public async Task<Vehicle> Add(Vehicle item)
        {
            await context.Vehicles.AddAsync(item);
            await context.Save();
            return item;
        }

        public async Task Delete(int id)
        {
           context.Vehicles.Remove(await GetById(id));
           await context.Save();

        }

        public async Task<List<Vehicle>> GetAll()
        {
            return await context.Vehicles.ToListAsync();
        }

        public async Task<Vehicle> GetById(int id)
        {
           return await context.Vehicles.FirstOrDefaultAsync(vehicle=> vehicle.Id==id);
        }

        public async Task<Vehicle> Update(int id, Vehicle item)
        {
            Vehicle vehicle= await GetById(id);
            vehicle.LicensePlate=item.LicensePlate;
            vehicle.Seats=item.Seats;
            vehicle.StationID=item.StationID;
            vehicle.Station=item.Station;
            vehicle.AvailabilityStatus=item.AvailabilityStatus;
            await context.Save();
            return vehicle;
        }
    }  
       
   
    }
