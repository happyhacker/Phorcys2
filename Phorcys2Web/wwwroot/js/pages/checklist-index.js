var checklistIndexConfig = document.getElementById("checklistIndexConfig");
var checklistEditUrlBase = checklistIndexConfig ? checklistIndexConfig.dataset.editUrl : "";
var checklistViewUrlBase = checklistIndexConfig ? checklistIndexConfig.dataset.viewUrl : "";

window.onEditChecklist = function (e) {
    e.preventDefault();

    var grid = $("#Grid").data("kendoGrid");
    var tr = $(e.target).closest("tr");
    var data = grid.dataItem(tr);

    var checklistId = data.ChecklistId;
    window.location.href = checklistEditUrlBase + "/" + checklistId;
};

window.onCheckList = function (e) {
    e.preventDefault();

    var grid = $("#Grid").data("kendoGrid");
    var tr = $(e.target).closest("tr");
    var data = grid.dataItem(tr);

    var checklistId = data.ChecklistId;
    window.location.href = checklistViewUrlBase + "/" + checklistId;
};

window.onDeleteChecklist = function (e) {
    e.preventDefault();

    var grid = $("#Grid").data("kendoGrid");
    var tr = $(e.target).closest("tr");
    var data = grid.dataItem(tr);

    var checklistId = data.ChecklistId;
    var title = data.Title;

    if (!confirm("Are you sure you want to delete checklist '" + title + "'?")) {
        return;
    }

    var tokenInput = document.querySelector("input[name='__RequestVerificationToken']");
    var token = tokenInput ? tokenInput.value : "";

    fetch("/Checklist/Delete", {
        method: "POST",
        headers: {
            "Content-Type": "application/x-www-form-urlencoded",
            "RequestVerificationToken": token
        },
        body: "checklistId=" + encodeURIComponent(checklistId) +
            "&__RequestVerificationToken=" + encodeURIComponent(token)
    })
        .then(response => {
            if (response.ok) {
                window.location.reload();
            } else {
                alert("Error deleting checklist.");
            }
        })
        .catch(err => {
            console.error(err);
            alert("An error occurred while deleting the checklist.");
        });
};
