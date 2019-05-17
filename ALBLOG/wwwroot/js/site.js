
// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$('#btnUserName').click(() => {
    var name = $('#txtUserName').val();
    var dto = {
        userName: name
    };
    $.post("Home/Test", dto, (data) => {
        if (data == null) {
            $('result').text("null");
        }
        else {
            $('#result').text(data);
        }
    })
});