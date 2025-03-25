using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.Interfaces;

namespace Repository.Repositories
{
    public class VehicleAvailabilityRepository : IRepository<VehicleAvailability>
    {
        private readonly IContext context;

        public VehicleAvailabilityRepository(IContext context)
        {
            this.context = context;
        }
        public async Task<VehicleAvailability> Add(VehicleAvailability item)
        {
            await context.VehicleAvailabilities.AddAsync(item);
            await context.Save();
            return item;
        }

        public async Task Delete(int id)
        {
            context.VehicleAvailabilities.Remove(await GetById(id));
            await context.Save();
        }

        public async Task<List<VehicleAvailability>> GetAll()
        {
            return await context.VehicleAvailabilities.ToListAsync();

        }

        public async Task<VehicleAvailability> GetById(int id)
        {
            return await context.VehicleAvailabilities.FirstOrDefaultAsync(vehicleAvailability => vehicleAvailability.Id == id);

        }

        public async Task<VehicleAvailability> Update(int id, VehicleAvailability item)
        {
            VehicleAvailability vehicleAvailability = await GetById(id);
            vehicleAvailability.VehicleId=item.VehicleId;
            vehicleAvailability.Date = item.Date;
            vehicleAvailability.StartTime = item.StartTime;
            vehicleAvailability.EndTime = item.EndTime;
            await context.Save();
            return vehicleAvailability;
        }



    }
}
