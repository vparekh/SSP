<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ASPXPageControlDemo.aspx.cs" Inherits="SSPWebUI.Views.ASPXPageControlDemo" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <dx:ASPxPageControl ID="ASPxPageControl1" runat="server">
             <TabPages>
                <dx:TabPage Text="Tab 1">
                    <ContentCollection>
                        <dx:ContentControl runat="server">
                            <dx:ASPxCheckBox ID="ASPxCheckBox1" runat="server" Text="Show Tab 2">
                                <ClientSideEvents CheckedChanged="function(s, e) {
                                      var tab = pageControl.GetTab(1);
                                      var isVisible = s.GetChecked();
                                      tab.SetVisible(isVisible);
                                    }" />
                            </dx:ASPxCheckBox>
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
                <dx:TabPage ClientVisible="False" Text="Tab 2">
                    <ContentCollection>
                        <dx:ContentControl runat="server">
                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Label on Tab 2">
                            </dx:ASPxLabel>
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
            </TabPages>
        </dx:ASPxPageControl>
    </div>
    </form>
</body>
</html>
