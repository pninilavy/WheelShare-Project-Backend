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
using static System.Net.WebRequestMethods;

namespace Service.Services
{
    public class GoogleMapsAlgorithm : IGoogleMapsAlgorithm
    {

        private readonly IRepository<Ride> _rideRepostory;
        private readonly IRepository<RideParticipant> _rideParticipantRepository;
        private readonly HttpClient _httpClient;
        private readonly IEmailService _emailService;
        private readonly IRepository<User> _userRepository;
        private readonly IDistanceFunction distanceFunction;
        private readonly IRepository<VehicleAvailability> _vehicleAvailabilityRepository;

        public GoogleMapsAlgorithm(IRepository<Ride> _rideRepostory, IConfiguration configuration, IRepository<RideParticipant> rideParticipantRepository, IEmailService emailService, IRepository<User> _userRepository,IDistanceFunction distanceFunction, IRepository<VehicleAvailability> vehicleAvailabilityRepository)
        {
            this._rideRepostory = _rideRepostory;
            _httpClient = new HttpClient();
            _rideParticipantRepository = rideParticipantRepository;
            _emailService = emailService;
            this._userRepository = _userRepository;
            this.distanceFunction = distanceFunction;
            _vehicleAvailabilityRepository = vehicleAvailabilityRepository; 
        }


        //חישוב זמן נסיעה  בדקות בין נקודת מוצא לנקודת יעד

