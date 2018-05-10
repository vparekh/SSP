<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TreelistWithObjectSource.aspx.cs" Inherits="SSPWebUI.TreelistWithObjectSource" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v16.2, Version=16.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>

<%@ Register assembly="DevExpress.Web.v16.2, Version=16.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        /* cellpadding */
        th, td { padding: 2px; }

        /* cellspacing */
        table { border-collapse: separate; border-spacing: 2px; } /* cellspacing="5" */
        table { border-collapse: collapse; border-spacing: 0; }   /* cellspacing="0" */

        /* valign */
        th, td { vertical-align: top; }

        /* align (center) */
        /*table { margin: 0 auto; }*/
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    <dx:ASPxTreeList ID="treeList" 
        KeyFieldName="ChecklistTemplateItemCkey" ParentFieldName="ParentItemCKey"        
        runat="server" DataSourceID="CheckListItemsSource" SettingsBehavior-AutoExpandAllNodes="true" 
        SettingsBehavior-AllowDragDrop="true" 
        Settings-GridLines="Both"
        
        OnNodeUpdating="treeList_NodeUpdating" OnNodeInserting="treeList_NodeInserting" SettingsEditing-Mode="EditForm">
        
        <Templates>
                <EditForm>
                     <dx:ASPxPageControl ID="tabs" runat="server" ActiveTabIndex="0" Width="100%">
                         <TabPages>
                            <dx:TabPage Text="Editor" >
                                <ContentCollection>
                                    <dx:ContentControl runat="server">
                                        <dx:ASPxLabel Text="Visible Text" runat="server" />
                                        <dx:ASPxMemo ID="VisibleText" runat="server" Width="100%" hight="50px" Text='<%# Eval("VisibleText") %>' />
                                        <div style="float:left;margin-right:100px;margin-top:10px">
                                            <table >

                                            <tr>
                                                <td><dx:ASPxLabel Text="Min Value" runat="server"></dx:ASPxLabel></td>
                                                <td><dx:ASPxTextBox Width="50px" ID="AnswerMinValue" runat="server" Text='<%# Eval("AnswerMinValue")==null?"": Eval("AnswerMinValue")%>' /></td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel Text="Max Value" runat="server"></dx:ASPxLabel></td>
                                                <td><dx:ASPxTextBox Width="50px" ID="AnswerMaxValue" runat="server" Text='<%# Eval("AnswerMaxValue")==null?"": Eval("AnswerMaxValue")%>' /></td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel Text="Max Chars" runat="server"></dx:ASPxLabel></td>
                                                <td><dx:ASPxTextBox Width="50px" ID="AnswerMaxChars" runat="server" Text='<%# Eval("AnswerMaxChars")==null?"": Eval("AnswerMaxChars")%>' /></td>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel Text="Data Type" runat="server"></dx:ASPxLabel></td>
                                                <td><dx:ASPxComboBox ID="DataType" value='<%# Eval("DataType")==null?"": Eval("DataType")%>'  DataSourceID="DataTypesSource" TextField="DataType" ValueField="DataTypeKey" runat="server" /></td>
                                                <%--<td><dx:ASPxTextBox Width="50px" ID="AnswerDataTypeKey" runat="server" Text='<%# Eval("AnswerDataTypeKey")==null?"": Eval("AnswerDataTypeKey")%>' /></td>--%>
                                            </tr>
                                            <tr>
                                                <td><dx:ASPxLabel Text="Item Type" runat="server"></dx:ASPxLabel></td>
                                                <td><dx:ASPxTextBox Width="50px" ID="ItemTypeKey" runat="server" Text='<%# Eval("ItemTypeKey")==null?"": Eval("ItemTypeKey")%>' /></td>
                                            </tr>
                                                                                     
                                            
                                            </table>
                                        </div>

                                        <div style="float:left; margin-right:100px; margin-top:10px" >
                                            <dx:ASPxCheckBox ID="Required" Text="Required" runat="server" Checked='<%# Eval("Required")==null?"": Eval("Required")%>' />
                                            <dx:ASPxCheckBox ID="Locked" Text="Locked" runat="server" Checked='<%# Eval("Locked")==null?"": Eval("Locked")%>' />
                                            <dx:ASPxCheckBox ID="DeprecatedFlag" Text ="Deprecated" runat="server" Checked='<%# Eval("DeprecatedFlag")==null?"": Eval("DeprecatedFlag")%>' />
                                            <dx:ASPxCheckBox ID="SelectionDisablesChildren" Text="SDC" runat="server" Checked='<%# Eval("SelectionDisablesChildren")==null?"": Eval("SelectionDisablesChildren")%>' />
                                            <dx:ASPxCheckBox ID="SelectionDisablesSiblings" Text="SDS" runat="server" Checked='<%# Eval("SelectionDisablesSiblings")==null?"": Eval("SelectionDisablesSiblings")%>' />
                                        </div>
        
                                        <div style="float:left; margin-right:100px;margin-top:10px">
                                            <dx:ASPxLabel Text="Comment" runat="server" />
                                            <dx:ASPxMemo ID="comment" runat="server" AutoResizeWithContainer="True" Height="100px" Width="500px"
                                                Text='<%# Eval("VisibleText") %>' />
                                        </div>
                                    </dx:ContentControl>
                                </ContentCollection>
                            </dx:TabPage>
                        </TabPages>
                        
                     
                        </dx:ASPxPageControl>
                       
                    <dx:ASPxTreeListTemplateReplacement runat="server" ReplacementType="UpdateButton" />
                    <dx:ASPxTreeListTemplateReplacement runat="server" ReplacementType="CancelButton" />
                
                </EditForm>
            </Templates>
            
        <Columns>
            <dx:TreeListTextColumn FieldName="VisibleText" Width="350px" CellStyle-Wrap="True">
                <EditFormSettings VisibleIndex="0" />
            </dx:TreeListTextColumn>
            <dx:TreeListTextColumn FieldName="AnswerMinValue" PropertiesTextEdit-DisplayFormatString="n3" Width="70px" Caption="Min Val">
                <EditFormSettings VisibleIndex="1" />
            </dx:TreeListTextColumn>
            <dx:TreeListTextColumn FieldName="AnswerMaxValue" PropertiesTextEdit-DisplayFormatString="n3" Width="70px" Caption="Max Val">
                <EditFormSettings VisibleIndex="2" />
            </dx:TreeListTextColumn>
            <dx:TreeListTextColumn FieldName="AnswerMaxChars" Width="70px" Caption="Max Chars">
                <EditFormSettings VisibleIndex="3" />
            </dx:TreeListTextColumn>
            <dx:TreeListTextColumn FieldName="DataType" Width="70px" Caption="Data Type">
                <EditFormSettings VisibleIndex="4" />
            </dx:TreeListTextColumn>
            <dx:TreeListTextColumn FieldName="ItemTypeKey" Width="70px" Caption="Item Type">
                <EditFormSettings VisibleIndex="5" />
            </dx:TreeListTextColumn>
            <dx:TreeListTextColumn FieldName="Required" Width="70px">
                <EditFormSettings VisibleIndex="6" />
            </dx:TreeListTextColumn>
            <dx:TreeListTextColumn FieldName="Locked" Width="70px">
                <EditFormSettings VisibleIndex="7" />
            </dx:TreeListTextColumn>
            <dx:TreeListTextColumn FieldName="DeprecatedFlag" Width="70px" Caption="Deprecated">
                <EditFormSettings VisibleIndex="8" />
            </dx:TreeListTextColumn>
            <dx:TreeListTextColumn FieldName="SelectionDisablesChildren" Caption="SDC" Width="70px">
                <EditFormSettings VisibleIndex="9" />
            </dx:TreeListTextColumn>
            <dx:TreeListTextColumn FieldName="SelectionDisablesSiblings" Caption="SDS" Width="70px">
                <EditFormSettings VisibleIndex="10" />
            </dx:TreeListTextColumn>
            <dx:TreeListCommandColumn ShowNewButtonInHeader="true">
                <EditButton Visible="true" />
                <NewButton Visible="true"  />
                <DeleteButton Visible="true" />
            </dx:TreeListCommandColumn>
        </Columns>

    </dx:ASPxTreeList>
        <asp:ObjectDataSource ID="CheckListItemsSource" runat="server" 
            DeleteMethod="DeleteChecklistItem" 
            InsertMethod="AddChecklistItem" 
            SelectMethod="GetAllChecklistItems" 
            TypeName="SSPWebUI.Data.ChecklistTemplateItems" 
            UpdateMethod="UpdateChecklistItem">
            <DeleteParameters>
                <asp:Parameter Name="checklistitemckey" Type="Decimal" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="ChecklistTemplateItemCkey" Type="Decimal" />
                <asp:Parameter Name="ParentItemCKey" Type="Decimal" />
                <asp:Parameter Name="VisibleText" Type="String" />
                <asp:Parameter Name="AnswerMinValue" Type="Int32" ConvertEmptyStringToNull="true" />
                <asp:Parameter Name="AnswerMaxValue" Type="Int32" ConvertEmptyStringToNull="true" />
                <asp:Parameter Name="AnswerMaxChars" Type="Int32" ConvertEmptyStringToNull="true"/>
                <asp:Parameter Name="AnswerDataTypeKey" Type="Int32" ConvertEmptyStringToNull="true"/>
                <asp:Parameter Name="ItemTypeKey" Type="Int32" ConvertEmptyStringToNull="true"/>
                <asp:Parameter Name="Required" Type="Boolean" ConvertEmptyStringToNull="true"/>
                <asp:Parameter Name="Locked" Type="Boolean" ConvertEmptyStringToNull="true"/>
                <asp:Parameter Name="DeprecatedFlag" Type="Boolean" ConvertEmptyStringToNull="true"/>
                <asp:Parameter Name="SelectionDisablesChildren" Type="Boolean" ConvertEmptyStringToNull="true"/>
                <asp:Parameter Name="SelectionDisablesSiblings" Type="Boolean" ConvertEmptyStringToNull="true"/>
            </InsertParameters>
            <SelectParameters>
                <asp:QueryStringParameter DefaultValue="184.100004300" Name="ChecklistTemplateVersionCKey" QueryStringField="ChecklistTemplateVersionCKey" Type="Decimal" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="ChecklistTemplateItemCkey" Type="Decimal" />
                <asp:Parameter Name="ParentItemCKey" Type="Decimal" />
                <asp:Parameter Name="VisibleText" Type="String" />
                <asp:Parameter Name="AnswerMinValue" Type="Int32" ConvertEmptyStringToNull="true" />
                <asp:Parameter Name="AnswerMaxValue" Type="Int32" ConvertEmptyStringToNull="true"/>
                <asp:Parameter Name="AnswerMaxChars" Type="Int32" ConvertEmptyStringToNull="true"/>
                <asp:Parameter Name="AnswerDataTypeKey" Type="Int32" ConvertEmptyStringToNull="true"/>
                <asp:Parameter Name="ItemTypeKey" Type="Int32" ConvertEmptyStringToNull="true"/>
                <asp:Parameter Name="Required" Type="Boolean" ConvertEmptyStringToNull="true"/>
                <asp:Parameter Name="Locked" Type="Boolean" ConvertEmptyStringToNull="true"/>
                <asp:Parameter Name="DeprecatedFlag" Type="Boolean" ConvertEmptyStringToNull="true"/>
                <asp:Parameter Name="SelectionDisablesChildren" Type="Boolean" ConvertEmptyStringToNull="true"/>
                <asp:Parameter Name="SelectionDisablesSiblings" Type="Boolean" ConvertEmptyStringToNull="true"/>
                
            </UpdateParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="DataTypesSource" runat="server" SelectMethod="GetDataTypes" TypeName="SSPWebUI.Data.DataTypes"></asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ItemTypesSource" runat="server"></asp:ObjectDataSource>

    </div>
    </form>
</body>
</html>
