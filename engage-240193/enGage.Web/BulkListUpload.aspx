<%@ Page Title="" Language="C#"  AutoEventWireup="true"    CodeBehind="BulkListUpload.aspx.cs" Inherits="enGage.Web.BulkListUpload" %>


    <!-- Jquery Upload -->
    <link id="theme" href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.10.3/themes/flick/jquery-ui.css"
        rel="stylesheet" />
    <link rel="stylesheet" href="css/JqueryUpload/jquery.fileupload.css">
    <link rel="stylesheet" href="css/JqueryUpload/jquery.fileupload-ui.css">
    
    
    
    
    
    <div class="page">
        <table class="page-table" width="100%">
            <tr>
                <td >
                     <div id="fileuploadDiv" class="">
       
            <form id="fileupload" action="BulkListUpload/Handler.ashx" method="POST" enctype="multipart/form-data">
    <!-- Redirect browsers with JavaScript disabled to the origin page -->
    <noscript><input type="hidden" name="redirect" value="http://blueimp.github.io/jQuery-File-Upload/"></noscript>
    <!-- The fileupload-buttonbar contains buttons to add/delete files and start/cancel the upload -->
    <div class="fileupload-buttonbar">
        <div class="fileupload-buttons">
            <!-- The fileinput-button span is used to style the file input field as button -->
            <span class="fileinput-button">
                <span>Add files...</span>
                <input type="file" name="files[]" multiple>
            </span>
            <button type="submit" class="start">Start upload</button>
            <button type="reset" class="cancel">Cancel upload</button>
            <button type="submit" class="delete">Delete</button>
            <input type="checkbox" class="toggle">
            <!-- The global file processing state -->
            <span class="fileupload-process"></span>
        </div>
        <!-- The global progress state -->
        <div class="fileupload-progress fade" style="display:none">
            <!-- The global progress bar -->
            <div class="progress" role="progressbar" aria-valuemin="0" aria-valuemax="100"></div>
            <!-- The extended global progress state -->
            <div class="progress-extended">&nbsp;</div>
        </div>
    </div>
    <!-- The table listing the files available for upload/download -->
    <table role="presentation"><tbody class="files"></tbody></table>
</form>
       
    </div>
                </td>
            </tr>

        </table>
        
    </div>
    
    
<!-- The template to display files available for upload -->
<script id="template-upload" type="text/x-tmpl">
{% for (var i=0, file; file=o.files[i]; i++) { %}
    <tr class="template-upload fade">
        <td>
            <span class="preview"></span>
        </td>
        <td>
            <p class="name">{%=file.name%}</p>
            <strong class="error"></strong>
        </td>
        <td>
            <p class="size">Processing...</p>
            <div class="progress"></div>
        </td>
        <td>
            {% if (!i && !o.options.autoUpload) { %}
                <button class="start">Start</button>
            {% } %}
            {% if (!i) { %}
                <button class="cancel">Cancel</button>
            {% } %}
        </td>
    </tr>
{% } %}
</script>
<!-- The template to display files available for download -->
<script id="template-download" type="text/x-tmpl">
{% for (var i=0, file; file=o.files[i]; i++) { %}
    <tr class="template-download fade">
        <td>
            <span class="preview">
                {% if (file.thumbnailUrl) { %}
                    <a href="{%=file.url%}" title="{%=file.name%}" download="{%=file.name%}" data-gallery><img src="{%=file.thumbnailUrl%}"></a>
                {% } %}
            </span>
        </td>
        <td>
            <p class="name">
                <a href="{%=file.url%}" title="{%=file.name%}" download="{%=file.name%}" {%=file.thumbnailUrl?'data-gallery':''%}>{%=file.name%}</a>
            </p>
            {% if (file.error) { %}
                <div><span class="error">Error</span> {%=file.error%}</div>
            {% } %}
        </td>
        <td>
            <span class="size">{%=o.formatFileSize(file.size)%}</span>
        </td>
        <td>
            <button class="delete" data-type="{%=file.deleteType%}" data-url="{%=file.deleteUrl%}"{% if (file.deleteWithCredentials) { %} data-xhr-fields='{"withCredentials":true}'{% } %}>Delete</button>
            <input type="checkbox" name="delete" value="1" class="toggle">
        </td>
    </tr>
{% } %}
</script>

<script src="//ajax.googleapis.com/ajax/libs/jqueryui/1.10.3/jquery-ui.min.js"></script>
<!-- The Templates plugin is included to render the upload/download listings -->
<script src="http://blueimp.github.io/JavaScript-Templates/js/tmpl.min.js"></script>
<!-- The Load Image plugin is included for the preview images and image resizing functionality -->
<script src="http://blueimp.github.io/JavaScript-Load-Image/js/load-image.min.js"></script>
<!-- The Canvas to Blob plugin is included for image resizing functionality -->
<script src="http://blueimp.github.io/JavaScript-Canvas-to-Blob/js/canvas-to-blob.min.js"></script>
<!-- blueimp Gallery script -->
<script src="http://blueimp.github.io/Gallery/js/jquery.blueimp-gallery.min.js"></script>
<!-- The Iframe Transport is required for browsers without support for XHR file uploads -->
<script src="scripts/JqueryUpload/jquery.iframe-transport.js"></script>
<!-- The basic File Upload plugin -->
<script src="scripts/JqueryUpload/jquery.fileupload.js"></script>
<!-- The File Upload processing plugin -->
<script src="scripts/JqueryUpload/jquery.fileupload-process.js"></script>
<!-- The File Upload image preview & resize plugin -->
<script src="scripts/JqueryUpload/jquery.fileupload-image.js"></script>
<!-- The File Upload audio preview plugin -->
<script src="scripts/JqueryUpload/jquery.fileupload-audio.js"></script>
<!-- The File Upload video preview plugin -->
<script src="scripts/JqueryUpload/jquery.fileupload-video.js"></script>
<!-- The File Upload validation plugin -->
<script src="scripts/JqueryUpload/jquery.fileupload-validate.js"></script>
<!-- The File Upload user interface plugin -->
<script src="scripts/JqueryUpload/jquery.fileupload-ui.js"></script>
<!-- The File Upload jQuery UI plugin -->
<script src="scripts/JqueryUpload/jquery.fileupload-jquery-ui.js"></script>
<script src="scripts/JqueryPopup/jquery.magnific-popup.min.js"></script>

<script src="scripts/JqueryUpload/mainFileUpload.js"></script>

