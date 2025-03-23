using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;

namespace Service.Services
{
    public class VehicleAvailabilityService : IService<VehicleAvailability>
    {
        private readonly IRepository<VehicleAvailability> _repository;
        public VehicleAvailabilityService(IRepository<VehicleAvailability> _repository)
        {
            this._repository = _repository;
        }
        public Task<VehicleAvailability> Add(VehicleAvailability item)
        {
            return _repository.Add(item);
        }

        public Task Delete(int id)
        {
            return _repository.Delete(id);
        }

        public Task<List<VehicleAvailability>> GetAll()
        {
            return _repository.GetAll();
        }

        public Task<VehicleAvailability> GetById(int id)
        {
            return _repository.GetById(id);
        }

        public Task<VehicleAvailability> Update(int id, VehicleAvailability item)
        {
            return _repository.Update(id, item);
        }
    }
}
