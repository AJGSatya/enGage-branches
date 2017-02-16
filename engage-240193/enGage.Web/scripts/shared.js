function resetValidationStyle() {

    $(".validation-err, .validation-warn").removeClass("validation-err").removeClass("validation-warn");

}

$.urlParam = function(name) {
    var results = new RegExp('[\\?&amp;]' + name + '=([^&amp;#]*)').exec(window.location.href);
    return results[1] || 0;
}