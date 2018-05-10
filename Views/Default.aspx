<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%--<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>--%>

<%@ Register assembly="DevExpress.Web.ASPxTreeList.v16.2" namespace="DevExpress.Web.ASPxTreeList" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v16.2" namespace="DevExpress.Web" tagprefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        
    <dx:ASPxTreeList ID="treeList" runat="server" AutoGenerateColumns="False" 
        DataSourceID="AccessDataSource1" KeyFieldName="ID" ParentFieldName="PARENTID">
        <Columns>
            <dx:TreeListTextColumn FieldName="IMAGEINDEX" VisibleIndex="0">
            </dx:TreeListTextColumn>
            <dx:TreeListTextColumn FieldName="DEPARTMENT" VisibleIndex="1">
            </dx:TreeListTextColumn>
            <dx:TreeListTextColumn FieldName="BUDGET" VisibleIndex="2">
            </dx:TreeListTextColumn>
            <dx:TreeListTextColumn FieldName="LOCATION" VisibleIndex="3">
            </dx:TreeListTextColumn>
            <dx:TreeListTextColumn FieldName="PHONE1" VisibleIndex="4">
            </dx:TreeListTextColumn>
            <dx:TreeListTextColumn FieldName="PHONE2" VisibleIndex="5">
            </dx:TreeListTextColumn>
        </Columns>
        <Templates>
            <DataCell>
                <%# GetCellText(Container) %>
            </DataCell>
        </Templates>
        <SettingsBehavior AutoExpandAllNodes="true" />
    </dx:ASPxTreeList>
    <asp:AccessDataSource ID="AccessDataSource1" runat="server" 
        DataFile="C:\eccrepo\SingleSourceProduct\trunk\treelistsample\App_Data/Departments.mdb" 
        SelectCommand="SELECT * FROM [Departments]"></asp:AccessDataSource>
        
    </form>
</body>
</html>
