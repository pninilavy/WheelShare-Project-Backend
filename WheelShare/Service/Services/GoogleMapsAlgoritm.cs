using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Mock;
using Newtonsoft.Json.Linq;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using Service.NewFolder;
using static System.Net.WebRequestMethods;

namespace Service.Services
{
    public class GoogleMapsAlgoritm : IGoogleMapsAlgorithm
    {
        private readonly string apiKey = "AIzaSyAY-WdTYSIPeRpTpYphRA6T-Nvq-agPNZ4";
        private readonly IRepository<Ride> _repository;
        //קבלת הכתובת שנשלחה כקורדינאטות של 
        //(X,Y)
        public async Task<Coordinates> GetCoordinates(string address)
        {

            string url = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(address)}&key={apiKey}";
            using HttpClient client = new HttpClient();
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

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                string responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {

                    JObject jsonResponse = JObject.Parse(responseBody);
                    var durationInSeconds = (double)jsonResponse["routes"][0]["legs"][0]["duration"]["value"];

                    return durationInSeconds / 60;
                }
                else
                {
                    throw new Exception("Error fetching data from Google Maps API.");
                }
            }
        }

        public async Task OptimalPlaceMent(Ride ride)
        {
            List<Ride> allRides = await _repository.GetAll();
            allRides = allRides.Where(x => !x.IsPrivateRide).ToList();
            Ride optimalRide = null;
            double ridePrice = await TravelTimeCalculation(ride.SourceStation.Address, ride.DestinationStation.Address);
            double driverPrice1, notDriver1, driverPrice2, notDriver2, rPrice;
            double finalDriverPrice=0, finalNotDriverPrice=0;
            Ride driverRide = null, notDriverRide = null;
            double maxDiffrence = -1;
            double part1, part2, part3;
            foreach (Ride r in allRides)
            {
                if (r.Driver != ride.Driver)//רק אם זה לא אותה נסיעה
                {
                   
                    rPrice = await TravelTimeCalculation(r.SourceStation.Address, r.DestinationStation.Address);
                    //כאשר הנסיעה ride הוא הנהג

                    
                    part1 = await TravelTimeCalculation(ride.SourceStation.Address, r.SourceStation.Address);
                    part2 = await TravelTimeCalculation(r.SourceStation.Address, r.DestinationStation.Address);
                    part3 = await TravelTimeCalculation(r.DestinationStation.Address, ride.DestinationStation.Address);

                    //מחיר לנהג
                    driverPrice1 = part1 + 0.5 * part2 + part3;
                    //מחיר למשתתף
                    notDriver1 = 0.5 * part2;

                    //אם הפרש המחיר לנהג גדול מההפרש עד כה
                    //נשמור נסיעות אלו
                    if (driverPrice1 < ridePrice && notDriver1 <= rPrice)
                    {
                        if (ridePrice - driverPrice1 > maxDiffrence)
                        {
                            maxDiffrence = ridePrice - driverPrice1;
                            driverRide = ride;
                            notDriverRide = r;
                            finalDriverPrice = driverPrice1;
                            finalNotDriverPrice = notDriver1;
                        }
                    }
                    //כאשר הנסיעה r הוא הנהג
                    part1 = await TravelTimeCalculation(r.SourceStation.Address, ride.SourceStation.Address);
                    part2 = await TravelTimeCalculation(ride.SourceStation.Address, ride.DestinationStation.Address);
                    part3 = await TravelTimeCalculation(ride.DestinationStation.Address, r.DestinationStation.Address);

                    //מחיר לנהג
                    driverPrice2 = part1 + 0.5 * part2 + part3;
                    //מחיר למשתתף
                    notDriver2 = 0.5 * part2;

                    if (driverPrice2 < rPrice && notDriver2 <= ridePrice)
                    {
                        if (rPrice - driverPrice2 > maxDiffrence)
                        {
                            maxDiffrence = rPrice - driverPrice2;
                            driverRide = r;
                            notDriverRide = ride;
                            finalDriverPrice = driverPrice2;
                            finalNotDriverPrice = notDriver2;
                        }
                    }

                }

               
                
            }
            if(driverRide!=null&&notDriverRide!=null)
            {
                RideParticipant rideParticipant = new RideParticipant();
                rideParticipant.Ride= driverRide;
                rideParticipant.RideId = driverRide.Id;
                rideParticipant.UserId = notDriverRide.Id;
                rideParticipant.User = notDriverRide.Driver;
                rideParticipant.PickupLocation = notDriverRide.SourceStation.Address;
                rideParticipant.DropOffLocation=notDriverRide.DestinationStation.Address;
                rideParticipant.ShareCost = finalNotDriverPrice;
                driverRide.TotalCost = finalDriverPrice;
                await _repository.Delete(notDriverRide.Id);

            }
            
            
        }
    










    }
}





    