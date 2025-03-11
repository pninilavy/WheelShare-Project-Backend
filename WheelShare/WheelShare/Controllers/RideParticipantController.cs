using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Service.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WheelShare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RideParticipantController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly IService<RideParticipant> _service;
        public RideParticipantController(IConfiguration config, IService<RideParticipant> _service)
        {
            this.config = config;
            this._service = _service;
        }
        // GET: api/<RideParticipantController>
        [HttpGet]
        public Task<List<RideParticipant>> Get()
        {
            return _service.GetAll();
        }

        // GET api/<RideParticipantController>/5
        [HttpGet("{id}")]
        public Task<RideParticipant> Get(int id)
        {
            return _service.GetById(id);
        }

        // POST api/<RideParticipantController>
        [HttpPost]
        public Task<RideParticipant> Post([FromBody] RideParticipant item)
        {
            return _service.Add(item);
        }

        // PUT api/<RideParticipantController>/5
        [HttpPut("{id}")]
        public Task<RideParticipant> Put(int id, [FromBody] RideParticipant item)
        {
            return _service.Update(id, item);
        }

        // DELETE api/<RideParticipantController>/5
        [HttpDelete("{id}")]
        public Task Delete(int id)
        {
            return _service.Delete(id);
        }
    }
}
