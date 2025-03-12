using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Mock;
using Service.Interfaces;
using Service.NewFolder;
using static System.Net.WebRequestMethods;

namespace Service.Services
{
    public class GoogleMapsAlgoritm : IGoogleMapsAlgorithm
    {
        private readonly string apiKey = "AIzaSyAOx9s57058qlgwXyX2Z2toIXlE9lSQDac";

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

        //חישוב זמן נסיעה בין נקודת מוצא לנקודת יעד
        public async Task<double> TravelTimeCalculation(Coordinates origin, Coordinates destination)
        {

            string url = $"https://maps.googleapis.com/maps/api/directions/json?origin=32.0853,34.7818&destination=31.7683,35.2137&mode=driving&key=AIzaSyAOx9s57058qlgwXyX2Z2toIXlE9lSQDac";

                /* $"https://maps.googleapis.com/maps/api/directions/json?origin={origin.X},{origin.Y}&destination={destination.X},{destination.Y}&mode=driving&key={apiKey}";*/
            
            using HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);

            
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response from Google Maps API: " + json);

                using JsonDocument doc = JsonDocument.Parse(json);
                JsonElement root = doc.RootElement;

                if (root.GetProperty("status").GetString() == "OK")
                {
                    JsonElement duration = root.GetProperty("routes")[0].GetProperty("legs")[0].GetProperty("duration");
                    double travelTimeInSeconds = duration.GetProperty("value").GetDouble();

                    // החזרת זמן הנסיעה בדקות
                    return travelTimeInSeconds / 60;
                }
            }
            return -1; // במקרה של שגיאה נחזיר ערך לא תקין





        }

    }
}





    