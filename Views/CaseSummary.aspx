<%@ Page Title="" Language="C#" MasterPageFile="~/Views/SSPEditor.Master" AutoEventWireup="true" CodeBehind="CaseSummary.aspx.cs" Inherits="SSPWebUI.Views.CaseSummary" %>
<%@ MasterType VirtualPath="~/Views/SSPEditor.Master" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v16.2, Version=16.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v16.2, Version=16.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Menu" runat="server">
    <span id="tabid" style="display: none">casesummary</span>
</asp:Content>
<asp:Content ID="CaseSummaryContent" ContentPlaceHolderID="ContentPlaceHolder_Body" runat="server">
    <asp:HiddenField runat="server" ID="allowtreeedit" />
    
     
    <div id="dlgInsertChecklistItem" style="display:none;width:400px;height:460px;background-color:lightsteelblue" title="Add Checklist Item">
        <table style="margin-top:20px;margin-left:8px;padding:4px">
            <tr style="vertical-align:text-top">
                <td>
                    Visible Text:
                </td>
                <td>
                    <textarea id="txtVisibleText" rows="5" cols="65"></textarea>
                    
                </td>
            </tr>
            <tr>
                <td>
                    Item Type:
                </td>
                <td>
                    <select id="myItemTypes"></select>
                </td>
            </tr>
            <tr >
                <td>
                    Required Status:
                </td>
                <td>
                    <select id="myRequiredStatus""></select>
                </td>
            </tr>
            <tr >                
                <td>
                    Condition
                    </td>
                <td> 
                        <input type ="text" style="width:300px" id="txtCondition"/>
                    
                    
                </td>
            </tr>
            <tr >
                <td>
                    Notes:
                </td>
                <td>
                    <select id="myNotesList" style="width:150px;height:120px" multiple="multiple"></select>
                    
                </td>
            </tr>
        
            <tr>
                <td colspan="2">
                     <fieldset>
                        <legend>Where do you want to add new item?</legend>
                            <input type="radio" onchange="toggleParentNewItem(this)" name="parent" value="root" />Add to root<br />
                            <input type="radio" onchange="toggleParentNewItem(this)" name="parent" value="selected" checked="checked" />Add to selected item <br />  
                         <div id="vistxt" style="margin-top:5px;border-top:solid;background-color:aqua"></div>
                        <input type="hidden" id="txtParentItem" />                         
                            
                    </fieldset>
                    
                </td>
            </tr>
        
        </table>
    </div>

    <asp:UpdatePanel ID="treePanel"  runat="server">
            <ContentTemplate>
                <asp:TextBox ID="txtProtocolVersion" ClientIDMode="Static" ForeColor="White" ReadOnly="true" BackColor="LightGray" runat="server"></asp:TextBox>
                <asp:TextBox ID="txtProtocolVersionValue" ClientIDMode="Static" ForeColor="White" readonly="true" BackColor="LightGray"  runat="server"></asp:TextBox>
                <asp:HiddenField ID="hdnCurrentItem" ClientIDMode="Static" runat="server" />
                <asp:HiddenField runat="server" ClientIDMode="Static" ID="checklistnotes" />
                <b> Case Summary List: </b>
                <asp:DropDownList ID="ddChecklists" AutoPostBack="true" OnSelectedIndexChanged="ddChecklists_SelectedIndexChanged" runat="server"></asp:DropDownList>
                <br />
                <br />
                <button runat="server" id="btnExpand" title="Expand all" style="height:25px" onserverclick="btnExpand_ServerClick">
                    <img style="height:20px" src="../Images/arrow_down.png" />                                            
                </button>
                <button runat="server" id="btnCollapse" style="height:25px" title="Collapse all" onserverclick="btnCollapse_ServerClick">
                        <img style="height:20px" src="../Images/arrow_up.png" />                                            
                </button>
                <button id="btnNew"  style="height:25px" title="Insert a new item" onclick="addChecklistItem();return false;"><img style="height:20px" src="../Images/New.PNG" /></button>
                <button id="btnDelete" style="height:25px" title="Delete selected item" onclick="deleteNode();return false;"><img style="height:20px" src="../Images/Delete.PNG" /></button>
                    
        
        <div style="position:absolute;overflow:auto;height:70%">
                <dx:ASPxTreeList ID="treeList"             
                    KeyFieldName="ChecklistTemplateItemCkey" ParentFieldName="ParentItemCKey"        
                    runat="server"  
                    ClientInstanceName="mytreeList"
                    SettingsBehavior-AutoExpandAllNodes="false" 
                    SettingsBehavior-AllowDragDrop="false" 
                    Settings-GridLines="Both"
                    OnHtmlDataCellPrepared="treeList_HtmlDataCellPrepared"
                    OnNodeUpdating="treeList_NodeUpdating" 
                    OnNodeInserting="treeList_NodeInserting"                                              
                    OnCustomCallback="treeList_CustomCallback"   
                    OnNodeDeleting="treeList_NodeDeleting"  
                    OnProcessDragNode="treeList_ProcessDragNode"                   
                    SettingsCookies-StoreColumnsWidth="true"   
                    SettingsCookies-CookiesID="TreeColumnWidths"                                         
                    SettingsBehavior-ColumnResizeMode="Control"
                    SettingsBehavior-AllowSort="false"
                    OnCustomDataCallback="treeList_CustomDataCallback">                 
                                                   
                    <Columns>                                                
                    
                                                
                        <dx:TreeListTextColumn FieldName="VisibleText" Name ="VisibleText" VisibleIndex="2" Width="250px" CellStyle-Wrap="True">
                          
                            <DataCellTemplate>
                                                     
                                <label><%#Eval("VisibleText")%></label>
                                    <a href='#' onclick="javascript:mytreeList.PerformCallback('RESTORE|' + '<%# Container.NodeKey.Substring(1)%>');">
                                        <dx:ASPxImage ID="Restore" runat="server"   
                                            ImageUrl="../Images/restore.png" Visible="false" ></dx:ASPxImage>
                                </a>
                                                                                       
                                                            
                                <dx:ASPxImage  ID="New" runat="server" Visible="false"  
                                    ImageUrl="../Images/new.png"></dx:ASPxImage>
                               
                                <dx:ASPxImage ID="Trash" runat="server" ShowLoadingImage="true" 
                                    ImageUrl="../Images/Recycle1.png" Visible ="false"></dx:ASPxImage>
                                                        

                            </DataCellTemplate>
                            <EditCellTemplate>
                                <dx:ASPxTextBox ID="VisibleTextbox" runat="server" Text='<%#Eval("VisibleText") %>'></dx:ASPxTextBox>
                            </EditCellTemplate>
                                                                          
                        </dx:TreeListTextColumn>                                         
                        <dx:TreeListTextColumn FieldName="TypeShortName" Width="150px" VisibleIndex="3" Caption="Item Type" Name="ItemType" >
                            
                            <EditCellTemplate>
                                <asp:DropDownList ID="ItemTypeList" CssClass="dropdownstyle" Text='<%# Eval("TypeShortName") %>'  
                                    DataTextField="TypeShortName" DataValueField="ItemTypeKey" SelectedValue='<%# Eval("ItemTypeKey") %>'   
                                    DataSourceID="ItemTypesSource" runat="server"></asp:DropDownList>                                                                                                                   
                            </EditCellTemplate>
                        </dx:TreeListTextColumn>

                            <dx:TreeListTextColumn FieldName="Required" VisibleIndex="3" Width="100px" Caption="Required">                                                    
                            <EditCellTemplate>  
                                <asp:DropDownList onchange="CheckConditionSelected(this)" ID="RequiredList" AutoPostBack="false" SelectedValue='<%# Eval("Required") %>'  
                                    runat="server">                                                          
                                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>                                                            
                                    <asp:ListItem Text="Conditional" Value="Conditional"></asp:ListItem> 
                                </asp:DropDownList>                                                                                                                   
                            </EditCellTemplate>
                        </dx:TreeListTextColumn>

                        <dx:TreeListTextColumn FieldName="Condition" VisibleIndex="4" Width="200px" CellStyle-Wrap="True" Caption="Condition">   
                            <EditCellTemplate>
                                                                                                              
                                
                                <dx:ASPxTextBox ClientInstanceName="cltConditionText" ID="ConditionText" runat="server" Text='<%# Eval("Required")=="Conditional"?Eval("Condition"):""%>'></dx:ASPxTextBox>
                            </EditCellTemplate>
                        </dx:TreeListTextColumn>


                        <dx:TreeListTextColumn FieldName="Notes" Caption="Notes" VisibleIndex="5" Width="200px" Name="Notes" PropertiesTextEdit-EncodeHtml="false">
                            
                            <EditCellTemplate>
                                <dx:ASPxDropDownEdit ClientInstanceName="checkComboBox" CssClass="dropdownstyle" ID="ASPxDropDownEdit1" Text='<%# Eval("NotesAlt")%>' runat="server" AnimationType="None">
                                    <DropDownWindowStyle BackColor="#EDEDED" />
                                    <DropDownWindowTemplate>
                                        <dx:ASPxListBox Width="100%" ID="listBox" DataSourceID="NotesSource" TextField="Tag" ValueField="CKey" ClientInstanceName="checkListBox" SelectionMode="CheckColumn"
                                            runat="server">
                                            <Border BorderStyle="None" />
                                            <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />                                                                                            
                                            <ClientSideEvents SelectedIndexChanged="OnListBoxSelectionChanged" />
                                        </dx:ASPxListBox>
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="padding: 4px">
                                                    <dx:ASPxButton ID="ASPxButton1" AutoPostBack="False" runat="server" Text="Close" style="float: right">
                                                        <ClientSideEvents Click="function(s, e){ checkComboBox.HideDropDown(); }" />
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </DropDownWindowTemplate>
                                    <ClientSideEvents TextChanged="SynchronizeListBoxValues" DropDown="SynchronizeListBoxValues" />
                                    </dx:ASPxDropDownEdit>
                                </EditCellTemplate>
                        </dx:TreeListTextColumn> 
                        <dx:TreeListTextColumn Name="Status" VisibleIndex="6" FieldName="Status" Visible="false" ></dx:TreeListTextColumn>
                                                
                    
                                                
                    </Columns>

                    <ClientSideEvents NodeClick="function(s,e){  
                                setCurrentItemKey(e.nodeKey);                
                                if (e.nodeKey != s.GetFocusedNodeKey() && editingnode != null) {
                                    s.UpdateEdit(editingnode);
                                }

                                else {
                                    if (s.IsEditing()) {

                                        return;
                                    }
                                    else {
                                            //mytreeList.PerformCallback('SETIMAGE|' + e.nodeKey);
                                    }
                                }
                                                    
                                                
                        }" />

                        <ClientSideEvents NodeDblClick="function(s,e){  
                            /*Enter edit mode by double-clicking on a row, and click somewhere else in the treelist to get out
                            of the dit mode  */
                        if (document.getElementById('allowtreeedit'))
                            if (document.getElementById('allowtreeedit').value!='true')
                                return;
                                if(!s.IsEditing()) {  
                                editingnode = e.nodeKey;                                                     
                                s.StartEdit(e.nodeKey);
                                }                                              
                                                    
                                                
                        }" />
                  
                                            
                    <ClientSideEvents FocusedNodeChanged="function(s, e) {
                            s.GetSelectedNodeValues('VisibleText', OnGetSelectedNodeValuesCallback);
                                                        
                        }" />

                        <ClientSideEvents ColumnResized="function(s, e) {
                                e.processOnServer = true;
                        }" />

                    <ClientSideEvents CustomButtonClick="function(s, e) {
                            if(e.buttonID=='New')
                        {
                            editingnode = e.nodeKey;
                            s.StartEditNewNode(e.nodeKey);
                        }

                        if(e.buttonID=='Delete')
                        {
                            deletingnode = e;    
                            CanDelete(e.nodeKey);
                        }  
                                                
                        if(e.buttonID=='Restore')
                        {   alert('hello');
                             $.get('../Service/SSPService.asmx/CanRestore', { parm: keyValue }, function (data) {
                                    var $xml = $(data);
                            

                                    data = $xml.find('boolean').text();
                                    alert(data);
                                    if (data == 'false') {            
                                        $.confirm({
                                            title: 'Cannot delete!',
                                            content: 'Selected item cannot be restored since parent is also deprecated. Please restore the parent item first.',
                                            type: 'red',
                                            boxWidth: '500px',
                                            useBootstrap: false,
                                            typeAnimated: true,
                                            buttons: {
                  
                                                close: function () {
                                                }
                                            }
                                        });

                                       
                                    }
                                    else
                                    {
                                         mytreeList.PerformCallback(e.nodeKey)
                                    } });                                  
                           
                        }     
                        }" />

                     <ClientSideEvents EndDragNode="function(s, e) {
                         //alert(e.targetElement.nodeKey);
	                     //   s.CollapseNode(e);
	                    }" />

                    <SettingsBehavior AllowFocusedNode="true" />
                </dx:ASPxTreeList>
                </div>            
            </ContentTemplate>
        </asp:UpdatePanel>  
    
    <asp:ObjectDataSource ID="DataTypesSource" runat="server" SelectMethod="GetDataTypes" TypeName="SSPWebUI.Data.DataTypes"></asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ItemTypesSource" runat="server" SelectMethod="GetItemTypes" TypeName="SSPWebUI.Data.ItemTypes"></asp:ObjectDataSource>
       <asp:ObjectDataSource ID="NotesSource" runat="server" SelectMethod="getProtocolNotes" TypeName="SSPWebUI.Data.Note">
             <SelectParameters>
                 <asp:ControlParameter Name="ProtocolVersionCKey" Type="decimal" ControlID="txtProtocolVersion" />
                
             </SelectParameters>
        </asp:ObjectDataSource>  
</asp:Content>
