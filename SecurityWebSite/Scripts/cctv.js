
function showStream(streamURL) {

    loadHTML("/pages/stream.html", document.getElementById("camera-module-container"), null, function () {

        let player = document.getElementById("video-player");

        if (Hls.isSupported()) {
            let hls = new Hls();
            hls.loadSource("http://localhost/stream/" + streamURL + "/index.m3u8");
            hls.attachMedia(player);
        }
        else if (player.canPlayType('application/vnd.apple.mpegurl')) {
            player.src = "http://localhost:8888/mystream/index.m3u8";
        }

    });

}

function showCameras() {

    let camera_list = document.getElementById("camera-list");

    GET_Request("/rtsp/v3/paths/list", function (response) {

        response["items"].forEach((element) => {

            let streamButton = document.createElement("button");
            streamButton.innerHTML = element["name"];
            streamButton.onclick = function () {
                showStream(element["name"]);
            };

            camera_list.appendChild(streamButton);

        });

    }, null);

}

showCameras();