using Common.Dto.Entities;
using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Service.Interfaces;
using Service.Models;
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
        private readonly IFindVehicleAlgorithm findVehicleAlgorithm;
        private readonly IEmailService emailService;
        private readonly IUserService<User> userService;
        private readonly IService<Station> stationService;
        private readonly IDistanceFunction distanceFunction;
        public RideController(IConfiguration config, IService<Ride> _service, IGoogleMapsAlgorithm googleMapsAlgoritm, IFindVehicleAlgorithm findVehicleAlgorithm, IEmailService emailService, IUserService<User> userService, IService<Station> stationService, IDistanceFunction distanceFunction)
        {
            this.config = config;
            this._service = _service;
            this.googleMapsAlgoritm = googleMapsAlgoritm;
            this.findVehicleAlgorithm = findVehicleAlgorithm;
            this.emailService = emailService;
            this.userService = userService;
            this.stationService = stationService;
            this.distanceFunction = distanceFunction;
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
        public async Task<RideDto> Post([FromBody] Ride item)

        {
            Coordinate source = await distanceFunction.GetCoordinatesAsync(item.SourceAddress);
            item.SourceLatitude = source.Latitude;
            item.SourceLongitude = source.Longitude;
            Coordinate distance = await distanceFunction.GetCoordinatesAsync(item.DestinationAddress);
            item.DestinationLatitude = distance.Latitude;
            item.DestinationLongitude = distance.Longitude;
            Vehicle v = await findVehicleAlgorithm.GetCar(item);
            if (v != null) {
                item.VehicleId = v.Id;
                if (v.VehicleAvailabilities == null)
                {
                    v.VehicleAvailabilities = new List<VehicleAvailability>();
                }
                v.VehicleAvailabilities.Add(new VehicleAvailability(v.Id,item.Date,item.StartTime,item.EndTime));
                item.SourceStationID = v.StationID;
                item.TotalCost = v.CostPerHour * (item.EndTime.TotalMinutes-item.StartTime.TotalMinutes)/60;
                

                await _service.Add(item);
                RideDto r=new RideDto(item);
                var subject = "אישור הזמנת רכב – WheelShare 🚗";
                var plainTextContent = $"📅 תאריך הנסיעה: {item.Date}\r\n" +
                       $"🕒 שעות הנסיעה: {item.StartTime} - {item.EndTime}\r\n" +
                       $"📍 תחנת מוצא: {item.SourceStationID}\r\n" +
                       $"💰 עלות: {item.TotalCost} ₪";
                Station s = await stationService.GetById(v.StationID);
                var htmlContent = $@"
                                <div dir='rtl' style='text-align: right; font-family: Arial, sans-serif;'>
                                <h2>🚗 הזמנת הנסיעה שלך נקלטה בהצלחה!</h2>
                                <p><strong>📍 תחנת מוצא:</strong> {s.Address+ " "+s.City}</p>
                                <p><strong>📅 תאריך:</strong> {item.Date:dd/MM/yyyy}</p>
                                <p><strong>🕒 שעות:</strong> {item.EndTime} - {item.StartTime}</p>
                                <p><strong>💰 עלות:</strong> {item.TotalCost} ₪</p>
                                <p>נסיעה טובה! צוות <strong>WheelShare</strong> 🚀</p>
                                </div>";


                User driver =await userService.GetById(item.DriveId);  


                await emailService.SendEmailAsync(driver.Email, subject, plainTextContent, htmlContent);
                if (item.SharedRide)
                {
                   await googleMapsAlgoritm.OptimalPlaceMent(item);
                }

                return r;
            }
            
            return null;
          
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
