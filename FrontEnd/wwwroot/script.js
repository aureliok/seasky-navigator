'use strict';

const limitDays = 9;
const today = new Date();
const maxDate = new Date();
const todayString = today.toISOString().split("T")[0];
const maxDateString = new Date(maxDate.setDate(maxDate.getDate() + limitDays)).toISOString().split("T")[0];
const locationInput = document.getElementById("location-input");
const startDateInput = document.getElementById("start-date-input");
const endDateInput = document.getElementById("end-date-input");
const getReportBtn = document.getElementById("get-report-btn");
const clearBtn = document.getElementById("clear-btn");
const weatherReportText = document.getElementById("weatherReportText");


startDateInput.min = todayString;
startDateInput.max = maxDateString;
endDateInput.min = todayString;
endDateInput.max = maxDateString;


const getStartEndDatesDifference = (start, end) => {
    const startD = new Date(start);
    const endD = new Date(end);

    const timeDifference = endD - startD;
    return timeDifference / (1000 * 60 * 60 * 24);
};

const preProcessLocation = location => {
    return location.trim();
};

const cleanOutputText = text => {
    return text.replace(/```(html)?/g, "");
};

const resetOutputBox = () => {
    weatherReportText.innerHTML = "";
    weatherReportText.parentElement.style.opacity = .5;
};

const clearFields = () => {
    locationInput.value = "";
    startDateInput.value = "";
    endDateInput.value = "";
}

const clearPage = () => {
    resetOutputBox();
    clearFields();
};

const sendApiRequest = (dataObj) => {
    const apiUrl = 'https://localhost:7009/SeaSkyNavigator/GiveMeTheForecastMate';

    weatherReportText.parentElement.style.opacity = .75;
    weatherReportText.innerHTML = "Hold fast, for the <strong>SeaSky Navigator</strong> is about to reveal the weather secrets!"

    const urlWithParams = new URL(apiUrl);
    Object.keys(dataObj).forEach(key => urlWithParams.searchParams.append(key, dataObj[key]));
    
    // Making the GET request
    fetch(urlWithParams)
      .then(response => {
        if (!response.ok) {
            alert(`Error Status: ${response.status} An error has ocurred :(`);
            return Promise.reject(`Error Status: ${response.status}`);
        }
        return response.text()
    })
        .then(data => {
          // clean output text
          console.log(cleanOutputText(data));
          resetOutputBox();
          weatherReportText.parentElement.style.opacity = 1;
          weatherReportText.innerHTML = cleanOutputText(data);
        })
      .catch(error => console.error('Error:', error));
};

const getReport = () => {

    const location = locationInput.value;
    const startDate = startDateInput.value;
    const endDate = endDateInput.value || startDate;

    resetOutputBox();

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
};


getReportBtn.addEventListener("click", getReport);
clearBtn.addEventListener("click", clearPage);
startDateInput.addEventListener("input", () => {
    endDateInput.min = startDateInput.value;
});