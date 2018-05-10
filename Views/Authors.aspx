<%@ Page Title="" Language="C#" MasterPageFile="~/Views/SSPEditor.Master" AutoEventWireup="true" CodeBehind="Authors.aspx.cs" Inherits="SSPWebUI.Views.Authors" %>
<%@ MasterType VirtualPath="~/Views/SSPEditor.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Menu" runat="server">
    <span id="tabid" style="display: none">authors</span>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Body" runat="server">
     <asp:HiddenField runat="server" ID="allowtreeedit" />
   
      <div id="authors" style="border-color:white;border-width:2px; border-style:solid;" class="tab-pane fade">
                                <p style="font-weight:bold">Authors</p>
                
                
                                <div id = "dvGrid" style ="padding:10px;width:550px">                                    
                                    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">


                                        <%-- clicking on refreshbutton will trigger update --%>
                                        <Triggers >
                                            <%--<asp:PostBackTrigger ControlID="refreshbutton" />--%>
                                            <asp:AsyncPostBackTrigger ControlID="refreshbutton" />
                                        </Triggers>
                                        <ContentTemplate>  
                                             <asp:TextBox ID="txtProtocolVersion" ClientIDMode="Static" ForeColor="White" ReadOnly="true" BackColor="LightGray" runat="server"></asp:TextBox>
                                            <asp:TextBox ID="txtProtocolVersionValue" ClientIDMode="Static" ForeColor="White" readonly="true" BackColor="LightGray"  runat="server"></asp:TextBox>                          
                                            <asp:GridView ID="grdAuthors" runat="server"
                                                AutoGenerateColumns = "false"
                                                OnRowCommand="grdAuthors_RowCommand"
                                                OnRowUpdating="grdAuthors_RowUpdating"
                                                OnRowDeleting="grdAuthors_RowDeleting"
                                                OnRowEditing="grdAuthors_RowEditing"
                                                OnRowCancelingEdit="grdAuthors_RowCancelingEdit"
                                                OnRowDataBound="grdAuthors_RowDataBound"                                
                                                ShowFooter="false">                   
                                                <Columns>                        
                         
                                                    <asp:TemplateField ItemStyle-Width = "200px"  HeaderText = "Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAuthorName" runat="server"
                                                    Text='<%# Eval("Name")%>'></asp:Label>
                                            </ItemTemplate>  
                                                <EditItemTemplate>
                                                <asp:DropDownList ID="lstAuthors" OnSelectedIndexChanged="lstAuthors_SelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="Name" DataValueField="AuthorCKey"> </asp:DropDownList>
                                            </EditItemTemplate>
                                                   
                                        </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-Width = "200px"  HeaderText = "Role">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRole" runat="server"
                                                    Text='<%# Eval("Role")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="lstRoles" OnSelectedIndexChanged="lstRoles_SelectedIndexChanged" runat="server" DataTextField="RoleName" DataValueField="RoleCKey" > </asp:DropDownList>
                                            </EditItemTemplate>
                              
                                            </asp:TemplateField>
                         
                                                    <asp:TemplateField ShowHeader="False" HeaderText = "Edit"> 
                                                        <EditItemTemplate> 
                                                            <asp:LinkButton ID="LinkButton1" runat="server" 
                                                        CausesValidation="True"  
                                                        CommandArgument = '<%# String.Format("{0} - {1}", Eval("AuthorCKey"), Eval("RoleCKey")) %>'
                                                        CommandName="Update" 
                                                        Text="Update"></asp:LinkButton> 
                                                            <asp:LinkButton ID="LinkButton2" runat="server" 
                                                        CausesValidation="False"  
                                                        CommandName="Cancel" 
                                                        Text="Cancel"> </asp:LinkButton> 
                                                        </EditItemTemplate> 
                                                        <ItemTemplate> 
                                                            <asp:LinkButton ID="lnkEdit" runat="server" 
                                                                CommandName="Edit" 
                                                                CommandArgument = '<%# String.Format("{0} - {1}", Eval("AuthorCKey"), Eval("RoleCKey")) %>'
                                                                Text="Edit"> </asp:LinkButton> 
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                         
                                                    <asp:TemplateField ShowHeader="false" HeaderText = "Delete">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkRemove" runat="server"
                                                                CommandName="Delete"
                                                                CommandArgument = '<%# String.Format("{0} - {1}", Eval("AuthorCKey"), Eval("RoleCKey")) %>'
                                                                OnClientClick = "if(!confirm('Do you want to delete ' + '?')){return false;};"                                    
                                                                Text = "Delete" ></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>  
                                                                         
                                                </Columns>
                                            </asp:GridView>
                                            
                                           
                                            <div id="addAuthor" runat="server">
                                                 <p style="font-weight:bold;margin-top:20px">Add authors</p>
                                                 <p>Please select an authors and a role from below and click Add. </p>
                                                <table>
                                                    <tr>
                                                        <th>Name</th>
                                                        <th>Role</th>
                                                    </tr>
                                                    <tr>
                                                        <td><asp:DropDownList ClientIDMode="Static" ID="lstAddAuthors" runat="server"></asp:DropDownList></td>
                                                        <td><asp:DropDownList ClientIDMode="Static" ID="lstAddRoles" runat="server"></asp:DropDownList></td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <asp:LinkButton ID="lbAdd" runat="server" OnClientClick = "validateAddAuthor()"  OnClick="lbAdd_Click">Add &nbsp;&nbsp;</asp:LinkButton>
                                            <asp:LinkButton ID="lbUser"  runat="server" TabIndex="5" data-toggle="tab" href="#user">&nbsp; New </asp:LinkButton><!-- VP -->
                                        </ContentTemplate>
                                    </asp:UpdatePanel>                    
                                </div>
                            </div>   
</asp:Content>
