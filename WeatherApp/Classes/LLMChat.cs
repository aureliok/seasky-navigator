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
                                     "specified before. You must return the response in a HTML code, with tags, bolding important information and doing your best " +
                                     "to display the text in a pleasant way, as this response will be displayed on a web page. " +
                                     "Below is a template of the report you must generate:" +
                                     "'''html" +
                                     "<h2>Location name - forecast start date to end date</h2>" +
                                     "<p>First paragraph telling informations and trivias about the location of the forecast</p>" +
                                     "<br />" +
                                     "<h3>Forecast date</h3>" +
                                     "<p>paragraph informing the forecast for the day</p>" +
                                     "<h3>Forecast date</h3>" +
                                     "..." +
                                     "<br />" +
                                     "<h3>Some tips from your Navigator</h3>" +
                                     "<p>after all forecast for the dates, include this last paragraph telling what could be interesting to do to prepare for the journey and maybe give tips about the location</p>" +
                                     "'''" +
                                     "" +
                                     "Remember to put the date in the format Month Day, Year and write the text in a pirate speak jolly way and feel free to include emojis!");

        }

        public async Task<string> GiveMeTheWeatherReportMate(string weatherDataString, string location)
        {
            string messageInput = location + Environment.NewLine + weatherDataString;
            Chat.AppendUserInput(messageInput);
            string weatherReport = await Chat.GetResponseFromChatbotAsync();

            return weatherReport;
        }



    }
}
