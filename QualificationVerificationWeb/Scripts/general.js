var errorCounter;
var clickCounter = 1;

function fnOpenConfirmDialog(buttonId) {
    var isValid = false;
    $("#dialog-confirm").html("Are you sure?");
    $("#dialog-confirm").dialog({
        resizable: false,
        modal: true,
        title: "Confirmation",
        height: 150,
        width: 250,
        resizeble: false,
        buttons: {
            "Yes": function () {
                isValid = true;
                $(this).dialog('close');
                __doPostBack(buttonId, '');
            },
            "Cancel": function () {
                isValid = false;
                $(this).dialog('close');
            }
        }
    });
    return isValid;
}

function fnOpenGeneralDialog(message, current, version) {
    var isValid = false;
    $("#dialog-confirm").html(message);
    $("#dialog-confirm").dialog({
        resizable: false,
        modal: true,
        title: "Whats new?",
        height: 250,
        width: 550,
        resizeble: false,
        buttons: {
            "Ok": function () {
                isValid = true;
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "General.asmx/UserAcknowlegdedUpdates",
                    data: "{'current':'" + current + "','version':'" + version + "'}",
                    dataType: "json",
                    success: function (data) {
                        if (data.d === true) {
                            fnSuccessful();
                        }
                        else {
                            fnSuccessful();
                        };
                        ;
                    }
                });
            }
        }

    });
    return isValid;
}

function fnOpenErrorDialog() {
    $("#dialog-confirm").html("An error has occurred. Our technical team has now been notified of this error and will investigate and resolve it as soon as possible. <br> <br>Please <a href='javascript:history.go(-1)'>click here</a> to return to previous page");
    $("#dialog-confirm").dialog({
        resizable: false,
        modal: true,
        title: "Our apologies",
        height: 210,
        width: 450,
        resizeble: false
    });

}

function fnTimeoutDialog() {
    var isValid = false;
    var currentUser = $('#currentUser').val();
    $("#dialog-confirm").html("Your session is about to expire, to continue please enter your password and click ok. <br>  <br>  <label>Password:</label><input id='password' name='password' runst='server' type='password'/></label>");
    $("#dialog-confirm").dialog({
        resizable: false,
        modal: true,
        title: "Warning",
        height: 250,
        width: 450,
        buttons: {
            "Ok": function () {
                fnClickCounter();
                isValid = true;
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "/VerifyPassword.asmx/VerifyUserPassword",
                    data: "{'username':'" + currentUser + "','password':'" + $('#password').val() + "'}",
                    dataType: "json",
                    success: function (data) {
                        if (data.d === true)
                        { fnSuccessful(); }
                        else {
                            fnNotSuccessful(errorCounter);
                        };
                        ;
                    }
                });
            },
            "Log out": function () {
                isValid = false;
                window.location = "/Login.aspx?LoggedOut=True";
                $(this).dialog('close');
            }
        }
    });
    return isValid;
}

function fnSuccessful() {
    $("#dialog-confirm").dialog('close');
    //  setTimeout(function () { fnTimeoutDialog(); }, 120000);
}

function fnNotSuccessful(passwordCount) {

    var count = 4 - parseInt(passwordCount);
    var tries = 3;
    if (tries > passwordCount) {
        $("#IncorrectPassword").text("Invalid login credentials suppied. " + count + " more tries");
        $("#IncorrectPassword").css('display', 'block');
    }
    else if (count === 1) {
        $("#IncorrectPassword").text("Last try else log out");

    } else {
        $("#dialog-confirm").dialog('close');
        window.location = "/Login.aspx?LoggedOut=True";
    }

}

function fnClickCounter() {
    clickCounter = clickCounter + 1;
    errorCounter = clickCounter;
    return clickCounter;
}
