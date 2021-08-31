// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const API_KEY = '0dd9af9e62914962bee54928200709';
let isHot;
let isDay;

$.get(`https://api.weatherapi.com/v1/current.json?key=${API_KEY}&q=Tel-Aviv`, (data, status) => {
    console.log(data.current.temp_c);
    if (data.current.temp_c > 22) {
        isHot = true;
    } else {
        isHot = false;
    }

    $('#weather-img').attr('src', 'https:' + data.current.condition.icon);
    $('#weather').html(`${data.current.temp_c}°C `)

    if (data.current.is_day === 1) {
        isDay = false;
        prefixString = 'Good Day, '
    } else {
        isDay = true;
        prefixString = 'Good Night, '
    }

    $('#greetings').html(`${prefixString}${$('#greetings').html()}`);
});