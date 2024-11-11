// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

toastr.options = {
    "closeButton": true,
    "debug": false,
    "newestOnTop": false,
    "progressBar": false,
    "positionClass": "toast-top-right",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
}

var loadCommonModal = function (url) {
    $("#SmallModalDiv").load(url, function () {
        $("#SmallModal").modal("show");
        $("#Name").focus();
    });
};

var loadMediumModal = function (url) {
    $("#MediumModalDiv").load(url, function () {
        $("#MediumModal").modal("show");
        $("#Name").focus();
    });
};

var loadBigModal = function (url) {
    $("#BigModalDiv").load(url, function () {
        $("#BigModal").modal("show");
        $('#Name').focus();
    });
};

var loadExtraBigModal = function (url) {
    $("#ExtraBigModalDiv").load(url, function () {
        $("#ExtraBigModal").modal("show");
    });
};

var loadPrintModal = function (url) {
    $("#PrintModalDiv").load(url, function () {
        $("#PrintModal").modal("show");
    });
};

var loadImageViewModal = function () {
    $("#ImageViewModal").modal("show");
};



var AddCustomer = function (id) {
    $('#titleMediumModal').html("Add Customer");
    loadMediumModal("/CustomerInfo/AddEdit?id=" + id);
};

var AddSupplier = function (id) {
    $('#titleMediumModal').html("Add Supplier");
    loadMediumModal("/Supplier/AddEdit?id=" + id);
};

var AddCategories = function (id) {
    $('#titleMediumModal').html("Add Categories");
    loadMediumModal("/Categories/AddEdit?id=" + id);
};

var AddUnitsofMeasure = function (id) {
    $('#titleMediumModal').html("Add Units of Measure");
    loadMediumModal("/UnitsofMeasure/AddEdit?id=" + id);
};


