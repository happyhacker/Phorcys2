var diveEditConfig = document.getElementById("diveEditConfig");
var diveEditIndexUrl = diveEditConfig ? diveEditConfig.dataset.indexUrl : "/";
var diveEditTanksUrl = diveEditConfig ? diveEditConfig.dataset.getTanksUrl : "";

window.redirectToIndex = function () {
    window.location.href = diveEditIndexUrl;
};

function isDarkMode() {
    return window.matchMedia && window.matchMedia("(prefers-color-scheme: dark)").matches;
}

function getScaleConfig() {
    var isBlackBackground = isDarkMode();
    return {
        min: 0,
        max: 5000,
        minorUnit: 100,
        majorUnit: 1000,
        startAngle: -30,
        endAngle: 210,
        minorTicks: { visible: true, color: isBlackBackground ? "#cccccc" : "#333333" },
        majorTicks: { visible: true, color: isBlackBackground ? "#ffffff" : "#000000" },
        labels: { color: isBlackBackground ? "#ffffff" : "#000000" }
    };
}

function renderTanks(tanks) {
    $("#tankGaugesContainer").empty();
    if (!tanks || !tanks.length) return;

    tanks.forEach(function (tank, index) {
        var startGaugeId = "tankGaugeStart_" + index;
        var endGaugeId = "tankGaugeEnd_" + index;
        var startSliderId = "tankSliderStart_" + index;
        var endSliderId = "tankSliderEnd_" + index;
        var startInputId = "tankStartPressure_" + index;
        var endInputId = "tankEndPressure_" + index;

        $("#tankGaugesContainer").append(
            "<div class=\"mb-5\">" +
            "  <div class=\"text-center\">" +
            "    <label><strong>" + (tank.GearTitle || "Tank") + "</strong></label>" +
            "    <div class=\"row flex-wrap justify-content-center gx-4\">" +
            "      <div class=\"col-md-6 col-12 d-flex flex-column align-items-center mb-4 px-3\">" +
            "        <p>Starting Pressure</p>" +
            "        <div id=\"" + startGaugeId + "\" style=\"width:200px;height:200px;\"></div>" +
            "        <div id=\"" + startSliderId + "\" style=\"width:200px;margin-top:10px;\"></div>" +
            "        <input type=\"hidden\" id=\"" + startInputId + "\" name=\"Tanks[" + index + "].StartingPressure\" />" +
            "        <div class=\"mt-3\" style=\"width: 200px;\">" +
            "          <div class=\"mb-2\">" +
            "            <label class=\"form-label small\">Gas Content:</label>" +
            "            <input type=\"text\" class=\"form-control form-control-sm\"" +
            "                   name=\"Tanks[" + index + "].GasContentTitle\"" +
            "                   value=\"" + (tank.GasContentTitle != null ? tank.GasContentTitle : "Air") + "\"" +
            "                   placeholder=\"Air, Nitrox, Trimix\" />" +
            "          </div>" +
            "          <div class=\"row\">" +
            "            <div class=\"col-6\">" +
            "              <label class=\"form-label small\">O2 %:</label>" +
            "              <input type=\"number\" class=\"form-control form-control-sm\"" +
            "                     name=\"Tanks[" + index + "].OxygenPercent\"" +
            "                     value=\"" + (tank.OxygenPercent != null ? tank.OxygenPercent : 21) + "\"" +
            "                     min=\"2\" max=\"100\" />" +
            "            </div>" +
            "            <div class=\"col-6\">" +
            "              <label class=\"form-label small\">He %:</label>" +
            "              <input type=\"number\" class=\"form-control form-control-sm\"" +
            "                     name=\"Tanks[" + index + "].HeliumPercent\"" +
            "                     value=\"" + (tank.HeliumPercent != null ? tank.HeliumPercent : 0) + "\"" +
            "                     min=\"0\" max=\"98\" />" +
            "            </div>" +
            "          </div>" +
            "        </div>" +
            "      </div>" +
            "      <div class=\"col-md-6 col-12 d-flex flex-column align-items-center mb-4 px-3\">" +
            "        <p>Ending Pressure</p>" +
            "        <div id=\"" + endGaugeId + "\" style=\"width:200px;height:200px;\"></div>" +
            "        <div id=\"" + endSliderId + "\" style=\"width:200px;margin-top:10px;\"></div>" +
            "        <input type=\"hidden\" id=\"" + endInputId + "\" name=\"Tanks[" + index + "].EndingPressure\" />" +
            "        <div class=\"mt-3\" style=\"width: 200px;\">" +
            "          <label for=\"fillDate_" + index + "\" class=\"form-label small\">Fill Date:</label>" +
            "          <input id=\"fillDate_" + index + "\" name=\"Tanks[" + index + "].FillDate\" />" +
            "        </div>" +
            "        <div class=\"mt-2\" style=\"width: 200px;\">" +
            "          <label for=\"fillCost_" + index + "\" class=\"form-label small\">Fill Cost:</label>" +
            "          <input id=\"fillCost_" + index + "\" name=\"Tanks[" + index + "].FillCost\" />" +
            "        </div>" +
            "      </div>" +
            "    </div>" +
            "    <input type=\"hidden\" name=\"Tanks[" + index + "].GearId\" value=\"" + tank.GearId + "\" />" +
            "  </div>" +
            "</div>"
        );

        var startGauge = $("#" + startGaugeId).kendoRadialGauge({
            pointer: { value: tank.StartingPressure || 0 },
            scale: getScaleConfig()
        }).data("kendoRadialGauge");

        var endGauge = $("#" + endGaugeId).kendoRadialGauge({
            pointer: { value: tank.EndingPressure || 0 },
            scale: getScaleConfig()
        }).data("kendoRadialGauge");

        $("#" + startSliderId).kendoSlider({
            min: 0,
            max: 5000,
            smallStep: 100,
            largeStep: 1000,
            value: tank.StartingPressure || 0,
            showButtons: false,
            tooltip: { enabled: true },
            slide: function (e) { startGauge.value(e.value); $("#" + startInputId).val(e.value); },
            change: function (e) { startGauge.value(e.value); $("#" + startInputId).val(e.value); }
        });

        $("#" + endSliderId).kendoSlider({
            min: 0,
            max: 5000,
            smallStep: 100,
            largeStep: 1000,
            value: tank.EndingPressure || 0,
            showButtons: false,
            tooltip: { enabled: true },
            slide: function (e) { endGauge.value(e.value); $("#" + endInputId).val(e.value); },
            change: function (e) { endGauge.value(e.value); $("#" + endInputId).val(e.value); }
        });

        $("#" + startInputId).val(tank.StartingPressure || 0);
        $("#" + endInputId).val(tank.EndingPressure || 0);

        $("#fillDate_" + index).kendoDatePicker({
            format: "yyyy-MM-dd",
            parseFormats: ["yyyy-MM-dd", "MM/dd/yyyy", "yyyy-MM-ddTHH:mm:ss", "yyyy-MM-ddTHH:mm"],
            value: tank.FillDate ? kendo.parseDate(tank.FillDate) : null
        });

        $("#fillCost_" + index).kendoNumericTextBox({
            min: 0,
            step: 0.01,
            format: "n2",
            value: (tank.FillCost != null) ? tank.FillCost : null
        });
    });
}

window.onDivePlanChange = function () {
    var divePlanDropdown = $("#DivePlanSelectedId").data("kendoDropDownList");
    var divePlanId = divePlanDropdown ? divePlanDropdown.value() : null;
    if (!divePlanId) return;

    $.ajax({
        url: diveEditTanksUrl,
        type: "GET",
        data: { divePlanId: divePlanId },
        success: function (tanks) { renderTanks(tanks); },
        error: function (err) { console.error("Failed to load tanks", err); }
    });
};

$(function () {
    var divePlanDropdown = $("#DivePlanSelectedId").data("kendoDropDownList");
    var divePlanId = divePlanDropdown ? divePlanDropdown.value() : null;
    if (!divePlanId) return;

    $.ajax({
        url: diveEditTanksUrl,
        type: "GET",
        data: { divePlanId: divePlanId },
        success: function (tanks) { renderTanks(tanks); },
        error: function (err) { console.error("Failed to load tanks on page load", err); }
    });
});

if (window.matchMedia) {
    window.matchMedia("(prefers-color-scheme: dark)").addEventListener("change", function () {
    });
}
