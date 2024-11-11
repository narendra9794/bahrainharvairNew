const Client = {
    BindTable: function () {
        try {
            if ($.fn.DataTable.isDataTable("#client-list-table")) {
                $('#client-list-table').DataTable().destroy();
            }

            var mainClientPage = 0;
            var isClientPage = $("#MainPageLoad").length > 0 ? $("#MainPageLoad").val() : 0;
            var ClientListIntoCollection = $("#ClientListIntoCollection").length > 0 ? $("#ClientListIntoCollection").val() : "0";
            var url = "/Client/GetClients";
            if (ClientListIntoCollection == "1") {
                mainClientPage = 2;
                isClientPage = 0;
                url = "/ClientCollection/GetCollectionClients";
            } else {
                mainClientPage = isClientPage;
            }

            var collectionId = ClientCollections.selectedCollectionId;
            if (collectionId == undefined || collectionId == "undefined" || collectionId == "") {
                collectionId = "00000000-0000-0000-0000-000000000000";
            }
            $("#client-list-table").DataTable({
                "processing": true, // for show progress bar    
                "serverSide": true, // for process server side    
                "filter": isClientPage == "1" || ClientListIntoCollection == "1", // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once  
                "paging": isClientPage == "0" || isClientPage == "1" || ClientListIntoCollection == "1",
                "ajax": {
                    "url": url,
                    "type": "POST",
                    "datatype": "json",
                    "data": function (d) {
                        return $.extend({}, d, {
                            "isMain": mainClientPage,
                            "id": collectionId,
                            "isActive": $("#isActive").val(),
                        });
                    },
                    "dataSrc": function (json) {
                        console.log(json); // Log the entire server response
                        return json.data || []; // Ensure you're returning the correct data array
                    }
                }
,
                "columnDefs": [{
                    "targets": [0],
                    "visible": false,
                    "searchable": false
                }],
                "language": {
                    "infoFiltered": ""
                },
                "columns": [{
                    "data": "id",
                    "name": "id",
                    "autoWidth": true
                },
                {
                    "data": null,
                    "targets": 'no-sort',
                    "orderable": false,
                    render: function (data, type, row) {
                        return '<div class="text-center"><img class="rounded img-fluid avatar-40" src="' + row.image + '" alt="profile"></div>';
                    }
                }, {
                    "data": "clientId",
                    "name": "clientId",
                    "autoWidth": true
                }, {
                    "data": "clientName",
                    "name": "clientName",
                    "autoWidth": true
                },
                {
                    "data": "phone",
                    "name": "phone",
                    "autoWidth": true
                },
                {
                    "data": "lastVisit",
                    "name": "lastVisit",
                    "autoWidth": true
                },
                {
                    "data": "properties",
                    "name": "properties",
                    "autoWidth": true
                },
                {
                    "data": "status",
                    "name": "status",
                    "autoWidth": true
                },
                {
                    "visible": isClientPage == "1" || ClientListIntoCollection == "1",
                    "data": null,
                    "targets": 'no-sort',
                    "orderable": false,
                    render: function (data, type, row) {
                        var str = "Deleted";
                        if (!row.isDeleted && ClientListIntoCollection != "1" && isClientPage == "1" && row.isActive) {
                            str = '<div class="flex align-items-center list-user-action">' +
                                '<a class="bg-primary" data-toggle="tooltip" data-placement="top" title="Edit" data-original-title="Edit" href="/Client/ClientDetail?clientId=' + row.id + '" ><i class="ri-pencil-line"></i></a>' +
                                '</div>';
                        }
                        if (ClientListIntoCollection == "1") {
                            str = '<div class="flex align-items-center list-user-action">' +
                                '<a class="bg-primary" data-toggle="tooltip" data-placement="top" title="Add Client" data-original-title="Add Client" href="#" onclick="Client.AddClientIntoCollection(' + "'" + row.id + "'" + ', true)" ><i class="ri-add-box-line"></i></a>' +
                                '</div>';
                        }
                        if (!row.isActive) {
                            str = "Inactive";
                        }
                        return str;
                    }
                }]
            });
        } catch (e) {
            console.log(e);
        }
    },
    AddClientIntoCollection: function (clientId, isAdd) {
        try {
            $.ajax({
                type: "GET",
                cache: false,
                contentType: "application/json; charset=utf-8",
                url: '/ClientCollection/AddClientIntoCollection?collectionId=' + ClientCollections.selectedCollectionId + '&clientId=' + clientId + '&isAdd=' + isAdd,
                success: function (response) {
                    ClientCollections.BindCollectionClientTable(ClientCollections.selectedCollectionId);
                    Client.BindTable();
                    CustomInfo("success", response);
                },
                error: function () {
                    ClientCollections.BindTable();
                    Client.BindTable();
                    CustomInfo("error", "Oops! Something went wrong.");
                }
            });
        } catch (e) {
            console.log(e);
        }
    },
    DeactivateClient: function (ev, clientId) {
        try {
            var isChecked = ev.currentTarget.checked;
            var message = "Are you sure to deactivate this record?";
            var heading = "Deactivate Record";
            var buttonText = "Deactivate";
            var infoMessage = "Successfully Deactivated";
            if (isChecked) {
                message = "Are you sure to activate this record?";
                heading = "Activate Record";
                buttonText = "Activate";
                infoMessage = "Successfully Activated";
            }
            CustomConfirmation(heading, message, buttonText, function (isConfirmed) {
                if (isConfirmed) {
                    $.ajax({
                        url: $("#DeactivateClientsActionURL").val() + "?clientId=" + clientId + "&isActive=" + isChecked,
                        type: 'PATCH',
                        success: function (data) {
                            Client.BindTable();
                            CustomInfo("success", infoMessage);
                        },
                        error: function (jqXhr, textStatus, errorMessage) {
                            CustomInfo("error", "Oops! Something went wrong.");
                        }
                    });
                } else {
                    Client.BindTable();
                }
            });

        } catch (e) {
            console.log(e);
        }
    },
    SendNotificationsForClient: function (ev, clientId) {
        try {
            var isChecked = ev.currentTarget.checked;
            var message = "Are you sure to send notification for this record?";
            var heading = "Send Notifications";
            var buttonText = "Send";
            var infoMessage = "Cancelled";
            if (isChecked) {
                message = "Are you sure to send notification for this record?";
                heading = "Send Notifications";
                buttonText = "Send";
                infoMessage = "Successfully Started";
            }
            CustomConfirmation(heading, message, buttonText, function (isConfirmed) {
                if (isConfirmed) {
                    $.ajax({
                        url: "/Client/SendNotificationsForClient" + "?clientId=" + clientId,
                        type: 'PATCH',
                        success: function (data) {
                            Client.BindTable();
                            CustomInfo("success", infoMessage);
                        },
                        error: function (jqXhr, textStatus, errorMessage) {
                            CustomInfo("error", "Oops! Something went wrong.");
                        }
                    });
                } else {
                    Client.BindTable();
                }
            });

        } catch (e) {
            console.log(e);
        }
    },
};