var SearchInHTMLTable = function () {
    var input, filter, table, tr, td, i, txtValue;
    input = document.getElementById("myInput");
    filter = input.value.toUpperCase();
    table = document.getElementById("myTable");
    tr = table.getElementsByTagName("tr");
    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[1];
        if (td) {
            txtValue = td.textContent || td.innerText;
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}

var FieldValidation = function (FieldName) {
    var _FieldName = $(FieldName).val();
    if (_FieldName == "" || _FieldName == null) {
        return false;
    }
    return true;
};

var FieldValidationAlert = function (FieldName, Message, icontype) {
    Swal.fire({
        title: Message,
        icon: icontype,
        onAfterClose: () => {
            $(FieldName).focus();
        }
    });
}

var SwalSimpleAlert = function (Message, icontype) {
    Swal.fire({
        title: Message,
        icon: icontype
    });
}

var FieldIdNullCheck = function (FieldId) {
    if (FieldId === null || FieldId === '')
        FieldId = 0;
    return FieldId;
}

var ViewImage = function (imageURL, Title) {
    $('#titleImageViewModal').html(Title);
    $("#UserImage").attr("src", imageURL);
    $("#ImageViewModal").modal("show");
};

var ViewImageByURLOnly = function (imageURL) {
    $('#titleImageViewModal').html('Asset Image');
    $("#UserImage").attr("src", imageURL);
    $("#ImageViewModal").modal("show");
};

var activaTab = function (tab) {
    $('.nav-tabs a[href="#' + tab + '"]').tab('show');
};

var BacktoPreviousPage = function () {
    window.history.back();
}

var printDiv = function (divName, RemoveCssClass) {
    $("table").removeClass(RemoveCssClass);
    var printContents = document.getElementById("printableArea").innerHTML;
    var originalContents = document.body.innerHTML;
    document.body.innerHTML = printContents;
    window.print();
    document.body.innerHTML = originalContents;
}

var printBarcodeDiv = function (printableBarcodeAreaId) {
    var printContents = document.getElementById(printableBarcodeAreaId).innerHTML;
    var originalContents = document.body.innerHTML;
    document.body.innerHTML = printContents;
    window.print();
    document.body.innerHTML = originalContents;
}

//function toasterSuccessMessage(message) {
//  toastr.clear();
//  toastr.success(message, "Success Message", { closeButton: true, timeOut: "1000", maxOpened: 1 });
//}
//function toasterErrorMessage(message) {
//  toastr.clear();
//  toastr.error(message, "Error Message", { closeButton: true, timeOut: "1000", maxOpened: 1 });
//}
//function toasterWarningMessage(message) {
//  toastr.clear();
//  toastr.warning(message, "Warning Message", { closeButton: true, timeOut: "1000", maxOpened: 1 });
//}
//function toasterInfoMessage(message) {
//  toastr.clear();
//  toastr.info(message, "Info Message", { closeButton: true, timeOut: "1000", maxOpened: 1 });
//}

function CustomConfirmation(confirmationHeading, confirmationMessage, buttonText, callback) {
    $("#DeleteConfirmation").modal('show');
    $("#ConfirmationMessage").text(confirmationMessage);
    $("#ConfirmationHeading").text(confirmationHeading);
    $("#DoDelete").text(buttonText);
    if (buttonText == "Activate") {
        $("#headerIcon").removeClass();
        $("#headerIcon").addClass("ri-flashlight-line");
    } else if (buttonText == "Deactivate") {
        $("#headerIcon").removeClass();
        $("#headerIcon").addClass("ri-shut-down-line");
    } else if (buttonText == "Delete") {
        $("#headerIcon").removeClass();
        $("#headerIcon").addClass("ri-delete-bin-line");
    }
    $("#DeleteConfirmation button").unbind();
    $("#DeleteConfirmation button").on("click", function (e) {
        if (e.currentTarget.id == "DoDelete") {
            callback(true);
        } else {
            callback(false);
        }
        $("#DeleteConfirmation").modal('hide');     // dismiss the dialog
    });
}
function SaveConfirmation(saveHeading, collectionName, callback) {
    $("#SaveConfirmation").modal('show');
    $("#SaveHeading").text(saveHeading);
    $("#SaveInputBox").val(collectionName);
    $("#SaveConfirmation button").unbind();
    $("#SaveConfirmation button").on("click", function (e) {
        if (e.currentTarget.id == "DoSave") {
            callback(true);
        } else {
            callback(false);
        }
        /*$("#SaveConfirmation").modal('hide');*/     // dismiss the dialog
    });
}
function CustomExportConfirmation(buttonText, callback) {
    $("#ExportConfirmation").modal('show');
    $("#DoExport").text(buttonText);
    $("#ExportConfirmation button").unbind();
    $("#ExportConfirmation button").on("click", function (e) {
        if (e.currentTarget.id == "DoExport") {
            callback(true);
        } else {
            callback(false);
        }
        $("#ExportConfirmation").modal('hide');     // dismiss the dialog
    });
}
function CustomInfo(InfoType, message) {
    if (InfoType == "error") {
        $("#CustomError").modal('show');
        $("#ErrorMessage").text(message);
        setTimeout(function () {
            $("#CustomError").modal('hide');
        }, 1500);
    }
    else if (InfoType == "success") {
        $("#CustomSuccess").modal('show');
        $("#SuccessMessage").text(message);
        setTimeout(function () {
            $("#CustomSuccess").modal('hide');
        }, 1500);
    }
    else if (InfoType == "warning") {
        $("#CustomWarning").modal('show');
        $("#WarningMessage").text(message);
        setTimeout(function () {
            $("#CustomWarning").modal('hide');
        }, 1500);
    }
    else if (InfoType == "info") {
        $("#CustomInfo").modal('show');
        $("#InfoMessage").text(message);
        setTimeout(function () {
            $("#CustomInfo").modal('hide');
        }, 1500);
    }
    else if (InfoType == "audio-player") {
        $("#AudioPlayer").modal('show');
        var audio = $("#AudioPlayerElement");
        $("#mp3_src").attr("src", message);

        /****************/
        audio[0].pause();
        audio[0].load();//suspends and restores all audio element

        //audio[0].play(); changed based on Sprachprofi's comment below
        audio[0].oncanplaythrough = audio[0].play();
        /****************/

    }

}

function secondsToHms(d) {
    d = Number(d);
    var h = Math.floor(d / 3600);
    var m = Math.floor(d % 3600 / 60);
    var s = Math.floor(d % 3600 % 60);

    return h + ":" + m + ":" + s;
    //var hDisplay = h > 0 ? h + (h == 1 ? " hour, " : " hours, ") : "";
    //var mDisplay = m > 0 ? m + (m == 1 ? " minute, " : " minutes, ") : "";
    //var sDisplay = s > 0 ? s + (s == 1 ? " second" : " seconds") : "";
    //return hDisplay + mDisplay + sDisplay;
}

function ShowLoader(isShow) {
    if (isShow) {
        $("#CustomLoader").modal('show');
    } else {
        $("#CustomLoader").modal('hide');
    }
}
function CustomDateFormat(dateToFormat) {
    try {
        var localDate = (new Date(dateToFormat));
        var formatedDate = jQuery.format.date(dateToFormat, dateFormateSettting.smallDateFormat);
        return formatedDate;
    } catch (e) {
        console.log(e);
    }

}

function CustomUTCDateFormat(dateToFormat) {
    try {
        var localDate = new Date(dateToFormat * 1000); //(new Date(dateToFormat * 1000).getDate() + "/" + (new Date(dateToFormat * 1000).getMonth() + 1) + "/" + new Date(dateToFormat * 1000).getFullYear());
        var formatedDate = jQuery.format.date(localDate, dateFormateSettting.smallDateFormat);
        return formatedDate;
    } catch (e) {
        console.log(e);
    }

}

