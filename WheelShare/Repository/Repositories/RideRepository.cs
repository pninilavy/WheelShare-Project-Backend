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
    public class RideRepository : IRepository<Ride>
    {
        private readonly IContext context;
        public RideRepository(IContext context)
        {
            this.context = context;
        }

        public async Task<Ride> Add(Ride item)
        {
            await context.Rides.AddAsync(item);
            await context.Save();
            return item;
        }

        public async Task Delete(int id)
        {
            context.Rides.Remove(await GetById(id));
            await context.Save();
        }

        public async Task<List<Ride>> GetAll()
        {
            return await context.Rides.ToListAsync();
        }

        public async Task<Ride> GetById(int id)
        {
            return await context.Rides.FirstOrDefaultAsync(ride => ride.Id == id);
        }

        public async Task<Ride> Update(int id, Ride item)
        {
            Ride ride=await GetById(id);
            ride.DriveId = item.DriveId;
            ride.Driver=item.Driver;
            ride.VehicleId = item.VehicleId;
            ride.Vehicle=item.Vehicle;
            ride.SourceStationID = item.SourceStationID;
            ride.SourceStation=item.SourceStation;
            ride.DestinationStationID = item.DestinationStationID;
            ride.DestinationStation=item.DestinationStation;
            ride.StartTime = item.StartTime;
            ride.EndTime = item.EndTime;
            ride.Status = item.Status;
            ride.TotalCost = item.TotalCost;
            ride.IsPrivateRide = item.IsPrivateRide;
            await context.Save();
            return ride;
        }
       
 
       
    }
}
