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
    public class VehicleService:IService<Vehicle>
    {
        private readonly IRepository<Vehicle> _repository;
        public VehicleService(IRepository<Vehicle> _repository)
        {
            this._repository = _repository;
        }
        public Task<Vehicle> Add(Vehicle item)
        {
            return _repository.Add(item);
        }

        public Task Delete(int id)
        {
            return _repository.Delete(id);
        }

        public Task<List<Vehicle>> GetAll()
        {
            return _repository.GetAll();
        }

        public Task<Vehicle> GetById(int id)
        {
            return _repository.GetById(id);
        }

        public Task<Vehicle> Update(int id, Vehicle item)
        {
            return _repository.Update(id, item);
        }
    }
}
