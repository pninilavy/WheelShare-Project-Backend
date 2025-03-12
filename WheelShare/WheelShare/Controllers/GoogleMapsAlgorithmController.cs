using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WheelShare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleMapsAlgorithmController : ControllerBase
    {
        // GET: api/<GoogleMapsAlgorithmController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<GoogleMapsAlgorithmController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<GoogleMapsAlgorithmController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<GoogleMapsAlgorithmController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<GoogleMapsAlgorithmController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
