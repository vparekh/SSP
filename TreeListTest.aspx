%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TreeListTest.aspx.cs" Inherits="SSPWebUI.TreeListTest" %>

<%@ Register Assembly="DevExpress.Web.v16.2, Version=16.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v16.2, Version=16.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="height:90%">
        <dx:ASPxTreeList ID="treeList" runat="server" Width="100%" Settings-GridLines="Both"  Height="500px" SettingsBehavior-AllowDragDrop="true" 
            OnNodeUpdating="treeList_NodeUpdating" OnNodeInserting="treeList_NodeInserting" SettingsEditing-Mode="EditForm" >
            <Templates>
                <EditForm>
                     
                    <dx:ASPxPageControl ID="tabs" runat="server" ActiveTabIndex="0" Width="100%">
                        
                        <TabPages>
                            <dx:TabPage Text="editor">
                                <ContentCollection>
                                    <dx:ContentControl runat="server">
                                        <dx:ASPxLabel Text="Visible Text" runat="server" /><dx:ASPxMemo ID="vistext" runat="server" AutoResizeWithContainer="True" Height="50px"
                                            Width="100%" Text='<%# Eval("ItemText") %>' />
                                       
                                    </dx:ContentControl>
                                </ContentCollection>
                            </dx:TabPage>
                        </TabPages>
                        
                     
                        </dx:ASPxPageControl>
                       
                    <dx:ASPxTreeListTemplateReplacement runat="server" ReplacementType="UpdateButton" />
                    <dx:ASPxTreeListTemplateReplacement runat="server" ReplacementType="CancelButton" />
                </div>
                </EditForm>
            </Templates>
        <Columns>
            <dx:TreeListDataColumn FieldName="ItemText" Caption="File name" Width="70%" CellStyle-Wrap="True" />
            <dx:TreeListDataColumn FieldName="MinRepetitions" Caption="Min Repetitions" Width="10%" />
            <dx:TreeListDataColumn FieldName="MaxRepetitions" Caption="Max Repetitions" Width="10%" />
            <dx:TreeListCommandColumn ShowNewButtonInHeader="False" VisibleIndex="3" Caption="Command">
                <EditButton Visible="True" />
                <NewButton Visible="True" />
            </dx:TreeListCommandColumn>
        </Columns>
        <SettingsBehavior ExpandCollapseAction="NodeDblClick" />
    </dx:ASPxTreeList>
    </div>
    </form>
</body>
</html>
