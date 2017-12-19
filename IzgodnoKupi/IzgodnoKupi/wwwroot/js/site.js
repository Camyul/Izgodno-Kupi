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
};

function dataForCompany() {
   $('.compani-info').toggleClass('hidden');
};

//Modal
window.onload = function () {
    var modal = new RModal(document.getElementById('modal'), {
        //content: 'Abracadabra'
        beforeOpen: function (next) {
            console.log('beforeOpen');
            next();
        }
        , afterOpen: function () {
            console.log('opened');
        }

        , beforeClose: function (next) {
            console.log('beforeClose');
            next();
        }
        , afterClose: function () {
            console.log('closed');
        }
        // , bodyClass: 'modal-open'
        // , dialogClass: 'modal-dialog'
        // , dialogOpenClass: 'animated fadeIn'
        // , dialogCloseClass: 'animated fadeOut'

        // , focus: true
        // , focusElements: ['input.form-control', 'textarea', 'button.btn-primary']

        // , escapeClose: true
    });

    window.modal = modal;
}
//Modal

//Register checkbox
