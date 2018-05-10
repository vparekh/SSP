<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompareNotes.aspx.cs" Inherits="SSPWebUI.Views.Compare" %>
<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v16.2, Version=16.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">

        ins {
    background-color: #cfc;
    text-decoration: none;
}

del {
    color: #999;
    background-color: #FEC8C8;
}

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div>            
                    <table>
                        <tr>
                            <th  style="text-align:left">
                                <span>Version </span><asp:DropDownList ID="ddVersionLeft" AutoPostBack="true" OnSelectedIndexChanged="ddVersionLeft_SelectedIndexChanged" runat="server"></asp:DropDownList>
                            </th>
                            <th style="text-align:left">
                                <span>Version </span><asp:DropDownList ID="ddVersionRight" AutoPostBack="true" OnSelectedIndexChanged="ddVersionRight_SelectedIndexChanged" runat="server"></asp:DropDownList>
                            </th>
                        </tr>
                        <tr>
                            
                            <td  style=" text-align:left; padding:10px" colspan="2">
                                <p>Section <asp:DropDownList ID="ddSection" AutoPostBack="true" OnSelectedIndexChanged="ddSection_SelectedIndexChanged" runat="server"></asp:DropDownList></p>
                                <asp:Button ID="btnCompare" runat="server" ToolTip="Compare" Text="Compare" OnClick="btnCompare_Click" /></td>
                        </tr>
                        <tr>
                            <td style="vertical-align:top">
                                <div runat="server" id="left" style="float:left; min-height:90vh; width:48vw; border:1px solid black; padding:8px; ">                
                                    
                                </div>
                                </td>
                            <td style="vertical-align:top">
                                <div runat="server" id="right" style="float:left; min-height:90vh; width:48vw; border:1px solid black; padding:8px;">                
                                   
                                </div>
                            </td>
                        </tr>
                    </table>            
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        
    </form>
</body>
</html>
