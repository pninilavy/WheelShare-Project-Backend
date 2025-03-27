using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Models;

namespace Service.Services
{
  
   
    public class FindVehicleAlgorithm:IFindVehicleAlgorithm
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly IRepository<Station> _stationRepostory;
        private readonly IRepository<VehicleAvailability> _VehicleAvailabilityRepository;
        private readonly string apiKey;
        private readonly HttpClient _httpClient;


        public async Task<Coordinate> GetCoordinatesAsync(string address)
        {
            string url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(address)}&format=json&limit=1";
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();

            List<NominatimResult> results = JsonSerializer.Deserialize<List<NominatimResult>>(json);
            if (results != null && results.Count > 0)
            {
                return new Coordinate
                {
                    Latitude = double.Parse(results[0].lat, CultureInfo.InvariantCulture),
                    Longitude = double.Parse(results[0].lon, CultureInfo.InvariantCulture)
                };
            }
            throw new Exception("לא נמצאו קואורדינטות עבור הכתובת: " + address);
        }

        public async Task<double> GetWalkingTimeAsync(string originAddress, string destinationAddress)
        {
           
            Coordinate origin = await GetCoordinatesAsync(originAddress);
            Coordinate destination = await GetCoordinatesAsync(destinationAddress);

        
            string osrmUrl = $"http://router.project-osrm.org/route/v1/walking/" +
                             $"{origin.Longitude.ToString(CultureInfo.InvariantCulture)},{origin.Latitude.ToString(CultureInfo.InvariantCulture)};" +
                             $"{destination.Longitude.ToString(CultureInfo.InvariantCulture)},{destination.Latitude.ToString(CultureInfo.InvariantCulture)}?overview=false";

            HttpResponseMessage response = await _httpClient.GetAsync(osrmUrl);
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();

        
            OsrmResponse osrmResponse = JsonSerializer.Deserialize<OsrmResponse>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (osrmResponse != null && osrmResponse.Routes != null && osrmResponse.Routes.Length > 0)
            {
         
                double durationInSeconds = osrmResponse.Routes[0].Duration;
                return durationInSeconds / 60;
            }
            throw new Exception("לא ניתן לחשב את זמן ההליכה בין הכתובות. תגובת OSRM: " + json);
        }



        public FindVehicleAlgorithm(IRepository<Station> _stationRepostory, IConfiguration configuration, IRepository<VehicleAvailability> vehicleAvailabilityRepository)
        {
            this._stationRepostory = _stationRepostory;
            this.apiKey = configuration["GoogleMaps:ApiKey"];
            this._VehicleAvailabilityRepository = vehicleAvailabilityRepository;
            _httpClient = new HttpClient();
           
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "MyOpenStreetMapApp/1.0");
        }

        public async Task<Vehicle> GetCar(Ride ride)
        {
            Station station = new Station();
            List<Station> stations = await _stationRepostory.GetAll();
            List<StationAndDuration> optionalStations = new List<StationAndDuration>();
            double duration = 0;
            //הכנסת כל התחנות הקרובות
            foreach (Station s in stations)
            {
                duration = await GetWalkingTimeAsync(ride.SourceAddress, s.Address + " " + s.City);
                if (duration <= 10)
                {
                    optionalStations.Add(new StationAndDuration(s, duration));
                }
            }
            List<VehicleAndDuration> optionalVehicles = new List<VehicleAndDuration>();
            foreach (StationAndDuration s in optionalStations)
            {
                foreach (Vehicle v in s.Station.Vehicles)
                {
                    if (v.Seats >= ride.NumSeats)
                    {
               
                        
                        optionalVehicles.Add(new VehicleAndDuration(v, s.Duration));
                    }

                }
            }
            optionalVehicles = optionalVehicles.OrderBy(v => v.Vehicle.Seats).ThenBy(v => v.Duration).ToList();
            
            foreach (VehicleAndDuration v1 in optionalVehicles)
            {
                
                bool flag = true;
             
                v1.Vehicle.VehicleAvailabilities =await GetVehicleAvailabilities(v1.Vehicle);
                foreach (VehicleAvailability v2 in v1.Vehicle.VehicleAvailabilities)
                {
                    if (ride.Date == v2.Date)
                    {
                        if ((ride.StartTime >= v2.StartTime && ride.StartTime <= v2.EndTime) || (ride.EndTime >= v2.StartTime && ride.EndTime <= v2.EndTime))
                        {
                            flag = false;
                            break;
                        }
                    }                               
                }
                if(flag)
                {
                    return v1.Vehicle;
                }

            }

            return null;
        }


        public async Task<List<VehicleAvailability>> GetVehicleAvailabilities(Vehicle vehicle)
        {
            List<VehicleAvailability> vehicleAvailabilities = await _VehicleAvailabilityRepository.GetAll();
            return vehicleAvailabilities.Where(va=>va.VehicleId==vehicle.Id).ToList();
        }

       
    }
}
