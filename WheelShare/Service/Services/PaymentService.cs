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
    public class PaymentService:IService<Payment>
    {
        private readonly IRepository<Payment> _repository;
        public PaymentService(IRepository<Payment> _repository)
        {
            this._repository = _repository;
        }
        public Task<Payment> Add(Payment item)
        {
            return _repository.Add(item);
        }

        public Task Delete(int id)
        {
            return _repository.Delete(id);
        }

        public Task<List<Payment>> GetAll()
        {
            return _repository.GetAll();
        }

        public Task<Payment> GetById(int id)
        {
            return _repository.GetById(id);
        }

        public Task<Payment> Update(int id, Payment item)
        {
            return _repository.Update(id, item);
        }
    }
}
