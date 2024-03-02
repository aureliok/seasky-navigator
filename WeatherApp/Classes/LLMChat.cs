using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenAI_API;
using OpenAI_API.Models;

namespace SeaSkyNavigator.Classes
{
    internal class LLMChat
    {
        private OpenAIAPI Api { get; }
        private Model Model { get; set; }
        double Temperature;
        OpenAI_API.Chat.Conversation Chat = null;


        internal LLMChat(double temperature = .5)
        {
            Api = new OpenAIAPI(new APIAuthentication(
                Environment.GetEnvironmentVariable("OPENAI_API_KEY")));
            Model = new Model("gpt-3.5-turbo");
            Temperature = temperature;
            Chat = Api.Chat.CreateConversation();
            Chat.Model = Model;
            Chat.RequestParameters.Temperature = Temperature;

            Chat.AppendSystemMessage("You are a pirate responsible to give your fellow ship members a helpful and detailed weather forecast." +
                                     "You will receive a text containing information about the dates, and their respectives forecasts on temperature," +
                                     "apparent_temperature, relative humidity and precipitation probability, all those values will be separated by commas." +
                                     "You may receive forecast on the current days and days ahead, and it's your job to give the information detailed and" +
                                     "accurately as possible so your ship members don't go go out to work or go sailing unprepared." +
                                     "Do not give out the results as in a table, but in a heartful and jolly way, just like a pirate, explaining what they must do" +
                                     "to prepare for the weather ahead. And answer for all days that you've been given the forecast data." +
                                     "You will be receiving also the name of the location of the forecasts, so you need to always start your report saying you'll " +
                                     "be sailing to the location provided and give a little bit of info of the location (if you know any) and then the report as " +
                                     "specified before. Feel free to format the location as you wish. " +
                                     "Remember also to always start a new paragraph with this symbol \"=> \".");

        }

        public async Task<string> GiveMeTheWeatherReportMate(string weatherDataString, string location)
        {
            string messageInput = location + Environment.NewLine + weatherDataString;
            Chat.AppendUserInput(messageInput);
            string weatherReport = await Chat.GetResponseFromChatbotAsync();
            //weatherReport += Environment.NewLine;

            //Console.WriteLine();
            //Console.WriteLine("=====================================================================");
            //Console.WriteLine();
            //Console.WriteLine(weatherReport);

            return weatherReport;
        }



    }
}
