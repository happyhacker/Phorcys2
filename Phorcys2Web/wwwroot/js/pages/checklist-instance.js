var checklistInstanceConfig = document.getElementById("checklistInstanceConfig");
var checklistIndexUrl = checklistInstanceConfig ? checklistInstanceConfig.dataset.indexUrl : "/";

window.redirectToIndex = function () {
    window.location.href = checklistIndexUrl;
};

window.onChecklistCheckedChange = function (cb) {
    var grid = $("#ChecklistInstanceGrid").data("kendoGrid");
    var item = grid.dataItem($(cb).closest("tr"));
    if (!item) {
        return;
    }

    item.set("IsChecked", cb.checked);
};
