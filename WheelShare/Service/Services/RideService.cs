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
    public class RideService:IService<Ride>
    {
        private readonly IRepository<Ride> _repository;
        public RideService(IRepository<Ride> _repository)
        {
            this._repository = _repository;
        }
        public Task<Ride> Add(Ride item)
        {
            return _repository.Add(item);
        }

        public Task Delete(int id)
        {
            return _repository.Delete(id);
        }

        public Task<List<Ride>> GetAll()
        {
            return _repository.GetAll();
        }

        public Task<Ride> GetById(int id)
        {
            return _repository.GetById(id);
        }

        public Task<Ride> Update(int id, Ride item)
        {
            return _repository.Update(id, item);
        }
    }
}
