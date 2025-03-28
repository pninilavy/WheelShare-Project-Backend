using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Service.Models
{
    public class CoordinateFunction
    {
        private HttpClient _httpClient;
        public CoordinateFunction()
        {
            _httpClient = new HttpClient();
        }

        public async Task<Coordinate> GetCoordinatesAsync(string address)
        {
            string url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(address)}&format=json&limit=1";
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();

            List<NominatimResult> results = JsonSerializer.Deserialize<List<NominatimResult>>(json);
            if (results != null && results.Count > 0)
            {
                return new Coordinate(double.Parse(results[0].lat, CultureInfo.InvariantCulture), double.Parse(results[0].lon, CultureInfo.InvariantCulture));

            }
            throw new Exception("לא נמצאו קואורדינטות עבור הכתובת: " + address);
        }
    }
}
