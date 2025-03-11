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
    public class StationService:IService<Station>
    {
        private readonly IRepository<Station> _repository;
        public StationService(IRepository<Station> _repository)
        {
            this._repository = _repository;
        }
        public Task<Station> Add(Station item)
        {
            return _repository.Add(item);
        }

        public Task Delete(int id)
        {
            return _repository.Delete(id);
        }

        public Task<List<Station>> GetAll()
        {
            return _repository.GetAll();
        }

        public Task<Station> GetById(int id)
        {
            return _repository.GetById(id);
        }

        public Task<Station> Update(int id, Station item)
        {
            return _repository.Update(id, item);
        }
    }
}
