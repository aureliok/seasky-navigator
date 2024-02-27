# SeaSky Navigator
![SeaSky Navigator](assets/seaskynavigator_title.jpg)

## Table of Contents

- [Introduction](#introduction)
- [APIs Used](#apis-used)
- [App Overview](#getting-started)
- [App .CS Files](#apps-cs-files)
- [Future Improvements](#future-improvements)

## Introduction
This small project started as a simple way I've found to have a little more practice on the C# programming language, and the initial idea was to just get practical experience on using consuming public available RestAPIs on a .NET framework, and start building a portfolio to consolidate and showcase skills that I've been studying recently.
<br><br>
Naming it "SeaSky Navigator", the core idea of the app is simple and perhaps a project that many beginners may have done it at some point in their study journey: a app that gives prediction of the weather.
<br><br>
There is a little twist to this one, though. The app is using the capabilities of a LLM (namely the ChatGPT 3.5 Turbo form OpenAI) to deliver the weather report in a pirate speak. It is done in good fun, and helped me feel a little more comfortable on using the .NET environment and with OOP (Object Oriented Programming) with C#.

## APIs Used
- [Open-Meteo](https://open-meteo.com/): for weather forecasts
- [Position Stack](https://positionstack.com/): for coordinates
- [OpenAI](https://openai.com/): for LLM capabilities


## App Overview
As a beginner, I've decided to start using a public API of weather forecast, **Open-Meteo**, to build a simple console app in which the user would input the location it wanted, and the days to get the weather forecast on.
<br><br>
Though, to be able to get the prediction from Open-Meteo API, one needs to pass the coordinates of both latitude and longitude on the request method. It would be an inconvenience to the user if he had to search these coordinates somewhere else, and then pass it to the app. So to solve this little problem, the app is instructed to use another free API from **PositionStack** to get the coordinates from the user's input.
<br><br>
![Asking for Location](assets/seaskynavigator_location.jpg)
<br><br>
After getting the location's coordinates returned with the Position Stack's API (and check if the location is valid), the next step is to ask the user for a date and how many days from that date he wants to get his weather forecast on. For example, 2024-02-26 and 2 days would mean we would return the user a report on the forecast of the days ranging from 2024-02-26 to 2024-02-28. <br>
In this part there's also a check to see if the user has passed a valid date, meaning it has to be on a format "yyyy-MM-dd" and also to see if the range of dates he wants doesn't exceed 7 days from the current day (not the date he inputted). The limit of 7 days from the current date is to prevent the possibility of the app to return null values on the forecast on those days that are farther on the future.
<br><br>
![Asking for dates](assets/seaskynavigator_dates.jpg)
<br><br>
With these information on hand, the app will then call the Open-Meteo API to get the JSON response on the forecast values. As of now, the app is receiving the data on temperature, apparent temperature, relative humidity and precipitation probability to build it's report.<br>
On the earlier stages of the app, these weather data would only be printed in tabular-like form on the console, but it seemed like an uninteresting way to do this. So the app is told to call the *OpenAI*'s API to use the **ChatGPT 3.5 Turbo** (the model can be changed) and feed it first with a initial prompt instructing the LLM that it will receive a string containing weather related data and that it should use those data to build an answer like a pirate and give the user a summarized forecast for each day the user requested. The connection to the OpenAI API is made using a unofficial NuGet package called OpenAI by OkGoDolt.
<br><br>
![Calling LLM](assets/seaskynavigator_gettingllm.jpg)
![Summarized report by LLM](assets/seaskynavigator_llmresponse.jpg)
<br><br>
After this, the app asks the user if he wants to get another forecast or quit. If he asks for another forecast, then all the process of asking the location, dates etc will be restarted.
<br>
![End of interaction](assets/seaskynavigator_endloop.jpg)

In summary, the app's flow looks something like this:
<br>
<img src="assets/seaskynavigator_diagram.jpg" alt="OpenAI Logo" width="70%">

## App .CS Files
### WeatherService.cs
Contains the class WeatherService, which implements the interface IDisposable. Responsible to use a HttpClient instance to make a async request on the Open-Meteo's API to get the forecast data.

### WeatherForecast.cs
Contains the code for the classes WeatherForecast, HourlyUnits, and specially the Hourly class, that will be used to store the data received from the Open-Meteo's API.

### CoordinatesGetter.cs
Contains the code for the class CoordinatesGetter that implements the IDisposable interface. Like the WeatherService class, it is responsible to use a HttpClient instance to make a call to an API, this time to the PositionStack. Needs an api key setted on the environment variables (POSITIONSTACK_API_KEY) to work.

### LLMChat.cs
Contains the class LLMChat that is used to initialize an conversation instance with a LLM model, and then feed the data to get the summarized final report. Also needs an api key on the environment variables (OPENAI_API_KEY) to work.

## Future Improvements
As my first project on a .NET framework and with C# language, this project has space for improvements and corrections that I would like to do in the future, such as:
- Removal of warnings on null references, as it is not handled extensively in the code;
- Handling of exceptions, as this is not something that is in the code yet, there are checks for formats and valid values, but doesn't have Exception handling for, say, a empty response for the calls made on the APIs;
- Better structuring and organization of code. The Program.cs have some functions that could be perhaps better fitted on a file for helper methods;
- A friendlier interface. I hope to be able to do this one soon, as I study more of HTML and CSS. And will be a great start point to practice building a RESTful API and get familiar with the ASP.NET framework.

