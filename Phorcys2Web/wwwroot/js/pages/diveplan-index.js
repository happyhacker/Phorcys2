var divePlanIndexConfig = document.getElementById("divePlanIndexConfig");
var divePlanEditUrlBase = divePlanIndexConfig ? divePlanIndexConfig.dataset.editUrl : "";

window.onDelete = function (e) {
    e.preventDefault();
    var tr = $(e.target).closest("tr");
    var data = this.dataItem(tr);

    var divePlanId = data.DivePlanId;
    if (confirm("Are you sure you want to delete this plan?")) {
        $("#hiddenDivePlanId").val(divePlanId);
        $("#hiddenDeleteForm").submit();
    }
};

window.onEdit = function (e) {
    e.preventDefault();
    var tr = $(e.target).closest("tr");
    var data = this.dataItem(tr);

    var divePlanId = data.DivePlanId;
    window.location.href = divePlanEditUrlBase + "/" + divePlanId;
};
