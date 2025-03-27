using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
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

        private readonly IRepository<Ride> _rideRepostory;
        private readonly IRepository<RideParticipant> _rideParticipantRepository;
        private readonly HttpClient _httpClient;

        public GoogleMapsAlgorithm(IRepository<Ride> _rideRepostory, IConfiguration configuration, IRepository<RideParticipant> rideParticipantRepository)
        {
            this._rideRepostory = _rideRepostory;         
            _httpClient = new HttpClient();
            _rideParticipantRepository = rideParticipantRepository; 
        }


        //חישוב זמן נסיעה  בדקות בין נקודת מוצא לנקודת יעד

        public async Task<double> GetTravelTimeAsync(string originAddress, string destinationAddress, string mode = "driving")
        {
            // קבלת קואורדינטות עבור הכתובות
            Coordinate origin = await GetCoordinatesAsync(originAddress);
            Coordinate destination = await GetCoordinatesAsync(destinationAddress);

            // בניית URL לשירות OSRM – הפורמט תלוי בפרופיל הנסיעה (driving, walking, cycling וכו')
            string osrmUrl = $"http://router.project-osrm.org/route/v1/{mode}/" +
                             $"{origin.Longitude.ToString(CultureInfo.InvariantCulture)},{origin.Latitude.ToString(CultureInfo.InvariantCulture)};" +
                             $"{destination.Longitude.ToString(CultureInfo.InvariantCulture)},{destination.Latitude.ToString(CultureInfo.InvariantCulture)}" +
            "?overview=false";

            HttpResponseMessage response = await _httpClient.GetAsync(osrmUrl);
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();

            // דסיריומל של התגובה תוך התעלמות מרגישות לאותיות
            OsrmResponse osrmResponse = JsonSerializer.Deserialize<OsrmResponse>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (osrmResponse != null && osrmResponse.Routes != null && osrmResponse.Routes.Length > 0)
            {
                double durationInSeconds = osrmResponse.Routes[0].Duration;
                return durationInSeconds / 60; // המרה לדקות
            }

            throw new Exception("לא ניתן לחשב את זמן הנסיעה בין הכתובות. תגובת OSRM: " + json);
        }
        public async Task<Coordinate> GetCoordinatesAsync(string address)
        {
            string url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(address)}&format=json&limit=1";

            // הוספת User-Agent לבקשה
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                request.Headers.Add("User-Agent", "WheelShareApp/1.0"); // שימי כאן שם מזהה ליישום שלך
                HttpResponseMessage response = await _httpClient.SendAsync(request);

                // בדיקת תקינות התגובה
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"שגיאה בקבלת קואורדינטות: {response.StatusCode}");
                }

                string json = await response.Content.ReadAsStringAsync();

                // בדיקה אם JSON ריק
                if (string.IsNullOrWhiteSpace(json))
                {
                    throw new Exception("שגיאה: קיבלנו JSON ריק מ-OSM.");
                }

                List<NominatimResult> results = JsonSerializer.Deserialize<List<NominatimResult>>(json);
                if (results != null && results.Count > 0)
                {
                    return new Coordinate
                    {
                        Latitude = double.Parse(results[0].lat, CultureInfo.InvariantCulture),
                        Longitude = double.Parse(results[0].lon, CultureInfo.InvariantCulture)
                    };
                }

                throw new Exception($"לא נמצאו קואורדינטות עבור הכתובת: {address}");
            }
        }



        public async Task OptimalPlaceMent(Ride ride)
        {
            //סינון וקבלת הנסיעות הממתינות המעונינות בשיתוף ותואמות מין  
            List<Ride> allRides = await _rideRepostory.GetAll();           
            allRides = allRides.Where(x => x.SharedRide && x.Status == "ONHOLD" && x.Driver.Gender == ride.Driver.Gender).ToList();
            double finalDriverPrice = 0, finalPartnerPrice = 0;
            double part1 = 0;
            Help help = null, maxDiffrenceHelp = null;
            Ride driver = null, partner = null;
            bool enoughSeats = false;
            //חישוב לנסיעה הנוכחית ללא שיתוף עד כתובת היעד
            double ridePrice = await GetTravelTimeAsync(ride.SourceStation.Address+ " "+ ride.SourceStation.City, ride.DestinationAddress);
            ridePrice *= ride.Vehicle.CostPerHour/60;
            double rPrice = 0;

            double maxDiffrence = -1;
            foreach (Ride r in allRides)
            {
                //חישוב לנסיעה ללא שיתוף עד כתובת היעד 
                rPrice = await GetTravelTimeAsync(r.SourceStation.Address+" "+r.SourceStation.City, r.DestinationAddress);
                rPrice *= r.Vehicle.CostPerHour/60;

                
                //רק הנהג נוסע עד לתחנת האיסוף
                part1 = await GetTravelTimeAsync(ride.SourceStation.Address+" "+ride.SourceStation.City, r.SourceAddress);

                if (r.Driver != ride.Driver)
                {
                    //הנהג ride כאשר


                    //בדיקה האם תואם מבחינת זמנים חורג עד 30 דקות
                    if (ride.StartTime.Add(TimeSpan.FromMinutes(part1)) >= r.StartTime.Add(TimeSpan.FromMinutes(-30)) && ride.StartTime.Add(TimeSpan.FromMinutes(part1)) <= r.StartTime.Add(TimeSpan.FromMinutes(30)))
                    {
                        //בדיקה האם יש מספיק מושבים ברכב
                        if (ride.NumSeats + r.NumSeats <= ride.Vehicle.Seats)
                        {
                            enoughSeats = true;
                            help = await OptimalDriver(ride, r, ridePrice, rPrice, part1);

                            //בדיקה האם ההפרש הוא המקסימלי עד כה
                            if (help.Diffrence > maxDiffrence)
                            {
                                maxDiffrence = help.Diffrence;
                                maxDiffrenceHelp = help;
                                driver = ride;
                                partner = r;

                            }

                        }
                    }

                    //הנהג r כאשר
                    part1 = await GetTravelTimeAsync(r.SourceStation.Address, ride.SourceAddress);

                    //בדיקה האם תואם מבחינת זמנים חורג עד 30 דקות
                    if (r.StartTime.Add(TimeSpan.FromMinutes(part1)) >= ride.StartTime.Add(TimeSpan.FromMinutes(-30)) && r.StartTime.Add(TimeSpan.FromMinutes(part1)) <= ride.StartTime.Add(TimeSpan.FromMinutes(30)))
                    {
                        //בדיקה האם יש מספיק מושבים ברכב
                        if (ride.NumSeats + r.NumSeats <= r.Vehicle.Seats)
                        {
                            enoughSeats = true;
                            help = await OptimalDriver(r, ride, rPrice, ridePrice, part1);

                            //בדיקה האם ההפרש הוא המקסימלי עד כה
                            if (help.Diffrence > maxDiffrence)
                            {
                                maxDiffrence = help.Diffrence;
                                maxDiffrenceHelp = help;
                                driver = r;
                                partner = ride;

                            }

                        }
                    }



                }
            }

            if (driver != null && partner != null)
            {
                //הוספת הנסיעה השיתופית כאשר הסטטוס הוא בהמתנה לאישור
                RideParticipant rideParticipant = new RideParticipant();
                rideParticipant.Ride = driver;
                rideParticipant.RideId = driver.Id;
                rideParticipant.UserId = partner.Id;
                rideParticipant.User = partner.Driver;
                rideParticipant.PickupLocation = partner.SourceAddress;
                rideParticipant.DropOffLocation = partner.DestinationAddress;
                rideParticipant.ShareCost = maxDiffrenceHelp.PartnerPrice;
                rideParticipant.Status = "ONHOLD";
                rideParticipant.DriverCost= maxDiffrenceHelp.DriverPrice + driver.Vehicle.CostPerHour * ((driver.EndTime.TotalMinutes - driver.StartTime.TotalMinutes - help.Duartion)/60);                             
                _rideParticipantRepository.Add(rideParticipant);
                //שליחת מיילים לנהג ולמשתתף

            }
        }

        //מחזיר את ההפרש בין המחיר ללא שיתוף נסיעה לבין מחיר עם שיתוף נסיעה 
        public async Task<Help> OptimalDriver(Ride driver, Ride partner, double driverPrice, double partnerPrice,double part1)
        {
            double part2 = 0, part3 = 0;
            double temporaryDriverPrice = 0, temporaryPartnerPrice = 0;

            //מציאת המרחק בזמן בכל חלק של נסיעה

            //הנסיעה המשותפת
            part2 = await GetTravelTimeAsync(partner.SourceAddress, partner.DestinationAddress);
            //רק הנהג נוסע עד לתחנת היעד
            part3 = await GetTravelTimeAsync(partner.DestinationAddress, driver.DestinationAddress);


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
                h.Duartion=part1+part2 + part3;
                return h;
                
            }
            return null;
        }
        public async Task SendEmailToPartnerAndDriver(RideParticipant rideParticipant)
        {   //שליחת מייל לנהג
            var plainTextContent = $"🚗 נמצאה התאמה לנסיעה שיתופית!\r\n" +
                          $"📍 תחנת איסוף: {rideParticipant.PickupLocation}\r\n" +
                          $"📍 תחנת הורדה: {rideParticipant.DropOffLocation}\r\n" +
                          $"💰 מחיר נסיעה שיתופית: {rideParticipant.DriverCost} ₪\r\n";

            var htmlContent = $@"
            <div dir='rtl' style='text-align: right; font-family: Arial, sans-serif;'>
            <h2>🚗 נמצאה התאמה לנסיעה שיתופית!</h2>
            <p><strong>📍 תחנת איסוף:</strong> {rideParticipant.PickupLocation}</p>
            <p><strong>📍 תחנת הורדה:</strong> {rideParticipant.DropOffLocation}</p>
            <p><strong>💰 מחיר נסיעה שיתופית:</strong> {rideParticipant.DriverCost} ₪</p>
            <p>נא לאשר או לדחות את ההזמנה:</p>
            <button style='background-color: #4CAF50; color: white; padding: 10px 20px; border: none; cursor: pointer; font-weight: bold; border-radius: 5px;'>
                ✅ אישור
            </button>
            <button style='background-color: #f44336; color: white; padding: 10px 20px; border: none; cursor: pointer; font-weight: bold; border-radius: 5px; margin-right: 10px;'>
                ❌ דחייה
            </button>
            <p>נסיעה טובה צוות ב-WheelShare! 🚀</p>
    </div>";
        }
        
    }
}
            






    