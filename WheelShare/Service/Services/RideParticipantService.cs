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
    public class RideParticipantService:IService<RideParticipant>
    {
        private readonly IRepository<RideParticipant> _repository;
        public RideParticipantService(IRepository<RideParticipant> _repository)
        {
            this._repository = _repository;
        }
        public Task<RideParticipant> Add(RideParticipant item)
        {
            return _repository.Add(item);
        }

        public Task Delete(int id)
        {
            return _repository.Delete(id);
        }

        public Task<List<RideParticipant>> GetAll()
        {
            return _repository.GetAll();
        }

        public Task<RideParticipant> GetById(int id)
        {
            return _repository.GetById(id);
        }

        public Task<RideParticipant> Update(int id, RideParticipant item)
        {
            return _repository.Update(id, item);
        }
    }
}
