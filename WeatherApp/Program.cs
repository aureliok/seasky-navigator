using System;
using System.Text.Json;
using System.Runtime.ConstrainedExecution;
using SeaSkyNavigator.Classes;
using SeaSkyNavigator.Records;

namespace SeaSkyNavigator
{
    class Program
    {

        public static async Task Main(string[] args)
        {
            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
            Hourly hourlyForecast = new Hourly();
            LocationData? locationData = null;
            CoordinatesGetter? weatherCoordinatesGetter = null;
            WeatherService? weatherService = null;
            LLMChat chat = new LLMChat();
            string? userInput;
            string address;
            string userOptionAnotherForecast;
            bool validForecast;
            string dateForecast;
            int daysAheadForecast;
            string forecastDataString;

            Console.WriteLine("      ____             ____  _                             ");
            Console.WriteLine("     / ___|  ___  __ _/ ___|| | ___   _                    ");
            Console.WriteLine("     \\___ \\ / _ \\/ _` \\___ \\| |/ / | | |              ");
            Console.WriteLine("      ___) |  __/ (_| |___) |   <| |_| |                   ");
            Console.WriteLine("     |____/ \\___|\\__,_|____/|_|\\_\\__, |                ");
            Console.WriteLine("      _   _             _         |___/                    ");
            Console.WriteLine("     | \\ | | __ ___   _(_) __ _  __ _| |_ ___  _ __       ");
            Console.WriteLine("     |  \\| |/ _` \\ \\ / / |/ _` |/ _` | __/ _ \\| '__|   ");
            Console.WriteLine("     | |\\  | (_| |\\ V /| | (_| | (_| | || (_) | |        ");
            Console.WriteLine("     |_| \\_|\\__,_| \\_/ |_|\\__, |\\__,_|\\__\\___/|_|   ");
            Console.WriteLine("                          |___/                            ");

            Console.WriteLine("============================================================");
            Console.WriteLine();
            Console.WriteLine("=> Ahoy there! Welcome to the SeaSky Navigator, me hearty!");
            Console.WriteLine($"=> The present moment be upon us is {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} | {localTimeZone.DisplayName}");

            do
            {
                Console.WriteLine();
                Console.WriteLine("=> Kindly share yer whereabouts or type \"quit\" to abandon the ship " +
                                  "and depart from the app, me matey. Where be ye sailin' today?");

                userInput = Console.ReadLine();
                if (string.IsNullOrEmpty(userInput))
                {
                    Console.WriteLine("=> Arrr, no coordinates set sail, me hearty! Ye forgot to mark the " +
                                      "treasure on the map. Without an address, " +
                                      "we be wanderin' the high seas with no destination in sight.");
                    continue;
                }
                else if (userInput == "quit")
                    break;
                else
                    address = userInput;

                weatherCoordinatesGetter = new CoordinatesGetter();
                Task<string> coordinatesResponse = weatherCoordinatesGetter.GetCoordinates(address);
                Location? location = JsonSerializer.Deserialize<Location>(coordinatesResponse.Result);

                validForecast = CheckIfAnyLocationForecast(location);
                if (validForecast == false)
                    continue;

                locationData = location.data[0];

                dateForecast = AskUserForecastDate();
                daysAheadForecast = AskUserForecastRange();
                daysAheadForecast = CheckForecastRangeLastDate(dateForecast, daysAheadForecast);

                weatherService = new WeatherService(dateForecast, daysAheadForecast);

                Task<string> weatherResponse = weatherService.GetWeatherData(locationData.latitude, locationData.longitude);
                WeatherForecast? weatherForecast = JsonSerializer.Deserialize<WeatherForecast>(weatherResponse.Result);

                //if (DateTime.Parse(dateForecast).Date <= DateTime.Today.Date)
                KeepOnlyActualForecast(hourlyForecast, weatherForecast);

                forecastDataString = CreateForecastDataString(hourlyForecast);

                Console.WriteLine();
                Console.WriteLine("=> Preparing to set sail...");
                Console.WriteLine("=> Hold fast, for the SeaSky Navigator is ready to reveal the weather secrets!");

                await chat.GiveMeTheWeatherReportMate(forecastDataString, address);


                userOptionAnotherForecast = AskUserAnotherForecast();

                if (userOptionAnotherForecast.ToUpper() == "N")
                    break;

            } while (userInput != "quit");


            if (weatherService != null)
                weatherService.Dispose();

            if (weatherCoordinatesGetter != null)
                weatherCoordinatesGetter.Dispose();

            Console.WriteLine();
            Console.WriteLine("=> Ye be most welcome, me heartie! Fair winds and followin' seas on yer journeys. " +
                              "Should ye ever need a weather update or guidance through the digital waters, feel free " +
                              "to return to the SeaSky Navigator. Until then, may the compass always point in yer favor!");
            Console.ReadKey();
        }


        private static bool CheckValidityUserDate(string userInputDate)
        {
            bool isValidDate = DateTime.TryParse(userInputDate, out _);

            return isValidDate;
        }

        private static bool CheckOnRangeUserDate(string userInputDate)
        {
            DateTime maxDate = DateTime.Today.AddDays(8);
            DateTime userDate = DateTime.Parse(userInputDate);

            bool dateOnRange = userDate <= maxDate;

            return dateOnRange;
        }


