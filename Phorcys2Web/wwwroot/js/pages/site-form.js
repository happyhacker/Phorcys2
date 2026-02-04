var siteFormConfig = document.getElementById("siteFormConfig");
var siteIndexUrl = siteFormConfig ? siteFormConfig.dataset.indexUrl : "/";

window.redirectToIndex = function () {
    window.location.href = siteIndexUrl;
};
