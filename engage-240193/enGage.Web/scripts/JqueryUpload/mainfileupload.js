/*
* jQuery File Upload Plugin JS Example 5.0.2
* https://github.com/blueimp/jQuery-File-Upload
*
* Copyright 2010, Sebastian Tschan
* https://blueimp.net
*
* Licensed under the MIT license:
* http://creativecommons.org/licenses/MIT/
*/

/*jslint nomen: true */
/*global $ */

$(function() {
    'use strict';

    // Initialize the jQuery File Upload widget:
    $('#fileupload').fileupload({

});

// Load existing files:
//    $.getJSON($('#fileupload form').prop('action'), function (files) {
//        var fu = $('#fileupload').data('fileupload');
//        fu._adjustMaxNumberOfFiles(-files.length);
//        fu._renderDownload(files)
//            .appendTo($('#fileupload .files'))
//            .fadeIn(function () {
//                // Fix for IE7 and lower:
//                $(this).show();
//            });
//    });
// Load existing files:
$('#fileupload').addClass('fileupload-processing');
$.ajax({
    // Uncomment the following to send cross-domain cookies:
    //xhrFields: {withCredentials: true},
    url: $('#fileupload').fileupload('option', 'url'),
    dataType: 'json',
    context: $('#fileupload')[0]
}).always(function() {
    $(this).removeClass('fileupload-processing');
}).done(function(result) {
    $(this).fileupload('option', 'done')
                .call(this, $.Event('done'), { result: result });
});

$('#fileupload').bind('fileuploaddone', function(e, data) {
    if (data.jqXHR.responseText || data.result) {
        var fu = $('#fileupload').data('fileupload');
        if (fu != null && fu != "undefined") {
            var JSONjQueryObject = (data.jqXHR.responseText) ? jQuery.parseJSON(data.jqXHR.responseText) : data.result;
            fu._adjustMaxNumberOfFiles(JSONjQueryObject.files.length);
            //                debugger;
            fu._renderDownload(JSONjQueryObject.files)
                .appendTo($('#fileupload .files'))
                .fadeIn(function() {
                    // Fix for IE7 and lower:
                    $(this).show();
                });
        }
    }
});

//    // Open download dialogs via iframes,
//    // to prevent aborting current uploads:
//    $('#fileupload .files a:not([target^=_blank])').live('click', function (e) {
//        e.preventDefault();
//        $('<iframe style="display:none;"></iframe>')
//            .prop('src', this.href)
//            .appendTo('body');
//    });

});
function openUploadDialog(button) {
    $(button).magnificPopup({
        items: {
            src: '#fileuploadDiv',
            type: 'inline'
        }
    });
}


$(document).ready(function() {
   // openUploadDialog("#open");
});
