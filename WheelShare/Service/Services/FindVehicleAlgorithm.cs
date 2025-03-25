using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private readonly string apiKey;
        public FindVehicleAlgorithm(IRepository<Station> _stationRepostory, IConfiguration configuration)
        {
            this._stationRepostory = _stationRepostory;
            this.apiKey = configuration["GoogleMaps:ApiKey"];
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
                duration = await WalkTimeCalculation(ride.SourceAddress, s.Address + " " + s.City);
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
                if (v1.Vehicle.VehicleAvailabilities == null)
                {
                    return v1.Vehicle;
                }
                foreach (VehicleAvailability v2 in v1.Vehicle.VehicleAvailabilities)
                {
                    if (ride.Date == v2.Date)
                    {
                        if ((ride.StartTime <= v2.StartTime && ride.StartTime <= v2.EndTime) || ride.EndTime <= v2.StartTime && ride.EndTime <= v2.EndTime)
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
        public async Task<double> WalkTimeCalculation(string origin, string destination)
        {
            string encodedOrigin = Uri.EscapeDataString(origin);
            string encodedDestination = Uri.EscapeDataString(destination);
            string url = $"https://maps.googleapis.com/maps/api/directions/json?origin={encodedOrigin}&destination={encodedDestination}&mode=walking&key={apiKey}";

            int durationInMinutes = -1; // ברירת מחדל

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                string jsonResponse = await response.Content.ReadAsStringAsync();
                JObject data = JObject.Parse(jsonResponse);

                if (data["status"]?.ToString() == "OK" &&
                    data["routes"]?.Any() == true &&
                    data["routes"][0]["legs"]?.Any() == true)
                {
                    int durationInSeconds = (int)data["routes"][0]["legs"][0]["duration"]["value"];
                    durationInMinutes = durationInSeconds / 60;
                    Console.WriteLine($"זמן ההליכה: {durationInMinutes} דקות");
                }
                else
                {
                    Console.WriteLine("שגיאה: לא ניתן לחשב זמן הליכה.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"שגיאה בקריאת Google Maps API: {ex.Message}");
            }

            return durationInMinutes;
        }


    }
}
