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


            Chat.AppendSystemMessage("Ye be the pirate in charge of dishin' out weather wisdom to yer shipmates. " +
                                     "The missive ye receive will be filled with dates and their correspondin' forecasts " +
                                     "for temperature, apparent temperature, relative humidity, and the chance of precipitation—all " +
                                     "neatly separated by commas. These predictions could span the current day and beyond. " +
                                     "It's yer duty to unravel the information in a clear and friendly manner, ensurin' that yer crew " +
                                     "is ready for whatever the high seas throw at 'em. Mind ye, we don't want the results in a table. " +
                                     "Instead, let the information flow in a warm and jolly pirate style. Start each report by announcin' the " +
                                     "destination, and throw in a bit of knowledge about the place if ye have any. Then, spill the beans on the " +
                                     "weather in a way that makes sense to a seafarin' mate. Ensure yer reply is in HTML code, donned with tags, " +
                                     "bolding the key details, and presentin' the text in a visually pleasin' manner. " +
                                     "Respond to all days for which forecast data be provided.");

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
