using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Models;
using Service.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WheelShare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleMapsAlgorithmController : ControllerBase
    {


        private readonly IGoogleMapsAlgorithm _googleMapsAlgorithm;
        public GoogleMapsAlgorithmController(IGoogleMapsAlgorithm _googleMapsAlgorithm)
        {
            this._googleMapsAlgorithm = _googleMapsAlgorithm;
        }

        // POST api/<GoogleMapsAlgorithmController>
        [HttpPost]
        public async Task<double> Post([FromBody] TravelRequest travelRequest)
        {
            return await _googleMapsAlgorithm.TravelTimeCalculation(travelRequest.Origin, travelRequest.Destination);
        }


    }
}
