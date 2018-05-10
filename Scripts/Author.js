function PopulateEditableAuthor() {
    $("#grid").empty();

    selectedprotocol = $('#hdnProtocol').val();
    //http://demos.shieldui.com/web/grid-editing/editing-external-form
    //document: https://www.shieldui.com/documentation/dropdown/javascript/getting.started
    //https://www.shieldui.com/documentation/datasource/javascript/api/settings/remote/read
    //http://demos.shieldui.com/web/grid-editing/cell-editing-autosync

    //$.getJSON("../api/Author/GetAll?ProtocolVersion=" + selectedprotocol, function (gridData) {


    var varroleckey = '';
    var varnameckey = '';
    var varprotocolauthorroleckey = '';
        //Example - http://demos.shieldui.com/web/grid-editing/editing-restful-web-service
    var rolelabel = 'x';
        var dataSrc = new shield.DataSource({
            events: {
                error: function (event) {
                    if (event.errorType == "transport") {
                        // transport error is an ajax error; event holds the xhr object
                        alert("transport error in PopulateEditableAuthor: \r\n" + event.error.statusText + "\r\n" + event.responseText);
                        //window.location.replace("login.aspx");
                        // reload the data source if the operation that failed was save
                        if (event.operation == "save") {
                            this.read();
                        }
                    }
                    else {
                        // other data source error - validation, etc
                        alert('Author.js - ' + event.errorType + " error: " + event.error);
                    }
                }
            },
            
            remote: {
                read: {
                    type: "GET",
                    url: "../api/Author/GetAll?ProtocolVersion=" + $('#hdnProtocol').val() + '&Mode=' + $('#hdnEditMode').val() + '&DraftVersion=' + $('#hdnDraftVersion').val(),
                    dataType: "json"
                },

                modify: {

                    update: function (edited, success, error) {

                        //alert(varroleckey);
                        //var newroleckey = varroleckey; // edited[0].data.roleckey;

                        //edited[0].data.role = rolelabel;
                        //alert(edited[0].data.protocolauthorroleckey + ':' + varnameckey + ':' + varroleckey);
                        $.ajax({
                            type: "PUT",
                            url: "../api/Author/UpdateAuthors?ProtocolVersion=" + $('#hdnProtocol').val()
                                + "&author=" + varnameckey + "&role=" + varroleckey
                                + "&protocolauthorroleckey="
                                + varprotocolauthorroleckey + "&userckey=" + $('#hdnUserCKey').val(),
                            dataType: "json",
                            contentType: "application/json",
                            data: JSON.stringify(edited[0].data)
                        }).then(success, error);
                    },
                    remove: function (items, success, error) {
                        $.ajax({
                            type: "DELETE",
                            url: "../api/Author/DeleteAuthorRole?protocolauthorroleckey=" + items[0].data.protocolauthorroleckey
                        }).then(success, error);
                    }
                }
            },
            
           
            schema: {
                fields: {
                    id: { path: "AuthorCKey", type: String },                    
                    roleckey: { path: "RoleCKey", type: String },
                    protocolauthorroleckey: { path: "ProtocolAuthorRoleCKey", type: String },
                    name: { path: "Name", type: String },
                    role: { path: "Role", type: String }
                    
                }
            }
        });

        function success(data) {
            

        }

        $("#grid").shieldGrid({
            dataSource: dataSrc,


            sorting: {
                multiple: false
            },

            rowHover: false,
            scrolling: true,
           
           
            columns: [
                { field: "id", title: "Author CKey", width: "1px" },
                { field: "roleckey", title: "Role CKey", width: "1px" },
                { field: "protocolauthorroleckey", title: "Protocol Author Role CKey", width: "1px" },
                { field: "name", title: "Author Name", width: "250px", editor: myNameEditor },                
                { field: "role", title: "Role", width: "150px", editor: myRoleEditor },
                {
                    width: 140,
                    title: " ",
                    buttons: [
                        { commandName: "edit", caption: "Edit" },
                        { commandName: "delete", caption: "Delete" }
                    ]
                }
            ],
            
            editing: {
                enabled: true,
                //event: "click",
                type: "row",
                confirmation: {
                    "delete": {
                        enabled: true,
                        template: function (item) {
                            return "Delete " + item.name + " from the " + item.role + ' role?'
                        }
                    }
                }
            },

            events:
            {

                edit: function (e) {
                    var row = e.row;
                    //alert(row.index());
                    var cell = e.cell;
                    varprotocolauthorroleckey = $("#grid").swidget().dataItem(row.index()).protocolauthorroleckey;
                    //alert($("#grid").swidget().dataItem(row.index()).protocolauthorroleckey);
                },
                getCustomEditorValue: function (e) {
                    if (e.field == "name") {
                        e.value = $("#namedropdown").swidget().text();
                        varnameckey = $("#namedropdown").swidget().value();
                        $("#namedropdown").swidget().destroy();
                    }
                    else {

                        //the cell that was edited will receive this value
                        //e.value = $("#roledropdown").swidget().value();
                        e.value = $("#roledropdown").swidget().text();
                        varroleckey = $("#roledropdown").swidget().value();
                        $("#roledropdown").swidget().destroy();
                    }

                }

            }
        });

    //});

    function myRoleEditor(cell, item) {
        //load roles from server
        $.getJSON("../api/Role/GetAll",
        function (roles) {
            $('<div id="roledropdown"/>')
              .appendTo(cell)
              .shieldDropDown({
                  dataSource: {
                      data: roles
                  },
                  textTemplate: "{RoleName}",
                  valueTemplate: "{RoleCKey}",
                  
                  value: item["roleckey"]
                  //value: !item["roleckey"] ? null : item["roleckey"].toString()
                  
              }).swidget().focus();
            
        });


    }

    function myNameEditor(cell, item) {
        //load names from server

        $.getJSON("../api/Author/GetAllAuthors",
        function (names) {

            $('<div id="namedropdown"/>')
              .appendTo(cell)
              .shieldDropDown({
                  dataSource: {
                      data: names
                  },
                  textTemplate: "{Name}",
                  valueTemplate: "{CKey}",

                  value: item["id"]
              }).swidget().focus();

        });
    }
}

