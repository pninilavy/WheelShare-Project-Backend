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
    public class StationRepository : IRepository<Station>
    {
        private readonly IContext context;
        public StationRepository(IContext context)
        {
            this.context = context;
        }
        public async Task<Station> Add(Station item)
        {
           await context.Stations.AddAsync(item);
           await context.Save();
           return item;
        }

        public async Task Delete(int id)
        {
            context.Stations.Remove(await GetById(id));
            await context.Save();
        }

        public async Task<List<Station>> GetAll()
        {
            return await context.Stations.ToListAsync();
        }

        public async Task<Station> GetById(int id)
        {
            return await context.Stations.FirstOrDefaultAsync(station => station.Id == id);
        }

        public async Task<Station> Update(int id, Station item)
        {
            Station station=await GetById(id);
            station.Area = item.Area;
            station.City = item.City;
            station.Address = item.Address;
            await context.Save();
            return station;

        }
    }
}


