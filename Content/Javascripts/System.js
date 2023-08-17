



$(function () {
    //Remove the style attributes.
    $(".navbar-nav li, .navbar-nav a, .navbar-nav ul").removeAttr('style');

    //Apply the Bootstrap class to the Submenu.
    $(".dropdown-menu").closest("li").removeClass().addClass("dropdown-toggle");

    //Apply the Bootstrap properties to the Submenu.
    $(".dropdown-toggle").find("a").eq(0).attr("data-toggle", "dropdown").attr("aria-haspopup", "true").attr("aria-expanded", "false").append("<span class='caret'></span>");

    ////Apply the Bootstrap "active" class to the selected Menu item.
    //$("a.selected").closest("li").addClass("active");
    //$("a.selected").closest(".dropdown-toggle").addClass("active");
});



$(document).bind("ajaxStart", function () {
    $("#ldrdiv").show();
}).bind("ajaxStop", function () {
    $("#ldrdiv").hide();
});

function Loader() {
    $('.blackscreen').show();
    $('.loader_popup').show();
}


