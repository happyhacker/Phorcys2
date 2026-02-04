var divePlanFormConfig = document.getElementById("divePlanFormConfig");
var divePlanIndexUrl = divePlanFormConfig ? divePlanFormConfig.dataset.indexUrl : "/";

window.redirectToIndex = function () {
    window.location.href = divePlanIndexUrl;
};
