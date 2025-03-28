using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Service.Interfaces;

namespace Service.Models
{
    public class DistanceFunction:IDistanceFunction
    {
        private static readonly HttpClient _httpClient = new HttpClient();
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
                    return new Coordinate(double.Parse(results[0].lat, CultureInfo.InvariantCulture), double.Parse(results[0].lon, CultureInfo.InvariantCulture));

                }

                throw new Exception($"לא נמצאו קואורדינטות עבור הכתובת: {address}");
            }
        }

    }
}
