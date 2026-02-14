var myCertificationIndexConfig = document.getElementById("myCertificationIndexConfig");
var myCertificationEditUrlBase = myCertificationIndexConfig ? myCertificationIndexConfig.dataset.editUrl : "";

window.onDelete = function (e) {
    e.preventDefault();
    var tr = $(e.target).closest("tr");
    var data = this.dataItem(tr);
    var diverCertificationId = data.DiverCertificationId;
    if (confirm("Are you sure you want to delete this Certification? " + diverCertificationId)) {
        $("#hiddenDiverCertificationId").val(diverCertificationId);
        $("#hiddenDeleteForm").submit();
    }
};

window.onEdit = function (e) {
    e.preventDefault();
    var tr = $(e.target).closest("tr");
    var data = this.dataItem(tr);
    var diverCertificationId = data.DiverCertificationId;
    window.location.href = myCertificationEditUrlBase + "/" + diverCertificationId;
};
