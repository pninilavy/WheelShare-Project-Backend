using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Mock;
using Service.NewFolder;

namespace Service.Services
{
    public class GoogleMapsAlgoritm
    {
        private readonly WheelShareContext wheelShareContext;

        public GoogleMapsAlgoritm(WheelShareContext wheelShareContext)
        {
            this.wheelShareContext = wheelShareContext;
        }


        //קבלת הכתובת שנשלחה כקורדינאטות של 
        //(X,Y)
        public async Task<Coordinates> GetCoordinates(string address)
        {
            string apiKey = "AIzaSyAOx9s57058qlgwXyX2Z2toIXlE9lSQDac";
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
    }

}





    