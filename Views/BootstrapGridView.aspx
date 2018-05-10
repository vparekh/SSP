<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BootstrapGridView.aspx.cs" Inherits="SSPWebUI.Views.BootstrapGridView" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
  
<%--http://bootsnipp.com/snippets/featured/bootstrap-editable-jquery-grid--%>
<link rel="stylesheet" type="text/css" href="http://www.prepbootstrap.com/Content/shieldui-lite/dist/css/light/all.min.css" />
<script src="../scripts/jquery-1.12.4.min.js"></script>
<script src="../Scripts/jquery-ui-1.12.1.min.js"></script>
<script src="../Scripts/jquery.blockUI.js"></script>
<script src="../Scripts/bootstrap.min.js"></script>  
<script type="text/javascript" src="http://www.prepbootstrap.com/Content/shieldui-lite/dist/js/shieldui-lite-all.min.js"></script>
<script type="text/javascript" src="../Scripts/griddata.js"></script>

<script type="text/javascript">
    $(document).ready(function () {
        $("#grid").shieldGrid({
            dataSource: {
                data: gridData,
                schema: {
                    fields: {
                        id: { path: "ckey", type: String },
                        name: { path: "name", type: String },
                        role: { path: "role", type: String }
                       
                    }
                }
            },
            sorting: {
                multiple: true
            },
            rowHover: false,
            columns: [
                { field: "name", title: "Author Name", width: "120px" },
                { field: "role", title: "Role", width: "80px", editor: myCustomEditor },
                {
                    width: "40px",
                    title: "Delete",
                    buttons: [
                        { cls: "deleteButton", commandName: "delete", caption: "<img src='http://www.prepbootstrap.com/Content/images/template/BootstrapEditableGrid/delete.png' /><span>Delete</span>" }
                    ]
                }
            ],
            editing: {
                enabled: true,
                event: "click",
                type: "cell",
                confirmation: {
                    "delete": {
                        enabled: true,
                        template: function (item) {
                            return "Delete row with ID = " + item.id
                        }
                    }
                }
            },
            events:
            {
                getCustomEditorValue: function (e) {
                    e.value = $("#dropdown").swidget().value();
                    $("#dropdown").swidget().destroy();
                }
            }
        });

        function myCustomEditor(cell, item) {
            $('<div id="dropdown"/>')
                .appendTo(cell)
                .shieldDropDown({
                    dataSource: {
                        data: ["primary", "secondary", "reviewer"]
                    },
                    value: !item["role"] ? null : item["role"].toString()
                }).swidget().focus();
        }
    });
</script>

      <style type="text/css">
    .sui-button-cell
    {
        text-align: center;
    }

    .sui-checkbox
    {
        font-size: 17px !important;
        padding-bottom: 4px !important;
    }

    .deleteButton img
    {
        margin-right: 3px;
        vertical-align: bottom;
    }

    .bigicon
    {
        color: #5CB85C;
        font-size: 20px;
    }
</style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container" style="width:400px;float:left">
            <div class="col-md-12">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h4 class="text-center">Bootstrap Editable jQuery Grid <span class="fa fa-edit pull-right bigicon"></span></h4>
                    </div>
                    <div class="panel-body text-center">
                        <div id="grid"></div>
                    </div>
                </div>
            </div>
        </div>

        <div id="addAuthor" visible="false" style="margin-left:10px;float:left;border:solid;background-color:lightgreen;padding:4px;width:600px">
                        <p style="font-weight:bold;margin-top:20px">Add authors</p>
                        <p>Please select an authors and a role from below and click Add. </p>
                    <table>
                        <tr>
                            <th>Name</th>
                            <th>Role</th>
                        </tr>
                        <tr>
                            <td><asp:DropDownList ID="lstAddAuthors" ClientIDMode="Static" runat="server"></asp:DropDownList></td>
                            <td><asp:DropDownList ID="lstAddRoles" ClientIDMode="Static" runat="server"></asp:DropDownList></td>
                        </tr>
                    </table>

                   <%-- <asp:LinkButton ID="lbAdd" runat="server" OnClientClick = "validateAuthorAndRole()"> </asp:LinkButton>                
                    <asp:LinkButton ID="lbUser"  runat="server" TabIndex="5" data-toggle="tab" href="#user">&nbsp; New </asp:LinkButton><!-- VP -->--%>
                </div>
    </form>
</body>
</html>
