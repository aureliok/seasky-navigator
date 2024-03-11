'use strict';

const todayString = new Date().toISOString().split("T")[0];
const locationInput = document.getElementById("location-input");
const startDateInput = document.getElementById("start-date-input");
const endDateInput = document.getElementById("end-date-input");
const getReportBtn = document.getElementById("get-report-btn");
const weatherReportText = document.getElementById("weatherReportText");

const getStartEndDatesDifference = (start, end) => {
    const startD = new Date(start);
    const endD = new Date(end);

    const timeDifference = endD - startD;
    console.log(timeDifference / (1000 * 60 * 60 * 24));
    return timeDifference / (1000 * 60 * 60 * 24);
};


const preProcessLocation = location => {
    console.log(location.trim());
    return location.trim();
};

const sendApiRequest = (dataObj) => {
    const apiUrl = 'https://localhost:7009/SeaSkyNavigator/GiveMeTheForecastMate';

    console.log(JSON.stringify(dataObj));

    const urlWithParams = new URL(apiUrl);
    Object.keys(dataObj).forEach(key => urlWithParams.searchParams.append(key, dataObj[key]));
    
    // Making the GET request
    fetch(urlWithParams)
      .then(response => response.text())
        .then(data => {
          // clean output text
          console.log(data);
          weatherReportText.innerHTML = data;
        })
      .catch(error => console.error('Error:', error));
};



const getReport = () => {
    // get data
    const location = locationInput.value;
    const startDate = startDateInput.value;
    const endDate = endDateInput.value || startDate;

    // check if data was inputted
    if (!location || !startDate) {
        alert("Please fill in all required fields");
        return;
    }


    const daysReport = getStartEndDatesDifference(startDate, endDate);
    const locationCleaned = preProcessLocation(location);

    const requestData = {
        address: locationCleaned,
        startDate: startDate,
        daysAheadForecast: daysReport
    };

    sendApiRequest(requestData);
    

    // send api
    // get from api
    // display on html
};


getReportBtn.addEventListener("click", getReport);