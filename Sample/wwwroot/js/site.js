// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// document.addEventListener('DOMContentLoaded', function () {
//     //create instances for sidebar element
//     var defaultSidebar = document.getElementById("sidebar").ej2_instances[0];
//     // Toggle Button to close and open the sidebar
//     document.getElementById('toggle').onclick = function () {
//         defaultSidebar.toggle();
//     }
//     // Close the sidebar
//     document.getElementById('close').onclick = function () {
//         defaultSidebar.hide();
//     }
// });



document.addEventListener('DOMContentLoaded', function () {
    var defaultSidebar = document.getElementById("default-sidebar").ej2_instances[0];
    document.getElementById('toggle').onclick = function () {
        defaultSidebar.toggle();
    }
    // Close the sidebar
    document.getElementById('close').onclick = function () {
        defaultSidebar.hide();
    }
});

function Open() {
    document.getElementById("main-container").style.marginLeft = "280px";
}
function Close() {
    document.getElementById("main-container").style.marginLeft = "0px";
}

// upload image
function APIUploadImage(postData) {
    var fileimage = postData['FileImage'];
    var formData = new FormData();
    var totalFiles = document.getElementById(fileimage).files.length;
    for (var i = 0; i < totalFiles; i++) {
        var file = document.getElementById(fileimage).files[i];
        formData.append("myfile", file); // named="myfile"
    }

    var filename = $('#' + fileimage).val().replace(/C:\\fakepath\\/i, '')
    var fileExtension = filename.substring(filename.lastIndexOf('.') + 1);
    if (fileExtension == 'jpg' || fileExtension == 'png' || fileExtension == 'jpeg') {
        $.ajax({
            type: "POST",
            url: '/Home/APIUploadImage',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false
        }).done(function (response) {
            if (response.status == "success") {
                $('#' + postData['FileImageHidden']).val(response.filename);
                $('#' + postData['FileImageError']).text("");
            }
            else
                alert(response.status);
        });
    } else {
        $('#' + postData['FileImageError']).text("Only formats are allowed: png, jpg, jpeg");
    }
}


// toggle panel
function OnToggleMenu(postData) {
    $('#collapse_' + postData['field_id']).toggle();

    // localStorage.setItem("a","show-item");
    // var lo = localStorage.getItem("a");
    // document.getElementById("#collapse_" + postData['field_id']).innerHTML = localStorage.getItem("a");

    // if ($('#collapse_' + postData['field_id']).toggle().hasClass("show-item")) {
    //     $('#collapse_' + postData['field_id']).removeClass(lo);
    // } else {
    //     $('#collapse_' + postData['field_id']).addClass(lo);
    // }
}

// alert message
$(window).on("load", function () {
    $('#btn_alert_success').click();
    $('#btn_alert_error').click();
});


