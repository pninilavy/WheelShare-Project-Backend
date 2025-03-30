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
        private readonly IDistanceFunction _distanceFunction;
        public FindVehicleAlgorithm(IRepository<Station> _stationRepostory, IConfiguration configuration, IRepository<VehicleAvailability> vehicleAvailabilityRepository,IDistanceFunction distanceFunction)
        {
            this._stationRepostory = _stationRepostory;
            this.apiKey = configuration["GoogleMaps:ApiKey"];
            this._VehicleAvailabilityRepository = vehicleAvailabilityRepository;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "MyOpenStreetMapApp/1.0");
            this._distanceFunction = distanceFunction;
        }




        public async Task<double> GetWalkingTimeAsync(Coordinate origin, Coordinate destination)
        {
            Console.WriteLine($"Origin: {origin.Latitude}, {origin.Longitude}");
            Console.WriteLine($"Destination: {destination.Latitude}, {destination.Longitude}");

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



   
        public async Task<Vehicle> GetCar(Ride ride)
        {
            Station station = new Station();
            List<Station> stations = await _stationRepostory.GetAll();
            List<StationAndDuration> optionalStations = new List<StationAndDuration>();
            double duration = 0;
            //הכנסת כל התחנות הקרובות
            foreach (Station s in stations)
            {
                Coordinate c1 = new Coordinate((double)ride.SourceLatitude,(double)ride.SourceLongitude);
                Coordinate c2 = new Coordinate((double)s.Latitude, (double)s.Longitude);
                duration = await GetWalkingTimeAsync(c1,c2);
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
                        DateTime dateRideStart = DateTime.Today.Add(ride.StartTime);
                        DateTime dateRideEnd = DateTime.Today.Add(ride.EndTime);
                        DateTime dateVStart = DateTime.Today.Add(v2.StartTime);
                        DateTime dateVEnd = DateTime.Today.Add(v2.EndTime);
                        if(ride.StartTime>ride.EndTime)
                            dateRideEnd=dateRideEnd.AddDays(1);
                        if(v2.StartTime>v2.EndTime)
                            dateVEnd=dateVEnd.AddDays(1);
                        if ((dateRideStart >= dateVStart && dateRideStart <= dateVEnd) ||
                              (dateRideEnd >= dateVStart && dateRideEnd <= dateVEnd)||
                              (dateRideStart < dateVStart) && (dateRideEnd > dateVEnd) 
                              || (dateVStart < dateRideStart) && (dateVEnd > dateRideEnd))
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
