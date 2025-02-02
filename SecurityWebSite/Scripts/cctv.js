
function showStream(streamURL) {

    loadHTML("/pages/stream.html", document.getElementById("camera-module-container"), null, function () {

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
        console.log(cameras);
        cameras["cameras"].forEach((element) => {

            let streamButton = document.createElement("button");
            streamButton.innerHTML = element["Name"];
            streamButton.onclick = function () {
                showStream(element["PublishURL"]);
            };

            camera_list.appendChild(streamButton);

        });

    }, null);

}

showCameras();