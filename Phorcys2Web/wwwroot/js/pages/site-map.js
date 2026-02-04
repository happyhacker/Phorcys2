var siteMapConfig = document.getElementById("siteMapConfig");
var siteMapLocationsId = siteMapConfig ? siteMapConfig.dataset.locationsId : "";

window.initMap = function () {
    var center = { lat: 27.994402, lng: -81.760254 };
    var map = new google.maps.Map(document.getElementById("map"), {
        zoom: 6,
        center: center,
        mapId: "1d8e97e4e5ab63ff"
    });

    var locations = [];
    if (siteMapLocationsId) {
        var locationsScript = document.getElementById(siteMapLocationsId);
        if (locationsScript && locationsScript.textContent) {
            locations = JSON.parse(locationsScript.textContent);
        }
    }

    locations.forEach(function (location) {
        var marker = new google.maps.Marker({
            position: { lat: location.Lat, lng: location.Lng },
            map: map,
            title: location.Title
        });

        var infoWindow = new google.maps.InfoWindow({
            content:
                "<div style=\"color: black; background-color: white; padding: 10px; border-radius: 5px;\">" +
                "  <h5 style=\"margin: 0;\">" + location.Title + "</h5>" +
                "  <p style=\"margin: 5px 0 0;\">" + location.Description + "</p>" +
                "</div>"
        });

        marker.addListener("click", function () {
            infoWindow.open({
                anchor: marker,
                map: map
            });
        });
    });
};
