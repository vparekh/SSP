<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DevExpressTreeList.aspx.cs" Inherits="SSPWebUI.Views.DevExpressTreeList" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v16.2, Version=16.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.v16.2, Version=16.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table class="OptionsTable OptionsBottomMargin">
        <tr>
            <td>
                <dx:ASPxComboBox ID="cmbMode" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbMode_SelectedIndexChanged"
                    ValueType="System.Int32" Caption="Mode"/>
            </td>
            <td style="width: 100%">
            </td>
            <td>
                <dx:ASPxCheckBox ID="chkDragging" runat="server" AutoPostBack="true" Checked="true"
                    Text="Allow node dragging" Wrap="false" />
            </td>
        </tr>
    </table>
        <dx:ASPxTreeList ID="treeList" runat="server" AutoGenerateColumns="False" 
        DataSourceID="ObjectDataSource1" KeyFieldName="ChecklistTemplateItemCkey" ParentFieldName="ParentItemCKey" EnableViewState="false" >
            <Settings GridLines="Both" />
            <SettingsBehavior ExpandCollapseAction="NodeDblClick" />
            <SettingsEditing Mode="EditFormAndDisplayNode" />
            <SettingsPopupEditForm Width="500" />
            
        <Columns>

            <dx:TreeListTextColumn Width="100" FieldName="VisibleText" CellStyle-Wrap="True">
                <EditFormSettings VisibleIndex="1"  ColumnSpan="1" />
            </dx:TreeListTextColumn>
         
            <dx:TreeListTextColumn FieldName="ItemType">
                <EditFormSettings VisibleIndex="3" ColumnSpan="2" />
            </dx:TreeListTextColumn>
            <dx:TreeListCheckColumn FieldName="Released" Caption="Released">
                <EditFormSettings ColumnSpan="2" />
                <EditFormSettings VisibleIndex="4" />
            </dx:TreeListCheckColumn>
            <dx:TreeListCommandColumn ShowNewButtonInHeader="true">
                <EditButton Visible="true" />
                <NewButton Visible="true" />
            </dx:TreeListCommandColumn>

            
        </Columns>
        <%--<Templates>
            <DataCell>
                <%# GetCellText(Container) %>
            </DataCell>
        </Templates>--%>
        <SettingsBehavior AutoExpandAllNodes="true" />
    </dx:ASPxTreeList>
    <%--<asp:AccessDataSource ID="AccessDataSource1" runat="server" 
        DataFile="C:\eccrepo\SingleSourceProduct\trunk\treelistsample\App_Data/Departments.mdb" 
        SelectCommand="SELECT * FROM [Departments]"></asp:AccessDataSource>--%>
    
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" DeleteMethod="Delete" InsertMethod="Insert" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="SSPWebUI.DataSet1TableAdapters.ChecklistTemplateItemsTableAdapter" UpdateMethod="UpdateQuery">
            <UpdateParameters>
                <asp:Parameter Name="ChecklistTemplateItemCKey" Type="Decimal" />
                
                <asp:Parameter Name="Released" Type="Boolean" />
                <asp:Parameter Name="VisibleText" Type="String" />
                <asp:Parameter Name="ItemTypeKey" Type="Int32" />
                <asp:Parameter Name="ParentItemCKey" Type="Decimal" />
                <asp:Parameter Name="MinRepetitions" Type="Boolean" />
                <asp:Parameter Name="MaxRepetitions" Type="Boolean" />
            </UpdateParameters>
        </asp:ObjectDataSource>
    
    </div>
    </form>
</body>
</html>
