
const htmlLoadEvent = new CustomEvent("html_load");

function POST_Request(url, jsonData, successCallback, errorCallback) {

    $.ajax({
        type: "POST",
        url: url,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonData),
        success: successCallback,
        error: errorCallback
    })

}

function POST_Request_Auth(url, jsonData, successCallback, errorCallback) {

    $.ajax({
        type: "POST",
        url: url,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonData),
        success: successCallback,
        error: errorCallback,
        headers: {
            "Authorization": "Bearer " + localStorage.getItem("authToken")
        }
    })

}

function GET_Request(url, successCallback, errorCallback) {

    $.ajax({
        type: "GET",
        url: url,
        success: successCallback,
        error: errorCallback
    })

}

function GET_Request_Auth(url, successCallback, errorCallback) {

    $.ajax({
        type: "GET",
        url: url,
        success: successCallback,
        error: errorCallback,
        headers: {
            "Authorization": "Bearer " + localStorage.getItem("authToken")
        }
    })

}

function loadScript(script) {

    $.getScript("/scripts/" + script);

}

function loadHTML(url, parentElement, script = null, success = function () { }) {

    GET_Request(url, function (response) {

        parentElement.innerHTML = response;

        if (script !== null) {
            loadScript(script);
        }

        document.dispatchEvent(htmlLoadEvent);
        success();

    }, function (response) {

        alert("Failed to load HTML, URL: " + url);
        console.log(response);

    });

}

function loadHTMLToID(url, parentID, script = null, success = function () { }) {

    let parent = document.getElementById(parentID);

    loadHTML(url, parent, script);

}