"use strict"

var autocomplete = null;
var txtAutocomplete = null;
var textLatitude = null;
var textLongitude = null;	

window.autoCompleteAddress = {
    
    // Initialize autocomplete
    initAutocomplete: function (textAutoCompleteID, textLatitudeID, textLongitudeID) {
        txtAutocomplete = document.getElementById(textAutoCompleteID);
        textLatitude = document.getElementById(textLatitudeID);
        textLongitude = document.getElementById(textLongitudeID);

        if (txtAutocomplete) {

            autocomplete = new google.maps.places.Autocomplete(txtAutocomplete);

            google.maps.event.clearInstanceListeners(textAutoCompleteID);

            autocomplete.addListener('place_changed', function () {

                var place = autocomplete.getPlace();

                if (place.geometry != undefined) {
                    autoCompleteAddress.set_Latitude_Longitude(autoCompleteAddress.setFixedLength(place.geometry.location.lat()), autoCompleteAddress.setFixedLength(place.geometry.location.lng()));
                }
            });

            txtAutocomplete.addEventListener('change', function () {
                autoCompleteAddress.set_Latitude_Longitude('', '');
            });
        }
    },

    // Set latitude and longitude
    set_Latitude_Longitude: function (latitude, longitude) {
        try {
            textLatitude.value = latitude;
            textLongitude.value = longitude;
        }
        catch (e) {
        }
    },

    // Set coordinate length to 6
    setFixedLength: function (coordinate) {
        try {
            var decimalplaces = coordinate.toString().split(".");
            if (decimalplaces != null && decimalplaces.length > 1 && decimalplaces[1].length > 5) {
                coordinate = coordinate.toFixed(6);
            }
        }
        catch (e) {
        }
        return coordinate;
    }
}
