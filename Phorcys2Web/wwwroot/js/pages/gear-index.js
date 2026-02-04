var gearIndexConfig = document.getElementById("gearIndexConfig");
var gearEditUrlBase = gearIndexConfig ? gearIndexConfig.dataset.editUrl : "";

window.onDelete = function (e) {
    e.preventDefault();
    var tr = $(e.target).closest("tr");
    var data = this.dataItem(tr);

    var gearId = data.GearId;
    if (confirm("Are you sure you want to delete this gear? " + gearId)) {
        $("#hiddenGearId").val(gearId);
        $("#hiddenDeleteForm").submit();
    }
};

window.onEdit = function (e) {
    e.preventDefault();
    var tr = $(e.target).closest("tr");
    var data = this.dataItem(tr);

    var gearId = data.GearId;
    window.location.href = gearEditUrlBase + "/" + gearId;
};
