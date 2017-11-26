// Write your JavaScript code.

// For Product Menu
$(document).ready(function () {
    $(".memenu").memenu();
});

//For Carousel in Home Page
(function () {
    $("#slider").responsiveSlides({
        auto: true,
        nav: true,
        speed: 500,
        namespace: "callbacks",
        pager: false,
    });
})();