        private static string AskUserForecastDate()
        {
            Console.WriteLine();
            Console.WriteLine("=> Avast ye! What day be ye wishin' to glimpse into the forecast?");
            Console.WriteLine("=> Please chart the course in this format: yyyy-mm-dd");
            Console.WriteLine("=> Should ye desire today's forecast, type 'today' or simply press enter.");

            string? userInputDate = Console.ReadLine();

            if (string.IsNullOrEmpty(userInputDate) || (userInputDate != null && userInputDate.ToLower() == "today"))
                userInputDate = DateTime.Now.ToString("yyyy-MM-dd");

            bool isValidDate = CheckValidityUserDate(userInputDate);
            bool isOnRangeDate = CheckOnRangeUserDate(userInputDate);

            if (isValidDate == true)
                if (isOnRangeDate == true)
                    return userInputDate;
                else
                    Console.WriteLine("Avast ye! The date be beyond the horizon for forecasts. Best be providin' " +
                                      "a date within 7 days from today, lest ye want the winds to play tricks on yer forecast!");


            Console.WriteLine("=> Arrr! The date ye provided be as slippery as an eel. Try again, and mind the format: yyyy-mm-dd");
            return AskUserForecastDate();
            
        }

        private static int CheckForecastRangeLastDate(string startDate, int days)
        {
            DateTime userDate = DateTime.Parse(startDate);
            DateTime lastDate = userDate.AddDays(days);
            DateTime maxDate = DateTime.Today.AddDays(7);

            if (lastDate <= maxDate)
                return days;
            else
            {
                TimeSpan maxDaysRange = maxDate - userDate;
                int maxDateDiffDays = maxDaysRange.Days;

                Console.WriteLine($"Arrr! Can't glimpse into the far reaches of the future, me heartie. " +
                                  $"I can spy up to {maxDateDiffDays} days ahead, and no further!");


                return maxDateDiffDays;
            }
        }

        private static int AskUserForecastRange()
        {
            string? userInput;
            Console.WriteLine();
            Console.WriteLine("=> Set sail into the future! Let me know how many days ahead ye want to spy on.");
            Console.WriteLine("=> This should be a value betwixt 0 and 7, no more, no less.");

            userInput = Console.ReadLine();

            int daysAhead;
            if (int.TryParse(userInput, out daysAhead) == true)
                if (daysAhead >= 0 && daysAhead <= 7)
                    return daysAhead;

            Console.WriteLine("=> Ahoy! That be an unfamiliar code on the map. Try again, and let the value be betwixt the known bounds.");

            return AskUserForecastRange();
        }


        private static bool CheckIfAnyLocationForecast(Location location)
        {
            if (location.data.Count > 0)
                return true;
            else
            {
                Console.WriteLine("=> Arrr! No charts mark the location ye seek. Try again, and let the winds reveal the way.");
                return false;
            }
        }

        private static string AskUserAnotherForecast()
        {
            string? userOption;
            Console.WriteLine("=> Ready for another forecast quest?");
            Console.WriteLine("=> [Y]es to embark on a new journey or [N]o to drop anchor and end the voyage.");
            userOption = Console.ReadLine();


            if (userOption.ToUpper() != "Y" && userOption.ToUpper() != "N")
            {
                Console.WriteLine("=> Arrr! The compass be confounded by that choice. Try a different course.");
                return AskUserAnotherForecast();
            }

            return userOption;

        }

        private static void KeepOnlyActualForecast(Hourly hourlyForecast, WeatherForecast weatherForecast)
        {
            int indexToKeep = weatherForecast.hourly.time.FindIndex(stringTime => DateTime.Parse(stringTime) >= DateTime.Now);

            if (indexToKeep < 0)
                indexToKeep = 0;
            
            hourlyForecast.time = weatherForecast.hourly.time.Skip(indexToKeep).ToList();
            hourlyForecast.temperature_2m = weatherForecast.hourly.temperature_2m.Skip(indexToKeep).ToList();
            hourlyForecast.apparent_temperature = weatherForecast.hourly.apparent_temperature.Skip(indexToKeep).ToList();
            hourlyForecast.relative_humidity_2m = weatherForecast.hourly.relative_humidity_2m.Skip(indexToKeep).ToList();
            hourlyForecast.precipitation_probability = weatherForecast.hourly.precipitation_probability.Skip(indexToKeep).ToList();  
        }

        private static void PrintForecast(LocationData locationData, Hourly hourly)
        {
            Console.WriteLine("=============================================================================================================");
            Console.WriteLine($"  Weather forecast for {locationData.name}, {locationData.country}");
            Console.WriteLine($"  Coordinates are latitude: {locationData.latitude:F2}, longitude: {locationData.longitude:F2}");
            Console.WriteLine("=============================================================================================================");
            Console.WriteLine("      Time                 Forecast (ºC)        Apparent temp. (ºC)         Rel. Humidity          Precipitation Prob. ");
            Console.WriteLine("=============================================================================================================");

            for (int i = 0; i < hourly.time.Count; i++)
            {
                Console.WriteLine($"  {hourly.time[i],20} " +
                                  $"{hourly.temperature_2m[i],20} " +
                                  $"{hourly.apparent_temperature[i],20} " +
                                  $"{hourly.relative_humidity_2m[i],20} " +
                                  $"{hourly.precipitation_probability[i],20}");
            }

            Console.WriteLine("=============================================================================================================");
            Console.WriteLine();
        }

        private static string CreateForecastDataString(Hourly hourlyForecast)
        {
            string result = "time,temperature_2m,apparent_temperature,relative_humidity_2m,precipitation_probability";
            result += Environment.NewLine;

            for (int i = 0; i < hourlyForecast.time.Count; i++)
            {
                result += $"{hourlyForecast.time[i]}," +
                          $"{hourlyForecast.temperature_2m[i]}," +
                          $"{hourlyForecast.apparent_temperature[i]}," +
                          $"{hourlyForecast.relative_humidity_2m[i]}," +
                          $"{hourlyForecast.precipitation_probability[i]}";

                result += Environment.NewLine;
            }

            return result;
        }
    }

}
