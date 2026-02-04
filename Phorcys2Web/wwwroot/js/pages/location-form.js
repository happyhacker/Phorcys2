var locationFormConfig = document.getElementById("locationFormConfig");
var locationIndexUrl = locationFormConfig ? locationFormConfig.dataset.indexUrl : "/";

window.redirectToIndex = function () {
    window.location.href = locationIndexUrl;
};
