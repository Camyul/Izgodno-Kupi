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

function displayContactInfo() {
    $('#order-now').addClass('hidden');
    $('#check-out').removeClass('hidden');
    $('.cart-items').fadeOut('slow', function (c) {
        $('.cart-items').addClass('hidden');
    });
    $('#contact-info').fadeIn('slow', function (c) {
        $('#contact-info').removeClass('hidden');
    });
}