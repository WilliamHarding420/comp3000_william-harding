


function submitCameraDetails(edit = false, editCamID = 0) {

    let camName = document.getElementById("camNameInput").value;
    let camIP = document.getElementById("camIPInput").value;
    let camPort = document.getElementById("camPortInput").value;
    let camURL = document.getElementById("camURLInput").value;
    let camPublish = document.getElementById("camPublishInput").value;

    let formData = {
        name: camName,
        ip: camIP,
        port: camPort,
        camurl: camURL,
        publishurl: camPublish,
        LocationID: localStorage.getItem("currentFolderID")
    }

    let postURL = "/cctv/new";

    if (edit == true) {
        postURL = "/cctv/update/" + editCamID;
    }

    POST_Request_Auth(postURL, formData, function (response) {

        alert("Camera " + camName + " details submitted.");
        loadHTMLToID('/pages/cctv.html', 'module-content', 'cctv.js');

    }, function (xhr, response) {

        let json = JSON.parse(xhr["responseJSON"]);

        alert("Error: " + json["error"]);

    });

}

function initialize() {

    let submitButton = document.getElementById("addCamButton");

    if (localStorage.getItem("editing") != null) {

        let camera = JSON.parse(localStorage.getItem("editing"));
        localStorage.removeItem("editing");

        document.getElementById("camNameInput").value = camera["Name"];
        document.getElementById("camIPInput").value = camera["IP"];
        document.getElementById("camPortInput").value = camera["Port"];
        document.getElementById("camURLInput").value = camera["StreamURL"];
        document.getElementById("camPublishInput").value = camera["PublishURL"];

        document.getElementById("cameraDetailsTitle").innerHTML = "Edit Details";
        submitButton.innerHTML = "Submit Details";

        submitButton.onclick = function () {
            submitCameraDetails(true, camera["CameraID"]);
        }

    } else {
        submitButton.onclick = submitCameraDetails;
    }

}

initialize();
