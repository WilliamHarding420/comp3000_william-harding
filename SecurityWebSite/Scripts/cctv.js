
function showStream(streamURL, cameraName) {

    loadHTML("/pages/stream.html", document.getElementById("camera-module-container"), null, function () {

        let recordingsButton = document.getElementById("recordings-button");
        recordingsButton.onclick = function () {
            showRecordings(streamURL)
        };

        let player = document.getElementById("video-player");

        if (Hls.isSupported()) {
            let hls = new Hls();
            hls.loadSource("http://localhost/stream/" + streamURL + "/index.m3u8");
            hls.attachMedia(player);

            // Event for stopping requesting hls stream after loading another page
            document.addEventListener("html_load", function () { stopLoadingStreamCallback(hls); });

        }
        else if (player.canPlayType('application/vnd.apple.mpegurl')) {
            player.src = "http://localhost:8888/" + streamURL + "/index.m3u8";
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

    let camera_list = document.getElementById("camera-list");

    /*GET_Request("/rtsp/v3/paths/list", function (response) {

        response["items"].forEach((element) => {

            let streamButton = document.createElement("button");
            streamButton.innerHTML = element["name"];
            streamButton.onclick = function () {
                showStream(element["name"]);
            };

            camera_list.appendChild(streamButton);

        });

    }, null);*/

    GET_Request("/cctv/get/0", function (response) {

        let cameras = JSON.parse(response);

        cameras["cameras"].forEach((element) => {

            let streamButton = document.createElement("button");
            streamButton.innerHTML = `<div class='list'><img width='320' height='180' src='/thumbnails/${element['Name']}.png'><br /> ${element["Name"]}</div>`;
            streamButton.onclick = function () {

                POST_Request(`/cctv/thumbnail/update/${element['Name']}`, null, null, null);

                showStream(element["PublishURL"], element["Name"]);

            };

            camera_list.appendChild(streamButton);

        });

    }, null);
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







function deleteLocation() {

    let deleteID = localStorage.getItem("currentFolderID");
    let deleteDetails = JSON.parse(localStorage.getItem("currentFolderPath")).at(-1);

    let confirmPrompt = prompt("Removing this location will also delete child locations and cameras. Type the location name to confirm.");

    if (confirmPrompt != deleteDetails.Name) {
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