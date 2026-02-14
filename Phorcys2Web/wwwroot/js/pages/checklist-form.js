var checklistFormConfig = document.getElementById("checklistFormConfig");
var checklistCancelUrl = checklistFormConfig ? checklistFormConfig.dataset.cancelUrl : "/";
var checklistExistingItemsId = checklistFormConfig ? checklistFormConfig.dataset.existingItemsId : "";

window.redirectToIndex = function () {
    window.location.href = checklistCancelUrl;
};

window.cancelAndExit = function () {
    window.location.href = checklistCancelUrl;
};

$(document).ready(function () {
    var grid = $("#checklistItemsGrid").data("kendoGrid");

    if (!grid) {
        return;
    }

    var existingItems = [];
    if (checklistExistingItemsId) {
        var existingItemsScript = document.getElementById(checklistExistingItemsId);
        if (existingItemsScript && existingItemsScript.textContent) {
            existingItems = JSON.parse(existingItemsScript.textContent);
        }
    }

    if (existingItems && existingItems.length > 0) {
        for (var i = 0; i < existingItems.length; i++) {
            grid.dataSource.add({
                SequenceNumber: existingItems[i].SequenceNumber || (i + 1),
                Title: existingItems[i].Title || ""
            });
        }
    }

    function ensureInitialRow() {
        if (grid.dataSource.total() === 0) {
            var newItem = grid.dataSource.add({
                SequenceNumber: 1,
                Title: ""
            });

            var row = grid.tbody.find("tr[data-uid='" + newItem.uid + "']");
            grid.editCell(row.find("td:eq(2)"));
        }
    }

    function getItemsInDisplayedOrder() {
        var orderedItems = [];

        grid.tbody.find("tr").each(function () {
            var dataItem = grid.dataItem(this);
            if (dataItem) {
                orderedItems.push(dataItem);
            }
        });

        return orderedItems;
    }

    function resequence() {
        var data = getItemsInDisplayedOrder();

        for (var i = 0; i < data.length; i++) {
            data[i].set("SequenceNumber", i + 1);
        }
    }

    ensureInitialRow();
    resequence();

    $("#btnAddItem").on("click", function (e) {
        e.preventDefault();

        grid.closeCell();

        var data = grid.dataSource.data();
        var nextSeq = data.length + 1;

        var newItem = grid.dataSource.add({
            SequenceNumber: nextSeq,
            Title: ""
        });

        resequence();

        var row = grid.tbody.find("tr[data-uid='" + newItem.uid + "']");
        grid.editCell(row.find("td:eq(2)"));
    });

    grid.dataSource.bind("change", function (e) {
        if (e && e.action === "remove") {
            resequence();
        }
    });

    grid.bind("rowReorder", function () {
        setTimeout(function () {
            resequence();
        }, 0);
    });

    $("#checklistItemsGrid").on("keydown", "input", function (e) {
        if (e.key === "Enter") {
            e.preventDefault();
            e.stopPropagation();

            var $input = $(e.target);
            var cell = $input.closest("td");
            var row = cell.closest("tr");

            var dataItem = grid.dataItem(row);

            var cellIndex = cell.index();
            var field = grid.columns[cellIndex] && grid.columns[cellIndex].field;

            if (field) {
                dataItem.set(field, $input.val());
            }

            grid.closeCell();

            var data = grid.dataSource.data();
            var nextSeq = data.length + 1;

            var newItem = grid.dataSource.add({
                SequenceNumber: nextSeq,
                Title: ""
            });

            resequence();

            var newRow = grid.tbody.find("tr[data-uid='" + newItem.uid + "']");
            grid.editCell(newRow.find("td:eq(2)"));
        }
    });

    $("#checklistForm").on("keydown", function (e) {
        if (e.key === "Enter" &&
            $(e.target).closest("#checklistItemsGrid").length > 0) {
            e.preventDefault();
        }
    });

    $("#checklistForm").on("submit", function () {
        grid.closeCell();

        var data = getItemsInDisplayedOrder();
        var items = [];

        for (var i = 0; i < data.length; i++) {
            var item = data[i];
            if (item.Title && item.Title.trim().length > 0) {
                items.push({
                    Title: item.Title,
                    SequenceNumber: i + 1
                });
            }
        }

        $("#ItemsJson").val(JSON.stringify(items));
    });
});
