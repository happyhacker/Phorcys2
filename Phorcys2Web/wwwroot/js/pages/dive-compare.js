var diveCompareConfig = document.getElementById("diveCompareConfig");
var diveCompareIndexUrl = diveCompareConfig ? diveCompareConfig.dataset.indexUrl : "/";

window.redirectToIndex = function () {
    window.location.href = diveCompareIndexUrl;
};

function isDarkMode() {
    return window.matchMedia && window.matchMedia("(prefers-color-scheme: dark)").matches;
}

function applyRowColors() {
    var grid = $("#CompareGrid").data("kendoGrid");
    if (!grid) return;

    var darkMode = isDarkMode();
    var matchColor = darkMode ? "rgba(40, 167, 69, 0.25)" : "rgba(40, 167, 69, 0.15)";
    var diffColor = darkMode ? "rgba(255, 193, 7, 0.35)" : "rgba(255, 193, 7, 0.20)";

    grid.tbody.find("tr").each(function () {
        var dataItem = grid.dataItem($(this));
        if (!dataItem) return;

        if (dataItem.HasPlanValue) {
            var color = dataItem.IsMatch ? matchColor : diffColor;
            $(this).find("td").css("background-color", color);
        } else {
            $(this).find("td").css("background-color", "");
        }

        if (dataItem.Field === "Notes") {
            $(this).find("td").css("vertical-align", "top");
        }
    });
}

window.onCompareDataBound = function () {
    applyRowColors();
};

if (window.matchMedia) {
    window.matchMedia("(prefers-color-scheme: dark)").addEventListener("change", function () {
        applyRowColors();
    });
}
