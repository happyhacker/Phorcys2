var certificationIndexConfig = document.getElementById("certificationIndexConfig");
var certificationEditUrlBase = certificationIndexConfig ? certificationIndexConfig.dataset.editUrl : "";

window.onDelete = function (e) {
    e.preventDefault();
    var tr = $(e.target).closest("tr");
    var data = this.dataItem(tr);

    var diveId = data.DiveId;
    var diveNumber = data.DiveNumber;
    if (confirm("Are you sure you want to delete dive #" + diveNumber + "?")) {
        $("#hiddenDiveId").val(diveId);
        $("#hiddenDeleteForm").submit();
    }
};

window.onEdit = function (e) {
    e.preventDefault();
    var tr = $(e.target).closest("tr");
    var data = this.dataItem(tr);

    var diveId = data.DiveId;
    window.location.href = certificationEditUrlBase + "/" + diveId;
};
