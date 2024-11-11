function selectLocation(country, state, city) {
    if (country !== null) {
        setTimeout(function () {
            var countryDropdown = $('#country');

            countryDropdown.find('option').filter(function () {
                return $(this).text() === country;
            }).prop('selected', true);

            setTimeout(function () {
                stateDropdownFunction(); 

                if (state !== null) {
                    setTimeout(function () {
                        var stateDropdown = $('#state');

                        stateDropdown.find('option').filter(function () {
                            return $(this).text() === state;
                        }).prop('selected', true);

                        setTimeout(function () {
                            cityDropdownFunction(); 

                            if (city !== null) {
                                setTimeout(function () {
                                    var cityDropdown = $('#city');

                                    cityDropdown.find('option').filter(function () {
                                        return $(this).text() === city;
                                    }).prop('selected', true);
                                }, 1000); 
                            }
                        }, 1000); 
                    }, 200); 
                }
            }, 200); 
        }, 200);
    }
}

// Usage
var Country = '@Model.Country';
var State = '@Model.State';
var City = '@Model.City';
selectLocation(Country, State, City);


    function loadCountries() {
        $.ajax({
            url: '/Administration/Location/GetCountries',
            method: 'GET',
            dataType: 'json',
            success: function (data) {
                var countryDropdown = $('#country');
                countryDropdown.empty();
                countryDropdown.append('<option value="">Select Country</option>');

                $.each(data, function (i, country) {
/*                    countryDropdown.append('<option value="' + country.id + '">' + country.name + '</option>');
*/countryDropdown.append('<option value="' + country.name + '" data-code="' + country.id + '">' + country.name + '</option>');

                });
            },
            error: function (error) {
                console.log('Error loading countries:', error);
            }
        });
    }
$(document).ready(function () {
    loadCountries();
});




$('#country').change(function () {
    stateDropdownFunction();
});

function stateDropdownFunction() {

    var countryId = $('#country').find('option:selected').data('code'); // This gets the data-code

    $.ajax({
        url: '/Administration/Location/GetStates?countryId=' + countryId,
        method: 'GET',
        dataType: 'json',
        success: function (data) {
            var stateDropdown = $('#state');
            stateDropdown.empty();
            stateDropdown.append('<option value="">Select State</option>');

            $.each(data, function (i, state) {
                /*                stateDropdown.append('<option value="' + state.id + '">' + state.name + '</option>');
                */
                stateDropdown.append('<option value="' + state.name + '" data-code="' + state.id + '">' + state.name + '</option>');

            });
        },
        error: function (error) {
            console.log('Error loading countries:', error);
        }
    });
    }

$('#state').change(function () {

    cityDropdownFunction();
});

function cityDropdownFunction() {
    var stateId = $('#state').find('option:selected').data('code'); // This gets the data-code

    $.ajax({
        url: '/Administration/Location/GetCities?stateId=' + stateId,
        method: 'GET',
        dataType: 'json',
        success: function (data) {
            var cityDropdown = $('#city');
            cityDropdown.empty();
            cityDropdown.append('<option value="">Select City</option>');

            $.each(data, function (i, city) {
                cityDropdown.append('<option value="' + city.name + '" data-code="' + city.id + '">' + city.name + '</option>');
            });
        },
        error: function (error) {
            console.log('Error loading countries:', error);
        }
    });
}