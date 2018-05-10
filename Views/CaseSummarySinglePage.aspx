<%@ Page Title="" Language="C#" MasterPageFile="~/Views/SSPEditorEx.Master" AutoEventWireup="true" CodeBehind="CaseSummarySinglePage.aspx.cs" Inherits="SSPWebUI.Views.CaseSummarySinglePage" %>

<%@ Register Assembly="DevExpress.Web.v16.2, Version=16.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="leftnavigation" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentplaceholder" runat="server">
    <div>
         <dx:ASPxHiddenField ID="ASPxHiddenField1" runat="server" ClientInstanceName="hidden">
        </dx:ASPxHiddenField>
        <dx:ASPxTreeView  ID="treeView" runat="server" EnableCallBacks="true" 
            OnVirtualModeCreateChildren="treeView_VirtualModeCreateChildren">
            
            <Images>
                <NodeImage Width="16px" Height="16px">
                </NodeImage>
            </Images>
            <Styles>
                <NodeImage Paddings-PaddingTop="3px">
                </NodeImage>
            </Styles>
        </dx:ASPxTreeView>

    </div>
</asp:Content>
