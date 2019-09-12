//import { transcode } from "buffer";

// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//~/home/index
$('#btnLogOut').click(() => {
    $.post("/admin/LogOut", () => {
        $(location).attr('href', '/home/login');
    })
});
//-------END-------


//~/home/about
//-------END-------


//~/home/login
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
        $.post("/home/login", dto, data => {
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
        var id = $('#postId').text();
        var dto = {
            id: id,
            title: title,
            tags: tags,
            context: context
        };
        var actionName = $('#pagetitle').text() == 'Create' ? "CreatePost" : "EditPost";
        $.post("/admin/" + actionName, dto, data => {
            if (data.message == "ok") {
                alert("ok");
                $(location).attr('href', '/admin/index');
            }
            else {
                alert(data.message);
            }
        });
    }
});

$('#btnDraftPost').click(() => {
    if (!IsEmpty()) {
        var title = $('#txtTitle').val();
        var tags = $('#txtTags').val();
        var context = editor.txt.html();
        var post = {
            title: title,
            tags: tags,
            context: context
        };
        var dto = {
            postDto: post,
            isDraft: true
        }
        $.post("/admin/CreatePost", dto, data => {
            if (data.message == "ok") {
                alert("ok");
                $(location).attr('href', '/admin/index');
            }
            else {
                alert(data.message);
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


//~/admin/settings

$('#btnUpLoadPhoto').click(() => {
    $('#inputProfilePhoto').trigger('click')
        .on('change', () => upLoadImg());
});


function upLoadImg() {
    $.ajax({
        url: "/admin/UpLoad",
        type: "post",
        dataType: "json",
        cache: false,
        data: new FormData($("#frmUpLoadProfileImg")[0]),
        processData: false,// 不处理数据
        contentType: false, // 不设置内容类型
        success: function (data) {
            $('#ImgPreProfile').prop('src', data.data[0]);
            $('#btnSavePhoto').removeClass('hidden');
            $('#btnPreviewPhoto').removeClass('hidden');
        }
    })
}


function showModal(url) {
    $('#imgModal').removeClass('hidden').prop('src', url);
    $('#modalImg').modal('show');
}

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

$('#btnSavePhoto').click(() => {
    $.post('/admin/UpLoadProfileImg', { url: $('#ImgPreProfile').attr('src') }, (data) => {
        if (data.state == "success") {
            GetProfilePhoto();
        }
        alert(data.message);
    })
});
$('#btnPreviewPhoto').click(() => $('#ProfilePhoto').attr("src", $('#ImgPreProfile').prop('src')));
$('#btnSaveProfile').click(() => SaveIntroduction(1));
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
    $('#frmUpLoadProfileImg').submit();
});

//------END-------


//~/home/cv
//-------END-------

//~/home/about
//-------END-------

//COMMON FUNCTION
function SetContent() {
    if ($('#Myself').css('visibility') == "hidden") {
        document.getElementById("main").removeAttribute("class");
    }
}

function SetEmptySection() {
    if ($('#Myself').css('visibility') != "hidden") {
        $('#sectionEmpty').height(window.outerHeight - $('#profile').height() - $('#_footer').height());
    }
}

function GetProfile() {
    $.get("/home/GetProfile", data => $('#Profile').empty().append(data.data));
}

function GetCV() {
    $.get("/home/getcv", data => $('#divCV').empty().append(data.data));
}

function GetAbout() {
    $.get("/home/getabout", data => $('#divAbout').empty().append(data.data));
}

function GetProfilePhoto() {
    $.get("/home/GetProfilePhotoPath", dto => $('#ProfilePhoto').attr("src", dto.data));
}

function AddActive(id) {
    document.getElementById(id).classList.add("active");
}

GetProfile();
GetProfilePhoto();
GetAbout();
GetCV();
SetEmptySection();
SetContent();
//-------END-------