// register
function validateEmail(email) {
    var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    return emailReg.test(email);
}
function apiRegister(postData) {
    var formData = new FormData();
    formData.append("first_name", postData["first_name"]);
    formData.append("last_name", postData["last_name"]);
    formData.append("phone", postData["phone"]);
    formData.append("email", postData["email"]);
    formData.append("logo", postData["logo"]);
    formData.append("gender", postData["gender"]);
    formData.append("username", postData["username"]);
    formData.append("password", postData["password"]);
    formData.append("confirm_password", postData["confirm_password"]);
    formData.append("agree", postData["agree"]);

    var validation = 0;
    if ($('#frist_name').val() == '') {
        validation = 1;
        $('#frist_name-error').text("The first name field is required.");
        $("#frist_name").on("keyup change", function (e) {
            if ($("#frist_name").val() == '')
                $('#frist_name-error').text("The first name field is required.");
            else
                $('#frist_name-error').text("");
        });
    }
    if ($('#last_name').val() == '') {
        validation = 1;
        $('#last_name-error').text("The last name field is required.");
        $("#last_name").on("keyup change", function (e) {
            if ($("#last_name").val() == '')
                $('#last_name-error').text("The last name field is required.");
            else
                $('#last_name-error').text("");
        });
    }
    if ($("#email").val() == '' || validateEmail($("#email").val()) == false) {
        validation = 1;
        $('#email-error').text("The email field is required.");
        $("#email").on("keyup change", function (e) {
            if ($("#email").val() == '' || validateEmail($("#email").val()) == false)
                $('#email-error').text("The email field is required.");
            else
                $('#email-error').text("");
        });
    }
    if (postData['username'] == '') {
        validation = 1;
        $('#username-error').text("The username field is required.");
        $("#username").on("keyup change", function (e) {
            if ($("#username").val() == '')
                $('#username-error').text("The username field is required.");
            else
                $('#username-error').text("");
        });
    }
    if (postData['password'] == '') {
        validation = 1;
        $('#password-error').text("The password field is required.");
        $("#password").on("keyup change", function (e) {
            if ($("#password").val() == '')
                $('#password-error').text("The password field is required.");
            else
                $('#password-error').text("");
        });
    }
    if (postData['confirm_password'] == '') {
        validation = 1;
        $('#confirm_password-error').text("The confirm password field is required.");
        $("#confirm_password").on("keyup change", function (e) {
            if ($("#confirm_password").val() == '')
                $('#confirm_password-error').text("The confirm password field is required.");
            else
                $('#confirm_password-error').text("");
        });
    }

    if (validation == 0) {
        $.ajax({
            type: "post",
            url: '/author/apiRegister',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false
        }).done(function (response) {
            // console.log(response);
            if (response.status == "success") {
                window.location.href = "Login";
            } else {
                if (response.userExistingError == 1 && response.passwordError == 1)
                    toastr.error('The user <strong>' + response.userExisting + '</strong> is already exists. And the password and confirm password does not match.', 'Error!');
                else {
                    if (response.userExistingError == 1)
                        toastr.error('The user <strong>' + response.userExisting + '</strong> is already exists.', 'Error!');
                    if (response.passwordError == 1)
                        toastr.error('The password and confirm password does not match.', 'Error!');
                }
            }
        });
    }
}
function api_get_checkbox_value(value, set_id , table_name) {
    var temp = '0';
	eles = document.getElementsByName(value);
	for (var i=0;i<eles.length;i++) {
		if (eles[i].name == value) {
			if(eles[i].checked == true) {
				temp = temp + "-" + eles[i].value;
			}
		} 
	}
    document.getElementById(set_id).value = temp;
    // document.getElementById("table_name").value = table_name;
}
function api_checkbox_all(value, check_value) {
    var temp = document.getElementsByName(check_value);
	for (var i = 0; i < temp.length; i++){
		if (temp[i].name && temp[i].name == check_value) {
            temp[i].checked = value.checked;
        }
	}
}
function bulk_actions_delete(){
    var value = $("#check_value").val();
    var formData = new FormData();
    formData.append("check_value", value);
    $.ajax({
        type: "POST",
        url: '/Home/APIActionDelete',
        data: formData,
        dataType: 'json',
        contentType: false,
        processData: false
    }).done(function (response) {
        // console.log(response);
        if (response.status == 1)
            location.reload();
        else
            toastr.error('No item selected. Please select at least one item.', 'Error!');
    });
}

function API_Bulk_Actions(key_action){
    var value = $("#check_value").val();
    var formData = new FormData();
    formData.append("check_value", value);
    formData.append("key_action", key_action);
    
    if (value != '') {
        $.ajax({
            type: "POST",
            url: '/Home/API_Bulk_Actions',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false
        }).done(function (response) {
            // console.log(response);
           
            if (response.status == "export_to_excel") {
                if (key_action == "export_to_excel") {
                    var bytes = Base64ToBytes(response.stream_file);
                    var blob = new Blob([bytes], { type: "application/octetstream" });

                    var isIE = false || !!document.documentMode;
                    if (isIE) {
                        window.navigator.msSaveBlob(blob, response.fileName);
                    } else {
                        var url = window.URL || window.webkitURL;
                        link = url.createObjectURL(blob);
                        var a = $("<a />");
                        a.attr("download", response.fileName);
                        a.attr("href", link);
                        $("body").append(a);
                        a[0].click();
                        $("body").remove(a);
                    }
                }
            }
            else if (response.status == 1)
                location.reload();
            else if (response.status == 0)
                toastr.error('No item selected. Please select at least one item.', 'Error!');
        });
    } else
        toastr.error('No item selected. Please select at least one item.', 'Error!');
}
function Base64ToBytes(base64) {
    var binary_string = window.atob(base64);
    var len = binary_string.length;
    var bytes = new Uint8Array(len);
    for (var i = 0; i <len; i++) {
        bytes[i] = binary_string.charCodeAt(i);
    }
    return bytes.buffer;
}


// active sidebar menu
$(document).ready(function(){
    var url = window.location;
    $('ul.sidebar-menu li a').filter(function() {
         return this.href == url;
    }).parent().addClass('active');

    // for treeview
    // $('ul.dropdown-menu-item li a').filter(function() {
    //      return this.href == url;
    // }).parentsUntil(".sidebar-menu > .dropdown-menu-item").addClass('active');
});

