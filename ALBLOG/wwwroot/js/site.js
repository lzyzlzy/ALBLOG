//import { transcode } from "buffer";

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


//~/admin/CreatePost
var E = window.wangEditor;
var editor = new E('#editor');
editor.customConfig.uploadImgServer = '/admin/upload';
editor.create();
$('pre code').each(function (i, block) {
    hljs.highlightBlock(block)
});


$('#btnSubmitPost').click(() => {
    if (!IsEmpty()) {
        var title = $('#txtTitle').val();
        var tags = $('#txtTags').val();
        var context = editor.txt.html();
        var dto = {
            title: title,
            tags: tags,
            context: context
        };
        var actionName = "";
        if ($('#pagetitle').text() == 'Create') {
            actionName = "CreatePost";
        }
        else {
            actionName = "EditPost";
        }
        $.post("/admin/" + actionName, dto, data => {
            if (data.message == "ok") {
                alert("ok");
                $(location).attr('href', '/admin/index');
            }
            else {
                alert(dto.message);
            }
        });
    }
});

$('#btnDraftPost').click(() => {
    if (!IsEmpty()) {
        var title = $('#txtTitle').val();
        var tags = $('#txtTags').val();
        var context = editor.txt.html();
        var dto = {
            title: title,
            tags: tags,
            context: context
        };
        $.post("/admin/CreateDraft", dto, data => {
            if (data.message == "ok") {
                alert("ok");
                $(location).attr('href', '/admin/index');
            }
            else {
                alert(dto.message);
            }
        });
    }
});

function IsEmpty() {
    var title = $('#txtTitle').val();
    var tags = $('#txtTags').val();
    var context = editor.txt.html();
    if (title == "" || tags == "" || context == "") {
        alert("title,tags can't be empty!");
        return true;
    }
    else {
        return false;
    }
}

$('#btnPreviewPost').click(() => {
    $('#previewBox').empty().append(editor.txt.html());
});
//-------END-------


//~/admin/changeintrodution
$('#btnPreviewAboutOrCV').click(() => {
    $('#previewBox').empty().append(editor.txt.html());
});

function SaveIntroduction(type) {
    var dto = {
        type: type,
        context: editor.txt.html()
    };
    $.post("/admin/ChangeIntroduction", dto, data => {
        alert(data.message);
        if (type == 1) {
            $.get("/admin/GetProfile", data => $('#Profile').empty().append(data.data));
        }
        else if (type == 2) {
            $.get("/admin/GetCV", data => $('#divCV').empty().append(data.data));
        }
        else if (type == 3) {
            $.get("/admin/Getabout", data => $('#divAbout').empty().append(data.data));
        }
    });
}

$('#btnSaveProfile').click(() => {
    SaveIntroduction(1);
});
$('#btnSaveCV').click(() => SaveIntroduction(2))
$('#btnSaveAbout').click(() => SaveIntroduction(3))

//------END-------


//~/home/cv
$.get("/admin/getcv", data => $('#divCV').empty().append(data.data));
//-------END-------

//~/home/about
$.get("/admin/getabout", data => $('#divAbout').empty().append(data.data));
//-------END-------

//COMMON FUNCTION
//-------END-------


