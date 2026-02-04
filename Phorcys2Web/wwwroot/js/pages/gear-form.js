var gearFormConfig = document.getElementById("gearFormConfig");
var gearIndexUrl = gearFormConfig ? gearFormConfig.dataset.indexUrl : "/";

window.redirectToIndex = function () {
    window.location.href = gearIndexUrl;
};
