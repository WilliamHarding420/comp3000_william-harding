

if (localStorage.getItem("currentFolderID") == null) {

    localStorage.setItem("currentFolderID", 0);
    localStorage.setItem("currentFolderPath", JSON.stringify([
        {
            "LocationID": 0,
            "LocationName": "root"
        }
    ]));

}

function showStream(camera) {

    loadHTML("/pages/stream.html", document.getElementById("camera-module-container"), null, function () {

        let cameraTitle = document.getElementById("cameraTitle");
        cameraTitle.innerHTML = camera["Name"];

        let recordingsButton = document.getElementById("recordings-button");
        let editButton = document.getElementById("editDetails");
        let deleteButton = document.getElementById("deleteCamera");

        recordingsButton.onclick = function () {
            showRecordings(camera["PublishURL"])
        };

        editButton.onclick = function () {
            loadHTMLToID('/pages/add_camera.html', 'module-content', 'add_camera.js');

            localStorage.setItem("editing", JSON.stringify(camera));
        }

        deleteButton.onclick = function () {
            deleteCamera(camera);
        }



        let player = document.getElementById("video-player");

        if (Hls.isSupported()) {
            let hls = new Hls();
            hls.loadSource("http://localhost/stream/" + camera["PublishURL"] + "/index.m3u8");
            hls.attachMedia(player);

            // Event for stopping requesting hls stream after loading another page
            document.addEventListener("html_load", function () { stopLoadingStreamCallback(hls); });

        }
        else if (player.canPlayType('application/vnd.apple.mpegurl')) {
            player.src = "http://localhost:8888/" + camera["PublishURL"] + "/index.m3u8";
        }

    });

}

function showRecordings(streamURL) {

    loadHTML("/pages/recordings.html", document.getElementById("camera-module-container"), null, function () {

        let video_list = document.getElementById("recordings-list");

        GET_Request(`/videos/list?path=${streamURL}`, function (response) {
            
            response.forEach((element) => {

                let videoButton = document.createElement("button");
                videoButton.innerHTML = `<div class='list'>${element["start"]}</div>`;
                videoButton.onclick = function () {

                    let url = `/videos/${element["url"].split("/")[3]}`

                    playbackRecording(url);

                };

                video_list.appendChild(videoButton);

            });

        }, null);

    });

}

function playbackRecording(recordingURL) {

    loadHTML("/pages/video.html", document.getElementById("camera-module-container"), null, function () {

        let videoPlayer = document.createElement("video");
        videoPlayer.width = "960";
        videoPlayer.height = "540";
        videoPlayer.controls = "controls";
        videoPlayer.src = recordingURL;

        document.getElementById("video-playback").appendChild(videoPlayer);

    });

}

function stopLoadingStreamCallback(hls) {

    hls.stopLoad();
    document.removeEventListener("html_load", function () { stopLoadingStreamCallback(hls); });

}

function showCameras() {

    // setting the location path display
    let pathDisplay = document.getElementById("pathDisplay");
    let currentPathString = "Current Location Path: Root Location";

    let currentPathList = JSON.parse(localStorage.getItem("currentFolderPath"));

    for (let i = 1; i < currentPathList.length; i++) {
        currentPathString += " >> " + currentPathList[i].LocationName;
    }

    pathDisplay.innerHTML = currentPathString;

    let locationBackButton = document.getElementById("locationBackButton");

    if (localStorage.getItem("currentFolderID") == 0) {
        locationBackButton.disabled = true;
    }
    else {
        locationBackButton.disabled = false;
    }

    let camera_list = document.getElementById("camera-list");
    let location_list = document.getElementById("location-list");

    camera_list.innerHTML = "";
    location_list.innerHTML = "";

    GET_Request_Auth("/cctv/get/" + localStorage.getItem("currentFolderID"), function (response) {

        let responseJSON = JSON.parse(response);
        console.log(responseJSON);

        responseJSON["locations"].forEach((element) => {

            let locationButton = document.createElement("button");
            locationButton.classList.add("w-100");
            locationButton.classList.add("mt-1");
            locationButton.innerHTML = `<div class='list w-100'>${element["LocationName"]}</div>`

            locationButton.onclick = function () {

                let currentPath = JSON.parse(localStorage.getItem("currentFolderPath"));
                currentPath.push({
                    "LocationID": element["LocationID"],
                    "LocationName": element["LocationName"]
                });

                localStorage.setItem("currentFolderID", element["LocationID"]);
                localStorage.setItem("currentFolderPath", JSON.stringify(currentPath));

                showCameras();

            }

            location_list.appendChild(locationButton);

        });

        responseJSON["cameras"].forEach((element) => {

            let streamButton = document.createElement("button");
            streamButton.classList.add("m-2");
            streamButton.innerHTML = `<div class='list'><img width='320' height='180' src='/thumbnails/${element['CameraID']}.png'><br /> ${element["Name"]}</div>`;
            streamButton.onclick = function () {

                POST_Request_Auth(`/cctv/thumbnail/update/${element["CameraID"]}`, null, null, null);

                showStream(element);

            }

            camera_list.appendChild(streamButton);

        });

    }, function (xhr, response)
    {
        alert("Could not load cameras."); console.log(response);
    });

}

function newLocation() {

    let locationName = prompt("Enter the name of the location...");

    let locationData = {
        "Name": locationName,
        "ParentID": localStorage.getItem("currentFolderID")
    }

    POST_Request_Auth("/cctv/location/new", locationData, function (response) {

        alert("Location " + locationName + " added.");
        showCameras();

    }, function (xhr, response) {

        let json = JSON.parse(xhr["responseJSON"]);

        alert("Error: " + json["error"]);

    });

}

function setPreviousLocation() {

    let folderPath = JSON.parse(localStorage.getItem("currentFolderPath"));

    folderPath.pop();
    localStorage.setItem("currentFolderID", folderPath[folderPath.length - 1].LocationID);
    localStorage.setItem("currentFolderPath", JSON.stringify(folderPath));

}

function previousLocation() {

    setPreviousLocation();

    showCameras();

}

function deleteLocation() {

    let deleteID = localStorage.getItem("currentFolderID");
    let deleteDetails = JSON.parse(localStorage.getItem("currentFolderPath")).at(-1);

    let confirmPrompt = prompt("Removing this location will also delete child locations and cameras. Type the location name to confirm.");

    if (confirmPrompt != deleteDetails.LocationName) {
        alert("Names do not match.");
        return;
    }

    POST_Request_Auth("/cctv/location/remove/" + deleteID, null, function (response) {

        alert("Location " + deleteDetails.LocationName + " removed.");

        setPreviousLocation();
        showCameras();

    }, function (xhr, response) {

        let json = JSON.parse(xhr["responseJSON"]);

        alert("Error: " + json["error"]);

    });


}

function deleteCamera(camera) {

    let confirmPrompt = prompt("Enter the camera name to confirm you want to delete it.");

    console.log(confirmPrompt);
    console.log(camera["Name"]);
    if (confirmPrompt != camera["Name"]) {
        alert("Name did not match.")
        return;
    }

    POST_Request_Auth("/cctv/remove/" + camera["CameraID"], null, function (response) {

        alert("Camera " + camera["Name"] + " removed.");

        loadHTMLToID('/pages/cctv.html', 'module-content', 'cctv.js');

    }, function (xhr, response) {

        let json = JSON.parse(xhr["responseJSON"]);

        alert("Error: " + json["error"]);

    });

}

showCameras();