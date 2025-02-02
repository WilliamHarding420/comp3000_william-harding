
document.getElementById("addCamButton").onclick = function () {

    let camName = document.getElementById("camNameInput").value;
    let camIP = document.getElementById("camIPInput").value;
    let camPort = document.getElementById("camPortInput").value;
    let camPublish = document.getElementById("camPublishInput").value;

    let formData = {
        name: camName,
        ip: camIP,
        port: camPort,
        publishurl: camPublish,
    }

    POST_Request("/cctv/new", formData, function (response) {

        alert("Camera " + camName + " added.");
        loadHTMLToID('/pages/cctv.html', 'module-content', 'cctv.js');

    }, function (xhr, response) {

        let json = JSON.parse(xhr["responseJSON"]);

        alert("Error: " + json["error"]);

    });

};
