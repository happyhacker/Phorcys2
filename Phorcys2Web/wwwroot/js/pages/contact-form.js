var contactFormConfig = document.getElementById("contactFormConfig");
var contactIndexUrl = contactFormConfig ? contactFormConfig.dataset.indexUrl : "/";

window.redirectToIndex = function () {
    window.location.href = contactIndexUrl;
};
