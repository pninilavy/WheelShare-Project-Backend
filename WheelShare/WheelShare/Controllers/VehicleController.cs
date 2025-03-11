using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Service.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WheelShare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly IService<Vehicle> _service;
        public VehicleController(IConfiguration config, IService<Vehicle> _service)
        {
            this.config = config;
            this._service = _service;
        }
        // GET: api/<VehicleController>
        [HttpGet]
        public Task<List<Vehicle>> Get()
        {
            return _service.GetAll();
        }

        // GET api/<VehicleController>/5
        [HttpGet("{id}")]
        public Task<Vehicle> Get(int id)
        {
            return _service.GetById(id);
        }

        // POST api/<VehicleController>
        [HttpPost]
        public Task<Vehicle> Post([FromBody] Vehicle item)
        {
            return _service.Add(item);
        }

        // PUT api/<VehicleController>/5
        [HttpPut("{id}")]
        public Task<Vehicle> Put(int id, [FromBody] Vehicle item)
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
