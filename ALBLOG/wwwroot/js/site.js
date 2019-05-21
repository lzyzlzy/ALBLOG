
// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//~/home/index
$('#btnLogOut').click(() => {
    $.post("/admin/LogOut", () => {
        $(location).attr('href', '/admin/login');
    })
});
//-------END-------


//~/home/about
//-------END-------


//~/admin/login
$('#btnLogin').click(() => {
    username = $('#txtUserName').val();
    password = $('#txtPassword').val();
    if (username == "" || password == "") {
        alert('Please input username or password!')
    }
    else {
        var dto = {
            userName: username,
            password: password
        }
        $.post("/admin/login", dto, data => {
            if (data.message == "ok") {
                $(location).attr('href', '/admin/index');
            }
            else {
                alert(data.message);
            }
        });
    }
});
//-------END-------


//~/admin/index
//-------END-------


//~/home/index
//-------END-------


//~/home/index
//-------END-------

//COMMON FUNCTION
//-------END-------


