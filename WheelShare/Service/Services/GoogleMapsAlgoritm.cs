using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Mock;
using Newtonsoft.Json.Linq;
using Service.Interfaces;
using Service.NewFolder;
using static System.Net.WebRequestMethods;

namespace Service.Services
{
    public class GoogleMapsAlgoritm : IGoogleMapsAlgorithm
    {
        private readonly string apiKey = "AIzaSyAY-WdTYSIPeRpTpYphRA6T-Nvq-agPNZ4";

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

                    return durationInSeconds/60;
                     }
                else
                {
                    throw new Exception("Error fetching data from Google Maps API.");
                }
            }
        }

        





    




    }
}





    