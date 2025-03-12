using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Service.Interfaces;
using Service.Models;
using Service.NewFolder;
using Service.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WheelShare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RideController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly IService<Ride> _service;
        private readonly IGoogleMapsAlgorithm googleMapsAlgoritm;
        public RideController(IConfiguration config, IService<Ride> _service, IGoogleMapsAlgorithm googleMapsAlgoritm)
        {
            this.config = config;
            this._service = _service;
            this.googleMapsAlgoritm = googleMapsAlgoritm;
        }
        // GET: api/<RideController>
        [HttpGet]
        public Task<List<Ride>> Get()
        {
            return _service.GetAll();
        }

        // GET api/<RideController>/5
        [HttpGet("{id}")]
        public Task<Ride> Get(int id)
        {
            return _service.GetById(id);
        }

        // POST api/<RideController>
        [HttpPost]
        public Task<Ride> Post([FromBody] Ride item)
        {
            return _service.Add(item);
        }
    
        

        // PUT api/<RideController>/5
        [HttpPut("{id}")]
        public Task<Ride> Put(int id, [FromBody] Ride item)
        {
            return _service.Update(id, item);
        }

        // DELETE api/<RideController>/5
        [HttpDelete("{id}")]
        public Task Delete(int id)
        {
            return _service.Delete(id);
        }
    }
}
