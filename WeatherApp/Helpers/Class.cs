using SeaSkyNavigator.Classes;
using SeaSkyNavigator.Records;

namespace SeaSkyNavigator.Helpers
{
    public static class ForecastHelper
    {
        internal static bool CheckValidityUserDate(string userInputDate)
        {
            bool isValidDate = DateTime.TryParse(userInputDate, out _);

            return isValidDate;
        }

        internal static bool CheckOnRangeUserDate(string userInputDate)
        {
            DateTime maxDate = DateTime.Today.AddDays(8);
            DateTime userDate = DateTime.Parse(userInputDate);

            bool dateOnRange = userDate <= maxDate;

            return dateOnRange;
        }


        internal static string AskUserForecastDate()
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

        internal static int CheckForecastRangeLastDate(string startDate, int days)
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

        internal static int AskUserForecastRange()
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


        internal static bool CheckIfAnyLocationForecast(Location location)
        {
            if (location.data.Count > 0)
                return true;
            else
            {
                Console.WriteLine("=> Arrr! No charts mark the location ye seek. Try again, and let the winds reveal the way.");
                return false;
            }
        }

        internal static string AskUserAnotherForecast()
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

        internal static void KeepOnlyActualForecast(Hourly hourlyForecast, WeatherForecast weatherForecast)
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


        internal static string CreateForecastDataString(Hourly hourlyForecast)
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
