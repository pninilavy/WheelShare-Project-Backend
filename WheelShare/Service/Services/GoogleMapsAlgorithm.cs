using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Mock;
using Newtonsoft.Json.Linq;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Models;
using Service.NewFolder;
using static System.Net.WebRequestMethods;

namespace Service.Services
{
    public class GoogleMapsAlgorithm : IGoogleMapsAlgorithm
    {
        private static readonly HttpClient client;
        private readonly string apiKey = "AIzaSyAY-WdTYSIPeRpTpYphRA6T-Nvq-agPNZ4";
        private readonly IRepository<Ride> _rideRepostory;
        public GoogleMapsAlgorithm(IRepository<Ride> _rideRepostory, IConfiguration configuration)
        {
            this._rideRepostory = _rideRepostory;
            this.apiKey = configuration["GoogleMaps:ApiKey"];
          
        }
        //קבלת הכתובת שנשלחה כקורדינאטות של 
        //(X,Y)
        public async Task<Coordinates> GetCoordinates(string address)
        {

            string url = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(address)}&key={apiKey}";
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                using JsonDocument doc = JsonDocument.Parse(json);
                JsonElement root = doc.RootElement;

                if (root.GetProperty("status").GetString() == "OK")
                {
                    JsonElement location = root.GetProperty("results")[0].GetProperty("geometry").GetProperty("location");
                    double x = location.GetProperty("lat").GetDouble();
                    double y = location.GetProperty("lng").GetDouble();
                    return new Coordinates(x, y);
                }
            }
            return null;
        }

        //חישוב זמן נסיעה  בדקות בין נקודת מוצא לנקודת יעד

        public async Task<double> TravelTimeCalculation(string origin, string destination)
        {
            string url = $"https://maps.googleapis.com/maps/api/directions/json?origin={origin}&destination={destination}&key={apiKey}";

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode(); // יוודא שהסטטוס קוד של התגובה הוא Success (2xx)

                string responseBody = await response.Content.ReadAsStringAsync();
                JObject jsonResponse = JObject.Parse(responseBody);

                var durationInSeconds = (double)jsonResponse["routes"]?[0]?["legs"]?[0]?["duration"]?["value"];

                if (durationInSeconds == 0)
                {
                    throw new Exception("Duration value is missing or zero.");
                }

                return durationInSeconds / 60; // מחזיר את הזמן בדקות
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching data from Google Maps API: {ex.Message}");
            }
        }


        public async Task OptimalPlaceMent(Ride ride)
        {
            //List<Ride> allRides = await _rideRepostory.GetAll();
            //double finalDriverPrice = 0, finalPartnerPrice = 0;
            //double part1 = 0;
            //Help help = null, maxDiffrenceHelp = null;
            //Ride driver = null, partner = null;
            //bool enoughSeats = false;
            //double ridePrice = await TravelTimeCalculation(ride.SourceStation.Address, ride.DestinationStation.Address);
            //ridePrice *= ride.Vehicle.CostPerHour;
            //double rPrice = 0;
            ////סינון וקבלת הנסיעות הממתינות המעונינות בשיתוף 
            //allRides = allRides.Where(x => !x.SharedRide && x.Status == "ONHOLD").ToList();
            //foreach (Ride r in allRides)
            //{

            //    rPrice = await TravelTimeCalculation(r.SourceStation.Address, r.DestinationStation.Address);
            //    rPrice *= r.Vehicle.CostPerHour;

            //    double maxDiffrence = -1;
            //    //רק הנהג נוסע עד לתחנת האיסוף
            //    part1 = await TravelTimeCalculation(ride.SourceStation.Address, r.SourceStation.Address);

            //    if (r.Driver != ride.Driver)
            //    {
            //        //הנהג ride כאשר


            //        //בדיקה האם תואם מבחינת זמנים חורג עד 30 דקות
            //        if (ride.StartTime.Add(TimeSpan.FromMinutes(part1)) >= r.StartTime.Add(TimeSpan.FromMinutes(-30) ) && ride.StartTime.Add(TimeSpan.FromMinutes(part1)) <= r.StartTime.Add(TimeSpan.FromMinutes(30)))
            //        {
            //            //בדיקה האם יש מספיק מושבים ברכב
            //            if (ride.NumSeats + r.NumSeats <= ride.Vehicle.Seats)
            //            {
            //                enoughSeats = true;
            //                help = await OptimalDriver(ride, r, ridePrice, rPrice, part1);

            //                //בדיקה האם ההפרש הוא המקסימלי עד כה
            //                if (help.Diffrence > maxDiffrence)
            //                {
            //                    maxDiffrence = help.Diffrence;
            //                    maxDiffrenceHelp = help;
            //                    driver = ride;
            //                    partner = r;

            //                }

            //            }
            //        }

            //        //הנהג r כאשר
            //        part1 = await TravelTimeCalculation(r.SourceStation.Address, ride.SourceStation.Address);

            //        //בדיקה האם תואם מבחינת זמנים חורג עד 30 דקות
            //        if (r.StartTime.Add(TimeSpan.FromMinutes(part1)) >= ride.StartTime.Add(TimeSpan.FromMinutes(-30) ) && r.StartTime.Add(TimeSpan.FromMinutes(part1)) <= ride.StartTime.Add(TimeSpan.FromMinutes(30)))
            //        {
            //            //בדיקה האם יש מספיק מושבים ברכב
            //            if (ride.NumSeats + r.NumSeats <= r.Vehicle.Seats)
            //            {
            //                enoughSeats = true;
            //                help = await OptimalDriver(r, ride, rPrice, ridePrice, part1);

            //                //בדיקה האם ההפרש הוא המקסימלי עד כה
            //                if (help.Diffrence > maxDiffrence)
            //                {
            //                    maxDiffrence = help.Diffrence;
            //                    maxDiffrenceHelp = help;
            //                    driver = r;
            //                    partner = ride;

            //                }

            //            }
            //        }



            //    }
            //}

            //if (driver != null && partner != null)
            //{
            //    //רק אם יש אישור במייל
            //    RideParticipant rideParticipant = new RideParticipant();
            //    rideParticipant.Ride = driver;
            //    rideParticipant.RideId = driver.Id;
            //    rideParticipant.UserId = partner.Id;
            //    rideParticipant.User = partner.Driver;
            //    rideParticipant.PickupLocation = partner.SourceStation.Address;
            //    rideParticipant.DropOffLocation = partner.DestinationStation.Address;
            //    rideParticipant.ShareCost = maxDiffrenceHelp.PartnerPrice;
            //    driver.TotalCost = maxDiffrenceHelp.DriverPrice;

            //}
        }

        //מחזיר את ההפרש בין המחיר ללא שיתוף נסיעה לבין מחיר עם שיתוף נסיעה 
        public async Task<Help> OptimalDriver(Ride driver, Ride partner, double driverPrice, double partnerPrice,double part1)
        {
            double part2 = 0, part3 = 0;
            double temporaryDriverPrice = 0, temporaryPartnerPrice = 0;

            //מציאת המרחק בזמן בכל חלק של נסיעה

            //הנסיעה המשותפת
            part2 = await TravelTimeCalculation(partner.SourceStation.Address, partner.DestinationStation.Address);
            //רק הנהג נוסע עד לתחנת היעד
            part3 = await TravelTimeCalculation(partner.DestinationStation.Address, driver.DestinationStation.Address);


            //מחיר לנהג
            temporaryDriverPrice = driver.Vehicle.CostPerHour * (part1 + 0.5 * part2 + part3) / 60;
            //מחיר למשתתף
            temporaryPartnerPrice = driver.Vehicle.CostPerHour * 0.5 * part2 / 60;

            //בדיקה האם משתלם לנהג ולמשתתף
            if (temporaryDriverPrice < driverPrice && temporaryPartnerPrice <= partnerPrice)
            {
                Help h=new Help();
                h.Diffrence = driverPrice - temporaryDriverPrice;
                h.PartnerPrice = temporaryPartnerPrice;
                h.DriverPrice=temporaryDriverPrice;
                return h;
                
            }
            return null;
        }
    }
}
            






    