var locationIndexConfig = document.getElementById("locationIndexConfig");
var locationEditUrlBase = locationIndexConfig ? locationIndexConfig.dataset.editUrl : "";

window.onDelete = function (e) {
    e.preventDefault();
    var tr = $(e.target).closest("tr");
    var data = this.dataItem(tr);

    var diveLocationId = data.DiveLocationId;
    if (confirm("Are you sure you want to delete this location? " + diveLocationId)) {
        $("#hiddenDiveLocationId").val(diveLocationId);
        $("#hiddenDeleteForm").submit();
    }
};

window.onEdit = function (e) {
    e.preventDefault();
    var tr = $(e.target).closest("tr");
    var data = this.dataItem(tr);

    var diveLocationId = data.DiveLocationId;
    window.location.href = locationEditUrlBase + "/" + diveLocationId;
};

window.isVisible = function (data) {
    if (data.LoggedIn == "system") {
        return true;
    }
    return data.UserName != "System";
};
