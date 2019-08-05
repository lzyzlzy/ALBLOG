﻿//import { transcode } from "buffer";

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


function SaveIntroduction(type) {
    var dto = {
        type: type,
        context: editor.txt.html()
    };
    $.post("/admin/ChangeIntroduction", dto, data => {
        alert(data.message);
        if (type == 1) {
            GetProfile();
        }
        else if (type == 2) {
            GetCV();
        }
        else if (type == 3) {
            GetAbout();
        }
    });
}

$('#btnSaveProfile').click(() => {
    SaveIntroduction(1);
});
$('#btnSaveCV').click(() => SaveIntroduction(2));
$('#btnSaveAbout').click(() => SaveIntroduction(3));
$('#btnRecallProfile').click(() => GetProfile());
$('#btnRecallCV').click(GetCV);
$('#btnRecallAbout').click(GetAbout);
$('#btnPreviewCV').click(() => $('#divCV').empty().append(editor.txt.html()));
$('#btnPreviewAbout').click(() => $('#divAbout').empty().append(editor.txt.html()));
$('#editor').keyup(ChangeProfile).mouseup(ChangeProfile);
function ChangeProfile() {
    $('#Profile').empty().append(editor.txt.html());
}
$('#btnSaveProfilePhoto').click(() => {
    var _data = new Array();
    _data.push($('#frmUpLoadProfileImg').serialize());
    var dto = {
        errno: 0,
        data: _data
    };
    $('#frmUpLoadProfileImg').submit();
    $.post("/admin/UpLoad", dto, data => {
        if (data.errno == 0) {
            alert("success!");
            $('#ImgPreProfile').attr("src", data.data[0]);
        }
        else {
            alert("fail");
        }
    });
});

//------END-------


//~/home/cv
GetCV();
//-------END-------

//~/home/about
GetAbout();
//-------END-------

//COMMON FUNCTION
function GetProfile() {
    $.get("/admin/GetProfile", data => $('#Profile').empty().append(data.data));
}

function GetCV() {
    $.get("/admin/getcv", data => $('#divCV').empty().append(data.data));
}

function GetAbout() {
    $.get("/admin/getabout", data => $('#divAbout').empty().append(data.data));
}
function GetProfilePhoto() {
    $.get("/admin/GetProfilePhotoPath", dto => $('#ProfilePhoto').attr("src", dto.data));
}
//-------END-------


