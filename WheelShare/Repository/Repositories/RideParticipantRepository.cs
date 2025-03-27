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
    public class RideParticipantRepository : IRepository<RideParticipant>
    {
        private readonly IContext context;
        public RideParticipantRepository(IContext context)
        {
            this.context = context;
        }

        public async Task<RideParticipant> Add(RideParticipant item)
        {
            await context.RideParticipants.AddAsync(item);
            await context.Save();
            return item;
        }

        public async Task Delete(int id)
        {
            context.RideParticipants.Remove(await GetById(id));
            await context.Save();
        }

        public async Task<List<RideParticipant>> GetAll()
        {
           return await context.RideParticipants.ToListAsync();
        }

        public async Task<RideParticipant> GetById(int id)
        {
            return await context.RideParticipants.FirstOrDefaultAsync(rideParticipant => rideParticipant.Id == id);
        }

        public async Task<RideParticipant> Update(int id, RideParticipant item)
        {
            RideParticipant rideParticipant=await GetById(id);
            rideParticipant.RideId = id;
            rideParticipant.Ride=item.Ride;
            rideParticipant.UserId = item.UserId;
            rideParticipant.User = item.User;
            rideParticipant.PickupLocation = item.PickupLocation;
            rideParticipant.DropOffLocation= item.DropOffLocation;
            rideParticipant.ShareCost= item.ShareCost;
            rideParticipant.Status= item.Status;
            rideParticipant.DriverCost= item.DriverCost;
            await context.Save();
            return rideParticipant;
        }
    }
}
