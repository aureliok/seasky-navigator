﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaSkyNavigator.Classes
{
    internal class WeatherService : IDisposable
    {
        private HttpClient _httpClient;
        public string StartDate;
        public string EndDate;
        public int DaysToForecast;

        public WeatherService(string startDate, int daysToForecast)
        {
            _httpClient = new HttpClient();
            DaysToForecast = daysToForecast;
            StartDate = startDate;
            EndDate = DateTime.Today.AddDays(daysToForecast).ToString("yyyy-MM-dd");
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        public async Task<string> GetWeatherData(double latitude, double longitude)
        {
            string ApiUrl = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&" +
                            $"longitude={longitude}&" +
                            $"hourly=temperature_2m," +
                            $"relative_humidity_2m," +
                            $"apparent_temperature," +
                            $"precipitation_probability&" +
                            $"timezone=auto&" +
                            $"start_date={StartDate}&" +
                            $"end_date={EndDate}";

            HttpResponseMessage responseWeather = await _httpClient.GetAsync(ApiUrl);

            if (responseWeather.IsSuccessStatusCode)
                return await responseWeather.Content.ReadAsStringAsync();
            else
                return $"status code: {responseWeather.StatusCode}";
        }
    }
}