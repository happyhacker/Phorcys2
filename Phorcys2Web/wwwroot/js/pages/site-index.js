var siteIndexConfig = document.getElementById("siteIndexConfig");
var siteEditUrlBase = siteIndexConfig ? siteIndexConfig.dataset.editUrl : "";

window.onDelete = function (e) {
    e.preventDefault();
    var tr = $(e.target).closest("tr");
    var data = this.dataItem(tr);
    var diveSiteId = data.DiveSiteId;
    if (data.UserName == "System") {
        alert("You may not delete a System Dive Site");
    } else {
        if (confirm("Are you sure you want to delete site #" + diveSiteId + "?")) {
            $("#hiddenDiveSiteId").val(diveSiteId);
            $("#hiddenDeleteForm").submit();
        }
    }
};

window.onEdit = function (e) {
    e.preventDefault();
    var tr = $(e.target).closest("tr");
    var data = this.dataItem(tr);
    var diveSiteId = data.DiveSiteId;
    window.location.href = siteEditUrlBase + "/" + diveSiteId;
};

window.isVisible = function (data) {
    if (data.LoggedIn == "system") {
        return true;
    }
    return data.UserName != "System";
};