//readonly
function PopulateReadonlyAuthor() {
    $("#grid").empty();

    selectedprotocol = $('#hdnProtocol').val();
   
    //http://demos.shieldui.com/web/grid-editing/editing-external-form
    //document: https://www.shieldui.com/documentation/dropdown/javascript/getting.started
    //https://www.shieldui.com/documentation/datasource/javascript/api/settings/remote/read
    //http://demos.shieldui.com/web/grid-editing/cell-editing-autosync
    $.getJSON("../api/Author/GetAll?ProtocolVersion=" + selectedprotocol + '&Mode=' + $('#hdnEditMode').val() + '&DraftVersion=' + $('#hdnDraftVersion').val(), function (gridData) {



        //Example - http://demos.shieldui.com/web/grid-editing/editing-restful-web-service

        var dataSrc = new shield.DataSource({
            events: {
                error: function (event) {
                    if (event.errorType == "transport") {
                        // transport error is an ajax error; event holds the xhr object
                        alert("transport error in PopulateReadonlyAuthor: " + event.error.statusText);
                        //window.location.replace("login.aspx");
                        // reload the data source if the operation that failed was save
                        if (event.operation == "save") {
                            this.read();
                        }
                    }
                    else {
                        // other data source error - validation, etc
                        alert('Author.js - ' + event.errorType + " error: " + event.error);
                    }
                }
            },
            remote: {
                read: {
                    type: "GET",
                    url: "../api/Author/GetAll?ProtocolVersion=" + $('#hdnProtocol').val() + '&Mode=' + $('#hdnEditMode').val() + '&DraftVersion=' + $('#hdnDraftVersion').val(),
                    dataType: "json"
                },

            },
            schema: {
                fields: {
                    id: { path: "AuthorCKey", type: String },
                    name: { path: "Name", type: String },
                    role: { path: "Role", type: String },
                    roleckey: { path: "RoleCKey", type: String },
                    protocolauthorroleckey: { path: "ProtocolAuthorRoleCKey", type: String }
                }
            }
        });

        function success(data) {
            

        }


        $("#grid").shieldGrid({
            dataSource: dataSrc,

            sorting: {
                multiple: false
            },
            
            rowHover: false,
            scrolling: true,
           
           
            columns: [
                { field: "id", title: "Author CKey", width: "1px" },
                { field: "name", title: "Author Name", width: "250px" },
                { field: "role", title: "Role", width: "150px" }

            ],
           

        });

    });


}
//--readonly end    
function AddAuthorRole() {
    var author = $('#lstAddAuthors').val();
    var role = $('#lstAddRoles').val();

    if (!regIsNumber(author) | !regIsNumber(role)) {
        alert("Please select an author and a role from the dropdown.");
        return;
    }

    $.ajax({
        url: '../api/author/AddAuthorRole?ProtocolVersion=' + $('#hdnProtocol').val() + "&author=" + author + "&role=" + role + "&userckey=" + $('#hdnUserCKey').val(),
        type: 'POST',
        dataType: 'json',
        data: "dummmy",
        success: function (data, textStatus, xhr) {
            PopulateEditableAuthor();
            console.log(data);
        },

        error: function (xhr, textStatus, errorThrown) {
            console.log('Error in Operation');
        }

    });
}