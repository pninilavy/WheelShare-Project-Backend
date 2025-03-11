using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Service.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WheelShare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {

        private readonly IConfiguration config;
        private readonly IService<Payment> _service;
        public PaymentController(IConfiguration config, IService<Payment> _service)
        {
            this.config = config;
            this._service = _service;
        }
        // GET: api/<PaymentController>
        [HttpGet]
        public Task<List<Payment>> Get()
        {
            return _service.GetAll();
        }

        // GET api/<PaymentController>/5
        [HttpGet("{id}")]
        public Task<Payment> Get(int id)
        {
            return _service.GetById(id);
        }

        // POST api/<PaymentController>
        [HttpPost]
        public Task<Payment> Post([FromBody] Payment item)
        {
            return _service.Add(item);
        }

        // PUT api/<PaymentController>/5
        [HttpPut("{id}")]
        public Task<Payment> Put(int id, [FromBody] Payment item)
        {
            return _service.Update(id, item);
        }

        // DELETE api/<PaymentController>/5
        [HttpDelete("{id}")]
        public Task Delete(int id)
        {
            return _service.Delete(id);
        }
    }
}