        public async Task<double> GetTravelTimeAsync(Coordinate origin, Coordinate destination, string mode = "driving")
        {
            

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
            Coordinate c7=new Coordinate((double)ride.SourceLatitude, (double)ride.SourceLongitude);
            Coordinate c8 = new Coordinate((double)ride.DestinationLatitude, (double)ride.DestinationLongitude);
            double ridePrice = await GetTravelTimeAsync(c7, c8);
            ridePrice *= ride.Vehicle.CostPerHour / 60;
            double rPrice = 0;

            double maxDiffrence = -1;
            foreach (Ride r in allRides)
            {
                //חישוב לנסיעה ללא שיתוף עד כתובת היעד 

                Coordinate c1 = new Coordinate((double)r.SourceLatitude, (double)r.SourceLongitude);
                Coordinate c2= new Coordinate((double)r.DestinationLatitude, (double)r.DestinationLongitude);
                rPrice = await GetTravelTimeAsync(c1,c2);
                rPrice *= r.Vehicle.CostPerHour / 60;


                //רק הנהג נוסע עד לתחנת האיסוף
                Coordinate c3 = new Coordinate((double)ride.SourceLatitude, (double)ride.SourceLongitude);
                Coordinate c4= new Coordinate((double)r.SourceLatitude, (double)r.SourceLongitude);
                part1 = await GetTravelTimeAsync(c3, c4);

                if (r.Driver != ride.Driver)
                {
                    //הנהג ride כאשר


                    //בדיקה האם תואם מבחינת זמנים חורג עד 30 דקות
                    DateTime dateRide= DateTime.Today.Add(ride.StartTime);
                    dateRide = dateRide.AddMinutes(part1);
                    DateTime dateR1 = DateTime.Today.Add(r.StartTime);
                    DateTime dateR2 = DateTime.Today.Add(r.StartTime);
                    dateR1 = dateR1.AddMinutes(30);
                    dateR2 = dateR2.AddMinutes(-30);
                    if(dateRide>=dateR2&&dateRide<=dateR1) 
                    {
                        //בדיקה האם יש מספיק מושבים ברכב
                        if (ride.NumSeats + r.NumSeats <= ride.Vehicle.Seats)
                        {
                            enoughSeats = true;
                            help = await OptimalDriver(ride, r, ridePrice, rPrice, part1);

                            //בדיקה האם ההפרש הוא המקסימלי עד כה
                            if (help?.Diffrence > maxDiffrence)
                            {
                                maxDiffrence = help.Diffrence;
                                maxDiffrenceHelp = help;
                                driver = ride;
                                partner = r;

                            }

                        }
                    }

                    //הנהג r כאשר

                    Coordinate c5=new Coordinate((double)r.SourceLatitude, (double)r.SourceLongitude);
                    Coordinate c6=new Coordinate((double)ride.SourceLatitude,  (double)ride.SourceLongitude);
                    part1 = await GetTravelTimeAsync(c5, c6);


                    //בדיקה האם תואם מבחינת זמנים חורג עד 30 דקות
                    DateTime dateDriver = DateTime.Today.Add(r.StartTime);
                    dateDriver = dateDriver.AddMinutes(part1);
                    DateTime datePartner1 = DateTime.Today.Add(ride.StartTime);
                    DateTime datePartner2 = DateTime.Today.Add(ride.StartTime);
                    datePartner1 = datePartner1.AddMinutes(30);
                    datePartner2 = datePartner2.AddMinutes(-30);
                    if (dateDriver >= datePartner2 && dateDriver <= datePartner1)
                        //בדיקה האם תואם מבחינת זמנים חורג עד 30 דקות
                    {                        if (r.StartTime.Add(TimeSpan.FromMinutes(part1)) >= ride.StartTime.Add(TimeSpan.FromMinutes(-30)) && r.StartTime.Add(TimeSpan.FromMinutes(part1)) <= ride.StartTime.Add(TimeSpan.FromMinutes(30)))

                        //בדיקה האם יש מספיק מושבים ברכב
                        if (ride.NumSeats + r.NumSeats <= r.Vehicle.Seats)
                        {
                            enoughSeats = true;
                            help = await OptimalDriver(r, ride, rPrice, ridePrice, part1);

                            //בדיקה האם ההפרש הוא המקסימלי עד כה
                            if (help?.Diffrence > maxDiffrence)
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

            if (help!=null &&driver != null && partner != null)
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
                DateTime dateRideStart = DateTime.Today.Add(driver.StartTime);
                DateTime dateRideEnd = DateTime.Today.Add(driver.EndTime);
                
                if (driver.StartTime > driver.EndTime)
                {
                    dateRideEnd = dateRideEnd.AddDays(1);
                }
                rideParticipant.DriverCost = maxDiffrenceHelp.DriverPrice + driver.Vehicle.CostPerHour * (((TimeSpan)(dateRideEnd - dateRideStart)).TotalMinutes - maxDiffrenceHelp.Duartion)/ 60;
                rideParticipant.PickUpTime = driver.StartTime.Add(TimeSpan.FromMinutes(part1));
                driver.Status = "NOTONHOLD";
                List<VehicleAvailability> lst=await _vehicleAvailabilityRepository.GetAll();
                VehicleAvailability v = lst.FirstOrDefault(x => x.VehicleId == partner.VehicleId && x.StartTime == partner.StartTime && x.EndTime == partner.EndTime);
                await _vehicleAvailabilityRepository.Delete(v.Id);
                await _rideRepostory.Delete(partner.Id);
                await _rideParticipantRepository.Add(rideParticipant);

                //שליחת מיילים לנהג ולמשתתף
                await SendEmailToPartnerAndDriver(rideParticipant);

            }
        }

        //מחזיר את ההפרש בין המחיר ללא שיתוף נסיעה לבין מחיר עם שיתוף נסיעה 
        public async Task<Help> OptimalDriver(Ride driver, Ride partner, double driverPrice, double partnerPrice, double part1)
        {
            double part2 = 0, part3 = 0;
            double temporaryDriverPrice = 0, temporaryPartnerPrice = 0;

            //מציאת המרחק בזמן בכל חלק של נסיעה

            //הנסיעה המשותפת

            Coordinate c1 = new Coordinate((double)partner.SourceLatitude,(double)partner.SourceLongitude);
            Coordinate c2=new Coordinate((double)partner.DestinationLatitude,(double)partner.DestinationLongitude);
            part2 = await GetTravelTimeAsync(c1, c2);
            //רק הנהג נוסע עד לתחנת היעד
            Coordinate c3 = new Coordinate((double)partner.DestinationLatitude, (double)partner.DestinationLongitude);
            Coordinate c4 = new Coordinate((double)driver.DestinationLatitude, (double)driver.DestinationLongitude);
            part3 = await GetTravelTimeAsync(c3, c4);


            //מחיר לנהג
            temporaryDriverPrice = driver.Vehicle.CostPerHour * (part1 + 0.5 * part2 + part3) / 60;
            //מחיר למשתתף
            temporaryPartnerPrice = driver.Vehicle.CostPerHour * 0.5 * part2 / 60;

            //בדיקה האם משתלם לנהג ולמשתתף
            if (temporaryDriverPrice < driverPrice && temporaryPartnerPrice <= partnerPrice)
            {
                Help h = new Help();
                h.Diffrence = driverPrice - temporaryDriverPrice;
                h.PartnerPrice = temporaryPartnerPrice;
                h.DriverPrice = temporaryDriverPrice;
                h.Duartion = part1 + part2 + part3;
                return h;

            }
            return null;
        }
        public async Task SendEmailToPartnerAndDriver(RideParticipant rideParticipant)
        {   //שליחת מייל לנהג
            var subject = $"🚗 נמצא לך נוסע מתאים!";

            var plainTextContent = $"🚗 נמצא לך נוסע מתאים לנסיעה השיתופית!\r\n" +
               $"📍 תחנת איסוף: {rideParticipant.PickupLocation}\r\n" +
               $"📍 תחנת הורדה: {rideParticipant.DropOffLocation}\r\n" +
               $"💰 מחיר נסיעה: {rideParticipant.DriverCost} ₪\r\n";

            var htmlContent = $@"
            <div dir='rtl' style='text-align: right; font-family: Arial, sans-serif;'>
            <h2>🚗 נמצא לך נוסע מתאים לנסיעה השיתופית!</h2>
            <p><strong>📍 תחנת איסוף:</strong> {rideParticipant.PickupLocation}</p>
            <p><strong>📍 תחנת הורדה:</strong> {rideParticipant.DropOffLocation}</p>
            <p><strong>💰 מחיר נסיעה:</strong> {rideParticipant.DriverCost} ₪</p>          
            <p>נסיעה טובה, צוות WheelShare! 🚀</p>
            </div>";

            Ride ride = await _rideRepostory.GetById(rideParticipant.RideId);
            User driver= await _userRepository.GetById(ride.DriveId);
            await _emailService.SendEmailAsync(driver.Email, subject, plainTextContent, htmlContent);
            //שליחת מייל למשתתף
            var subject1 = $"🚗 נמצא לך נהג מתאים!";

            var plainTextContent1 = $"🚗 נמצא לך נהג מתאים לנסיעה השיתופית!\r\n" +
                $"📍 תחנת איסוף: {rideParticipant.PickupLocation}\r\n" +
                $"📍 תחנת הורדה: {rideParticipant.DropOffLocation}\r\n" +
                $"🕒 שעת איסוף: {rideParticipant.PickUpTime}\r\n" +
                $"💰 מחיר נסיעה: {rideParticipant.ShareCost} ₪\r\n";

            var htmlContent1 = $@"
                <div dir='rtl' style='text-align: right; font-family: Arial, sans-serif;'>
                <h2>🚗 נמצא לך נהג מתאים לנסיעה השיתופית!</h2>
                <p><strong>📍 תחנת איסוף:</strong> {rideParticipant.PickupLocation}</p>
                <p><strong>📍 תחנת הורדה:</strong> {rideParticipant.DropOffLocation}</p>
                <p><strong>🕒 שעת איסוף:</strong> {rideParticipant.PickUpTime}</p>
                <p><strong>💰 מחיר נסיעה:</strong> {rideParticipant.ShareCost} ₪</p>
                <p>נסיעה טובה, צוות WheelShare! 🚀</p>
                </div>";
            User partner = await _userRepository.GetById(rideParticipant.UserId);
            await _emailService.SendEmailAsync(partner.Email, subject1, plainTextContent1, htmlContent1);
        }

    }
}







