using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Service.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WheelShare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleAvailabilityController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly IService<VehicleAvailability> _service;
        public VehicleAvailabilityController(IConfiguration config, IService<VehicleAvailability> _service)
        {
            this.config = config;
            this._service = _service;
        }
        // GET: api/<VehicleController>
        [HttpGet]
        public Task<List<VehicleAvailability>> Get()
        {
            return _service.GetAll();
        }

        // GET api/<VehicleController>/5
        [HttpGet("{id}")]
        public Task<VehicleAvailability> Get(int id)
        {
            return _service.GetById(id);
        }

        // POST api/<VehicleController>
        [HttpPost]
        public Task<VehicleAvailability> Post([FromBody] VehicleAvailability item)
        {
            return _service.Add(item);
        }

        // PUT api/<VehicleController>/5
        [HttpPut("{id}")]
        public Task<VehicleAvailability> Put(int id, [FromBody] VehicleAvailability item)
        {
            return _service.Update(id, item);
        }

        // DELETE api/<VehicleController>/5
        [HttpDelete("{id}")]
        public Task Delete(int id)
        {
            return _service.Delete(id);
        }

    }
}
