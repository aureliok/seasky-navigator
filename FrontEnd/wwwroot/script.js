'use strict';

const todayString = new Date().toISOString().split("T")[0];
const locationInput = document.getElementById("location-input");
const startDateInput = document.getElementById("start-date-input");
const endDateInput = document.getElementById("end-date-input");
const getReportBtn = document.getElementById("get-report-btn");

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
    // const apiUrl = 'https://localhost:7009/SeaSkyNavigator/GiveMeTheForecastMate';
    // const apiUrl = 'https://localhost:7009/SeaSkyNavigator/GiveMeTheForecastMate?address=barcelona&startDate=2024-03-15&daysAheadForecast=0'

    const apiUrl = 'http://api.positionstack.com/v1/forward';
    const apiKey = '9a79baffc457faf0e909a4ef44cdb8e9'; // Replace with your actual API key
    const address = 'barcelona';

    const requestUrl = `${apiUrl}?access_key=${apiKey}&query=${address}`;
    console.log(requestUrl);
    fetch(requestUrl)
    .then(response => response.json())
    .then(data => console.log(data))
    .catch(error => console.error('Error:', error));

    // fetch(apiUrl, {
    //     method: "POST",
    //     headers: {
    //         "Content-Type": "application/json"
    //     },
    //     body: JSON.stringify(dataObj),
    // }) 
    //     .then(response => {
    //         if (!response.ok) {
    //             throw new Error(`Response status: ${response.status}`);
    //         }
    //         return response.json();
    //     })
    //     .then(responseData => {
    //         console.log(responseData);
    //     })
    //     .catch(error => {
    //         console.error(error.message);
    //     });

    // $.ajax({
    //     url: apiUrl,
    //     method: 'POST',
    //     contentType: 'application/json',
    //     data: JSON.stringify(dataObj),
    //     success: function(data) {
    //       // Handle the success response
    //       console.log(data);
    //     },
    //     error: function(xhr, textStatus, errorThrown) {
    //       // Handle errors
    //       console.error('Error:', errorThrown);
    //     }
    //   });
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

    // if (endDateInput.value) {
    //     endDate = startDate;
    // }

    const daysReport = getStartEndDatesDifference(startDate, endDate);
    const locationCleaned = preProcessLocation(location);

    const requestData = {
        "address": locationCleaned,
        "startDate": startDate,
        "daysAheadForecast": daysReport
    };

    sendApiRequest(requestData);
    

    // send api
    // get from api
    // display on html
};


getReportBtn.addEventListener("click", getReport);