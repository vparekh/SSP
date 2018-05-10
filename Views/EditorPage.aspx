<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditorPage.aspx.cs" Inherits="SSPWebUI.Views.EditorPage" %>
<%@ Register tagprefix="ssp" Tagname="myHeader" src="~/UserControls/Header.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin-top:0px;margin-bottom:20px;clear:both">
                    <div style="float:left">
                        <b> Protocol</b>
                        <%--<asp:DropDownList ID="ddlProtocolGroups" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddlProtocolGroups_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>--%>
                        <%--<asp:DropDownList ID="ddlProtocols" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="Protocols_SelectedIndexChanged" onchange="LoadContents()" AutoPostBack="true" ></asp:DropDownList>--%>
                        <asp:DropDownList ID="ddlProtocols" runat="server" ClientIDMode="Static" onchange="LoadContents()"></asp:DropDownList>
                    </div>
                    
                     <div style="margin-left:20px; float:left">
                        <%--Form some AssociatedUpdatePanelID no longer working 
                        <asp:UpdateProgress ID="UpdateProgress1" DisplayAfter="1" AssociatedUpdatePanelID="parentPanel" runat="server">  --%>    
                        <asp:UpdateProgress ID="UpdateProgress1" DisplayAfter="1" runat="server">                        
                            <ProgressTemplate>
                            <img src="../images/ajax-loader.gif"/> Loading ... 
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    </div>
                    
                </div>
    <div id="divTitle">
    <ssp:myHeader runat="server" id="hdr"></ssp:myHeader>
    </div>
    </form>
</body>
</html>
