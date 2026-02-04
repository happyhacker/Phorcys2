var contactIndexConfig = document.getElementById("contactIndexConfig");
var contactEditUrlBase = contactIndexConfig ? contactIndexConfig.dataset.editUrl : "";

window.onDelete = function (e) {
    e.preventDefault();
    var tr = $(e.target).closest("tr");
    var data = this.dataItem(tr);

    var contactId = data.ContactId;
    if (confirm("Are you sure you want to delete this contact? " + contactId)) {
        $("#hiddenContactId").val(contactId);
        $("#hiddenDeleteForm").submit();
    }
};

window.onEdit = function (e) {
    e.preventDefault();
    var tr = $(e.target).closest("tr");
    var data = this.dataItem(tr);

    var contactId = data.ContactId;
    window.location.href = contactEditUrlBase + "/" + contactId;
};

window.isVisible = function (data) {
    if (data.LoggedIn == "system") {
        return true;
    }
    return data.UserName != "System";
};
