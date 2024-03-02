using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaSkyNavigator.Classes
{
    public class CoordinatesGetter : IDisposable
    {
        private string? ApiKey = Environment.GetEnvironmentVariable("POSITIONSTACK_API_KEY");
        private string? ApiUrl;
        HttpClient _httpClient;

        public CoordinatesGetter()
        {
            _httpClient = new HttpClient();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        public async Task<string> GetCoordinates(string address)
        {
            ApiUrl = $"http://api.positionstack.com/v1/forward?access_key={ApiKey}&query={address}";

            HttpResponseMessage responseCoordinates = await _httpClient.GetAsync(ApiUrl);

            if (responseCoordinates.IsSuccessStatusCode)
                return await responseCoordinates.Content.ReadAsStringAsync();
            else
                return $"status code: {responseCoordinates.StatusCode}";
        }
    }
}
