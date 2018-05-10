<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Views/SSPStart.Master" CodeBehind="Dashboard.aspx.cs" Inherits="SSPWebUI.Views.MainDashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Menu" runat="server">
    <span id="tabid" style="display: none">dashboard</span>

    <script>
        $(document).ready(function () {            
            $('#admin').hide();
        })
    </script>
    <style type="text/css">

        .dropdown{width:100%;color:black;}
         .GridView 
    {
        float:left;
    }
    .GridView TR TH
{
        font-family: Arial, Helvetica, sans-serif;
        font-size: 12px;
        color:White;
        text-decoration: none;
        line-height: 22px;
        padding-left:5px;
        border-left:solid 1px gray;
        border-right:solid 1px gray;
        border-bottom:solid 1px gray;
}
.GridView TR TD
{
        font-family: Arial, Helvetica, sans-serif;
        font-size: 12px;
        /*color:Black;*/
        font-weight:bold;
        /*background-color:white;*/
        text-decoration: none;
        line-height: 22px;
        padding-left:5px;
        border-left:solid 1px lightgray;
        border-right:solid 1px lightgray;
        border-bottom:solid 1px lightgray;
}
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Body" runat="server">
    
    
    <div id="content" style="overflow:auto; height:84vh; background-color:#e1ebf4"> 
          <asp:DropDownList ID="ddlProtocolGroups" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddlProtocolGroups_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>
        <br />
        <br />
        <asp:GridView ID="grdDashboard" runat="server" AutoGenerateColumns="false"
             OnRowDataBound="grdDashboard_RowDataBound" OnSorting="grdDashboard_Sorting" AllowSorting="true" CssClass="GridView" 
            AllowPaging="true"  OnPageIndexChanging="grdDashboard_PageIndexChanging" Width="100%" >
             
            <%--<rowstyle backcolor="LightCyan"  
               forecolor="DarkBlue"
               font-italic="true"/>

            <alternatingrowstyle backcolor="PaleTurquoise"  
              forecolor="DarkBlue"
              font-italic="true"/>--%>

            <rowstyle backcolor="#a4cae6"  
               forecolor="black"
               />

            <alternatingrowstyle backcolor="#dee5f4"  
              forecolor="black"
              />
            <PagerStyle HorizontalAlign="Right" />
            
            <pagersettings mode="NextPrevious"
            firstpagetext="First"
            lastpagetext="Last"
            nextpagetext="Next"
            previouspagetext="Prev"   
            position="Top"/>

            <Columns>
                <asp:TemplateField ItemStyle-Width = "200px"  HeaderText = "Protocol CKey" Visible="false">
                
                 <ItemTemplate>
                    <asp:Label ID="lblProtocoCKey" runat="server"
                        Text='<%# Eval("ProtocolVersionCKey")%>'></asp:Label>
                    </ItemTemplate>  
                 </asp:TemplateField>

                <%--<asp:TemplateField ItemStyle-Width = "200px"  SortExpression="ProtocolName" HeaderText = "Protocol Name">--%>
                <asp:TemplateField ItemStyle-Width = "20%"  SortExpression="ProtocolName" HeaderText = "Protocol Name">
                    <ItemTemplate>
                        <%--<asp:HyperLink ID="lnkProtocolName" runat="server"                             
                            NavigateUrl='<%# string.Concat("ProtocolEditor.aspx?ProtocolID=",Eval("ProtocolVersionCKey"))%>'
                            Text ='<%# Eval("ProtocolName")%>'></asp:HyperLink>--%>
                        <asp:Label runat="server" Text ='<%# Eval("ProtocolName")%>'></asp:Label>
                        </ItemTemplate>  
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-Width = "10%" SortExpression="ProtocolGroup" HeaderText = "Protocol Group">
                
                    <ItemTemplate>
                        <asp:Label ID="protocolgroup" runat="server"
                        Text='<%# Eval("protocolgroup")%>'></asp:Label>
                    </ItemTemplate>  
                </asp:TemplateField>
                 <asp:TemplateField ItemStyle-Width = "10%" SortExpression="WebPostingDate"  HeaderText = "Webposting Date">
                    <ItemTemplate>
                        <asp:Label ID="webpostingdate" runat="server"
                        Text='<%# Eval("WebPostingDate")%>'></asp:Label>
                    </ItemTemplate>  
                 </asp:TemplateField>
                
                <asp:TemplateField ItemStyle-Width = "10%"  HeaderText = "Protocol Version">
                    <ItemTemplate>
                        <asp:Label ID="lblProtocolVersion" runat="server"
                            Text='<%# Eval("Version")%>'></asp:Label>
                    </ItemTemplate>  
                 </asp:TemplateField>

               <asp:TemplateField ItemStyle-Width = "10%" HeaderText="Draft Version">
                        <ItemTemplate>
                            <asp:DropDownList id="ddlVersions" AutoPostBack="true" OnSelectedIndexChanged="ddlVersions_SelectedIndexChanged" CssClass="dropdown" runat="server">
                                </asp:DropDownList>
                        </ItemTemplate>                        
                </asp:TemplateField>

                <asp:TemplateField ItemStyle-Width = "10%" HeaderText="Edit Status">
                        <ItemTemplate>
                            <asp:DropDownList id="ddlEditStatus" CssClass="dropdown" runat="server">
                                </asp:DropDownList>
                        </ItemTemplate>
                        
              </asp:TemplateField>

                <asp:TemplateField ItemStyle-Width = "10%" HeaderText="Review Status">
                        <ItemTemplate>
                            <asp:DropDownList id="ddlReviewStatus" CssClass="dropdown" runat="server">
                                </asp:DropDownList>
                        </ItemTemplate>
                        
              </asp:TemplateField>

                <asp:TemplateField ItemStyle-Width = "10%" HeaderText="eCC Status">
                        <ItemTemplate>
                            <asp:DropDownList id="ddleCCStatus" CssClass="dropdown" runat="server">
                                </asp:DropDownList>
                        </ItemTemplate>
                        
              </asp:TemplateField>

                <asp:TemplateField ItemStyle-Width = "10%" HeaderText="Publication Status">
                        <ItemTemplate>
                            <asp:DropDownList id="ddlPublicationStatus" CssClass="dropdown" runat="server">
                                </asp:DropDownList>
                        </ItemTemplate>
                        
              </asp:TemplateField>

                <asp:TemplateField HeaderText="Protocol Format">
                     <ItemTemplate>
                            <a id='html' runat="server">HTML</a>&nbsp;<a id='word' runat="server">Word</a>&nbsp;<a id='pdf' runat="server">PDF</a>
                        </ItemTemplate> 
                        
              </asp:TemplateField>
            
            </Columns>
            <HeaderStyle
                    BackColor="#989898" 
                    BorderColor="Gray" 
                    Font-Bold="True" 
                    ForeColor="White" 
                    Height="20px" />
                    
                <RowStyle HorizontalAlign="Left" Height="20px" />
        </asp:GridView>
    </div>
    
</asp:Content>