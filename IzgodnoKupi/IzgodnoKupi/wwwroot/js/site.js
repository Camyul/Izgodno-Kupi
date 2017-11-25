// Write your JavaScript code.

//For Carousel in Home Page
$(document).ready(function () {
    $("#slider").responsiveSlides({
        auto: true,
        nav: true,
        speed: 500,
        namespace: "callbacks",
        pager: false,
    });
});

// For Product Menu
$(document).ready(function () {
    $(".memenu").memenu();
});