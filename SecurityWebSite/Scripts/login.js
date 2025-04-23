
let username = document.getElementById("usernameInput");
let password = document.getElementById("passwordInput");

let submitButton = document.getElementById("submitButton");

submitButton.onclick = function() {

    let formData = {
        username: username.value,
        password: password.value
    }

    POST_Request("/auth/login", formData, function (response) {

        let jsonResponse = JSON.parse(response);

        localStorage.setItem("authToken", jsonResponse["token"])

        loadHTML("/pages/home.html", document.body);

    }, function (xhr, response) {

        let json = JSON.parse(xhr["responseJSON"]);

        alert("Error: " + json["error"]);

    });

}