using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Service.Interfaces;
using Service.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WheelShare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly IService<Station> _service;
        private readonly IDistanceFunction distanceFunction;
        public StationController(IConfiguration config, IService<Station> _service, IDistanceFunction distanceFunction)
        {
            this.config = config;
            this._service = _service;
            this.distanceFunction= distanceFunction;
        }
        // GET: api/<StationController>
        [HttpGet]
        public Task<List<Station>> Get()
        {
            return _service.GetAll();
        }

        // GET api/<StationController>/5
        [HttpGet("{id}")]
        public Task<Station> Get(int id)
        {
            return _service.GetById(id);
        }

        // POST api/<StationController>
        [HttpPost]
        public async Task<Station> Post([FromBody] Station item)
        {
            Coordinate coordinate =await distanceFunction.GetCoordinatesAsync(item.Address + " " + item.City);
            item.Latitude = coordinate.Latitude;
            item.Longitude = coordinate.Longitude;
            return await _service.Add(item);
        }

        // PUT api/<StationController>/5
        [HttpPut("{id}")]
        public Task<Station> Put(int id, [FromBody] Station item)
        {
            return _service.Update(id, item);
        }

        // DELETE api/<StationController>/5
        [HttpDelete("{id}")]
        public Task Delete(int id)
        {
            return _service.Delete(id);
        }
    }
}
