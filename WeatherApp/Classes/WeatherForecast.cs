using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SeaSkyNavigator.Classes
{
    internal class Hourly
    {
        public List<string>? time { get; set; }
        public List<double>? temperature_2m { get; set; }
        public List<int>? relative_humidity_2m { get; set; }
        public List<double>? apparent_temperature { get; set; }
        public List<int>? precipitation_probability { get; set; }
    }

    internal class HourlyUnits
    {
        public string? time { get; set; }
        public string? temperature_2m { get; set; }
        public string? relative_humidity_2m { get; set; }
        public string? apparent_temperature { get; set; }
        public string? precipitation_probability { get; set; }
    }

    internal class WeatherForecast
    {
        public double? latitude { get; set; }
        public double? longitude { get; set; }
        public double? generationtime_ms { get; set; }
        public int? utc_offset_seconds { get; set; }
        public string? timezone { get; set; }
        public string? timezone_abbreviation { get; set; }
        public double? elevation { get; set; }
        public HourlyUnits? hourly_units { get; set; }
        public Hourly? hourly { get; set; }
    }
}
