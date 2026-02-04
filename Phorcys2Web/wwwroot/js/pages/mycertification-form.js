var myCertificationFormConfig = document.getElementById("myCertificationFormConfig");
var myCertificationIndexUrl = myCertificationFormConfig ? myCertificationFormConfig.dataset.indexUrl : "/";

window.redirectToIndex = function () {
    window.location.href = myCertificationIndexUrl;
};

window.AdjustCertList = function () {
    console.log("AdjustCertList called");

    var mainForm = document.getElementById("TheForm");
    var agencyForm = document.getElementById("AgencyForm");
    console.log("mainForm:", mainForm);
    console.log("agencyForm:", agencyForm);

    if (!mainForm) {
        alert("Main form not found");
        return;
    }
    if (!agencyForm) {
        alert("Agency form not found");
        return;
    }

    var diveAgencyId = mainForm.elements["DiveAgencyId"].value;
    console.log("diveAgencyId:", diveAgencyId);

    agencyForm.elements["HiddenDiveAgencyId"].value = diveAgencyId;
    console.log("agencyForm HiddenDiveAgencyId:", agencyForm.elements["HiddenDiveAgencyId"].value);

    agencyForm.submit();
};

document.addEventListener("DOMContentLoaded", function () {
    console.log("DOMContentLoaded event fired");

    var agencyFormCheck = document.getElementById("AgencyForm");
    if (agencyFormCheck) {
        console.log("AgencyForm found during DOMContentLoaded:", agencyFormCheck);
    } else {
        console.log("AgencyForm NOT found during DOMContentLoaded");
        alert("AgencyForm NOT found during DOMContentLoaded");
    }

    var dropdown = $("#DiveAgencyId").data("kendoDropDownList");
    if (dropdown) {
        console.log("Dropdown found, binding change event");
        dropdown.bind("change", AdjustCertList);
    } else {
        console.log("Dropdown element not found");
    }
});
