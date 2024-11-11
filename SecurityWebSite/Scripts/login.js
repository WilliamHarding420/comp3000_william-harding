﻿
let username = document.getElementById("usernameInput");
let password = document.getElementById("passwordInput");

let submitButton = document.getElementById("submitButton");

submitButton.onclick = function() {

    let formData = {
        username: username.value,
        password: password.value
    }

    POST_Request("/auth/login", formData, function (response) {

        loadHTML("/pages/home.html", document.body);

    }, function (response) {

        alert("Invalid credentials.");

    });

}