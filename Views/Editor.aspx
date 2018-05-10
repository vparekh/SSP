<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Editor.aspx.cs" EnableEventValidation="false" Inherits="SSPWebUI.Views.Editor" %>
<%@ Register tagprefix="ssp" Tagname="myHeader" src="~/UserControls/Header.ascx" %>
<%@ Register tagprefix="ssp" Tagname="myAuthors" src="~/UserControls/Authors.ascx" %>
<%@ Register tagprefix="ssp" Tagname="myCaseSummary" src="~/UserControls/CaseSummary.ascx" %>

<%@ Register tagprefix="ssp" Tagname="myProcedure" src="~/UserControls/ProcedureEditor.ascx" %>
<%@ Register tagprefix="ssp" Tagname="myReference" src="~/UserControls/ReferenceEditor.ascx" %>
<%@ Register tagprefix="ssp" Tagname="myNote" src="~/UserControls/NoteEditor.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link rel="stylesheet" href="../content/bootstrap.min.css"/>
    <link rel="stylesheet" href="../scripts/jquery-ui-1.12.1/jquery-ui.css"/>
  
    <%-- make sure jquery and jquery-ui are compatible --%>
    <script src="../scripts/jquery-1.12.4.min.js"></script>
    <script src="../Scripts/jquery.blockUI.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>  
    <script src="../Scripts/jquery-ui-1.12.1.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-confirm/3.0.3/jquery-confirm.min.js"></script>
   <%-- <script src="../scripts/SSP.js"></script>--%>

        
    <%--<link rel="stylesheet" href="../content/menu.css"/>--%>
    <link rel="stylesheet" href="../Content/core/core.css" />
   <%-- <link rel="stylesheet" href="../Content/core.css" />--%>
 <%--   <link rel="stylesheet" href="../Content/bootstrap.css" />--%>
    <link rel="stylesheet" href="../Content/core/nav.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/jquery-confirm/3.0.3/jquery-confirm.min.css"/>
    <link rel="stylesheet" href="../content/bootstrap.min.css"/>
    <link rel="stylesheet" href="../Content/jquery-ui.css"/>
     
     
  <!--fix width of dropdown-->
    <style type="text/css">
    .CommandItem {
        display:none;
    }
     .dxtlFocusedNode .CommandItem {
            display:inline;
        }
   
     table#usercomments th {
         padding:10px;
         color:white;
         background-color:black;
     }

     table#usercomments tr:nth-child(even) {background: #CCC}
     table#usercomments tr:nth-child(odd) {background: #FFF}

     select {height:24px;}
     .ui-dialog-titlebar {
          background-color: blue;
          background-image: none;
          color: white;
        }

        td{
            vertical-align:top;
        }

        .dropdownstyle {
        width:150px;
    }
    .dropdownnstyle option {
        width:auto;
    }

    .caret.caret-up {
    border-top-width: 0;
    border-bottom: 4px solid #fff;
    }

</style>

    <script type="text/javascript">

        $(document).ready(function () {

            $("#test").dialog({
                autoOpen: false
            });

            $("#dlgAddComment").dialog({
                autoOpen: false,
                height: 540,
                width: 580,
                modal: true,
                buttons: {
                    "Add": function () {
                        var comment = $('#txtComment').val();
                        var section = "section";
                        var userckey = $('#hdnUserCKey').val()
                        section = $('#sections :selected').val();
                        $(this).dialog("close");
                        dlgAddCommentCallback(comment, section, userckey);
                       
                       
                    },
                    "cancel": function () {
                        $(this).dialog("close");
                    }
                },
                close: function () {

                }


            });
        });


        function dlgAddCommentCallback(usercomment, section, userckey) {
                 

            var comment = new Object();
            comment.UserComment = usercomment;
            comment.CommentSection = section;
            comment.UserCKey = userckey;
            comment.ProtocolVersionCKey = $('#txtProtocolVersion').val();

            $.ajax({
                url: '../api/comments',
                type: 'POST',
                dataType: 'json',
                data: comment,
                success: function (data, textStatus, xhr) {
                    console.log(data);
                },

                error: function (xhr, textStatus, errorThrown) {
                    console.log('Error in Operation');
                }

            });       

        }

        $(document).ready(function () {
            $("#dlgViewComments").dialog({
                autoOpen: false,
                height: 540,               
                width: 580,
                modal: true,
                buttons: {
                    Ok: function () {
                        $(this).dialog("close"); //closing on Ok click
                    }
                }
            });
        });

        function activateDevExpressControls(tabId) {
            ASPxClientControl.AdjustControls(document.getElementById(tabId));
        }

       
       
        function OnProcedureSaveClick(s, e) {
            myProcedures.PerformCallback("save|" + $("#txtProtocolVersion").val());
        }

               

       

        function OpenCommentBox() {
            $('#dlgAddComment').dialog('open');

            $.getJSON("../api/comments",
           function (data) {
               var comments = '';
             
               $.each(data, function (key, val) {
                   comments = comments + val.UserId;
               }); //each
              
               $('#txtComment').val(comments);
           });  //data

        }

        function ViewComment() {

            $('#dlgViewComments').dialog('open');

            var protocol = $("#txtProtocolVersion").val()

            $.getJSON("../api/comments/" + protocol + "/",
              function (data) {
                  
                  //remove all rows except the first
                  $("#usercomments").find("tr:gt(0)").remove();

                  //add rows
                  $.each(data, function (key, val) {
                      $('#usercomments > tbody:last-child').append('<tr><td>'
                          + val.UserId + '</td><td>'
                          + val.DateAdded + '</td><td>'
                          + val.CommentSection + '</td><td>'
                          + val.UserComment + '</td></tr>');

                      
                  }); //each

              });  //data
        }


        function saveAll() {
            //saveHeader();
            myRich.PerformCallback("save|" + $("#txtProtocolVersion").val());
            myProcedures.PerformCallback("save|" + $("#txtProtocolVersion").val());
            myReferences.PerformCallback("save|" + $("#txtProtocolVersion").val());
            return false;
        }

    </script>

   

</head>
<body id="home">
    <form id="form1" runat="server">
        <input type="hidden" id="hdnUserCKey" runat="server" />
        <%--<div id="test" style="display:none;width:400px;height:500px;background-color:lightsteelblue" title="Add Checklist Item">
            hello
        </div>--%>

          <div id="dlgInsertReference" style="visibility:hidden" title="References"></div>
        <%--<div id="dlgInsertChecklistItem" style="display:none;width:400px;height:500px;background-color:lightsteelblue" title="Add Checklist Item">--%>
        <div id="dlgInsertChecklistItem" style="display:none;width:450px;height:500px;background-color:lightsteelblue" title="Add Checklist Item">
        <table style="margin-top:20px;margin-left:8px;padding:4px">
            <tr style="vertical-align:text-top">
                <td>
                    Visible Text:
                </td>
                <td>
                    <textarea id="txtVisibleText" rows="5" cols="50"></textarea>                    
                </td>
            </tr>
            <tr>
                <td>
                    Item Type:
                </td>
                <td>
                    <select  id="myItemTypes"></select>
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
                        <input type ="text" style="width:400px" id="txtCondition"/>              
                    
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

        <div id="dlgAddComment" runat="server" title="Add Comments" style="display:none">
            <asp:DropDownList ClientIDMode="Static" ID="sections" runat="server">
                <asp:ListItem Text="Title" Value="title"></asp:ListItem>
                <asp:ListItem Text="Procedure" Value="procedure"></asp:ListItem>
                <asp:ListItem Text="Authors" Value="authors"></asp:ListItem>                
                <asp:ListItem Text="Case Summary" Value="casesummary"></asp:ListItem>
                <asp:ListItem Text="Explanatory Notes" Value="notes"></asp:ListItem>
                <asp:ListItem Text="References" Value="references"></asp:ListItem>
            </asp:DropDownList>
                <br />
                <br />
            <asp:TextBox ID="txtComment" TextMode="MultiLine" Rows="24" Columns="70" ClientIDMode="Static" runat="server"></asp:TextBox>
        </div>

        <div id="dlgViewComments" runat="server" title="Comments" style="display:none">
            <table id="usercomments">
             <tbody>
                 <tr>
                     <th>Reviewer</th>
                     <th>Date</th>
                     <th>Section</th>
                     <th>Comment</th>
                 </tr>
             </tbody>

                
            </table>
        </div>
        <asp:ScriptManager runat="server"></asp:ScriptManager>
         
  
        <div id="container"  class="clearfix" > 
              <div ><h1><img src="../Images/CAPLogo.PNG" alt="CAP" width="501" height="101" /></h1> </div>
             
             <div style="float:left;margin-top:10px;margin-bottom:10px;align-content:center">
                <asp:Label ID="lblGreeting" Font-Bold="true" Font-Size="Large" runat="server">Greeting here</asp:Label>
            </div>                        

            <div style="float:right">
                <asp:Label ID="lblUserName" runat="server" Font-Size="Smaller"></asp:Label>&nbsp;
                <a href="/SSPHTML/MainPage.html"> <asp:Image ID="Image2" width="20" height="20" alt="Home Page" ImageUrl="~/Images/home_page.jpg" runat="server" />  </a>  
                <p><asp:Label ID="lblDate" runat="server" Font-Size="Smaller"></asp:Label>
                <a id="signin" runat="server" style="font-size:smaller" href="~/Views/LogIn.aspx">Sign out</a></p>
            </div>
            <div style="clear:both" />
            <hr />
                <asp:LinkButton ID="refreshbutton" runat="server"></asp:LinkButton>          
            
                
            <asp:UpdatePanel ID="parentPanel" runat="server">
                <ContentTemplate>
                   <%-- #C3D9FF--%>
             <div id="content" style="background-color: darkgray;padding:10px;width:100%">
                
                <div style="margin-top:20px;margin-bottom:20px;clear:both">
                    <b> Protocol List: </b>
                    <asp:DropDownList ID="Protocols" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="Protocols_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>
                    <div style="margin-left:200px; display:inline">
                        <asp:UpdateProgress ID="UpdateProgress1" DisplayAfter="1" AssociatedUpdatePanelID="parentPanel" runat="server">                            
                            <ProgressTemplate>
                            <img src="../images/ajax-loader.gif"/> Loading ... 
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    </div>
                    
                </div>
                <%--<div id="showAddComment" runat="server" style="float:right"><i class="glyphicon glyphicon-edit" style="font-size: 40px;color:white" title="Add comments" onclick="OpenCommentBox();return false;"></i></div>--%>
                 <div id="showViewComments" runat="server" style="float:right"><i class="glyphicon glyphicon-comment" style="font-size: 40px;color:white" title="View comments" onclick="ViewComment();return false;"></i></div>
              
                       <ul class="nav nav-tabs" id="maintab">
                    <li class="active"><a data-toggle="tab" href="#headers">Title</a></li>
                    <li><a data-toggle="tab" href="#procedures">Procedures</a></li>
                    <li><a data-toggle="tab" href="#authors">Authors</a></li>
                    <li><a data-toggle="tab" href="#checklist">Case Summary</a></li>
                    <li><a data-toggle="tab" href="#notes">Explanatory Notes</a></li>
                    <li><a data-toggle="tab" href="#references">References</a></li>                                 

                </ul>
                <%-- #E6EFF5;--%>
          <%--background-color: #C3D9FF--%>
                <div class="tab-content" style="border-width:2px;  position:relative;border-style:none; background-color: silver;width:100%;clear:both;overflow:scroll" >                            
                  
                    <div id="headers" style="border-width:2px; border-style:none;height:60vh;padding:10px" class="tab-pane fade in active">
                            <ssp:myHeader runat="server" id="hdr"></ssp:myHeader>
					</div>

                    <div id="procedures" style="border-color:white;border-width:2px; border-style:none;height:60vh;padding:10px" class="tab-pane fade" onclick="activateDevExpressControls()">
                            <ssp:myProcedure id="MyProcedure1" runat="server" />
                    </div>
                    
                    <div id="authors" style="border-color:white;border-width:2px; border-style:none;height:60vh;padding:10px" class="tab-pane fade">
                            <p style="font-weight:bold">Authors</p>
                        <ssp:myAuthors id="MyAuthors" runat="server"></ssp:myAuthors>
                    </div>
                    
                    <div id="checklist" style="border-color:white;border-width:2px; border-style:none;height:60vh;padding:10px" class="tab-pane fade" onclick="activateDevExpressControls()">
                        <ssp:myCaseSummary id="mySummary" runat="server"></ssp:myCaseSummary>
                    </div>
                    
                    <div id="notes" style="border-color:white;border-width:2px; border-style:none;height:60vh;padding:10px" class="tab-pane fade" onclick="activateDevExpressControls()">
                        <ssp:myNote id="MyNote" runat="server" />
                    </div>

                    <div id="references" style="border-color:white;border-width:2px; border-style:none;height:60vh;padding:10px" class="tab-pane fade" onclick="activateDevExpressControls()">
                        <ssp:myReference id="MyReference" runat="server" />
                           
                    </div>
                           
                    <asp:Button ID="btnSaveAll" Text="Save All (Draft)" OnClientClick="saveAll();return false;" runat="server" />
                    <asp:Button ID="btnSubmit" Text="Submit" OnClientClick="alert('to be implemented'); return false;" runat="server" />
                </div>    <%--tab-content --%>    
                 <asp:HiddenField ID="hfTab" ClientIDMode="Static" runat="server" />
                 
                 </ContentTemplate>
               <Triggers>
                   <asp:AsyncPostBackTrigger ControlID="Protocols" EventName="SelectedIndexChanged" />
               </Triggers>
                   
                </asp:UpdatePanel>
              </div>            
        
        
    
      
    </form>

    <script>
        function pageLoad() {          
             
           maintainSelectedTab();
       }

       function maintainSelectedTab() {
           selectedTab = $("#<%=hfTab.ClientID%>");
           var tabId = selectedTab.val() != "" ? selectedTab.val() : "headers";

           $('#container a[href="#' + tabId + '"]').tab('show');

           activateDevExpressControls(tabId);
           $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {

               var tabId = selectedTab.val() != "" ? selectedTab.val() : "headers";
               activateDevExpressControls(tabId);
           });


           $("#container a").click(function () {
               
               //ignore if not the tabs
               if ($.inArray($(this).attr("href"), ["#authors", "#title", "#notes", "#procedures", "#headers", "#checklist", "#references"]) == -1)
                   return;

               if ($(this).attr("href") != null)
               {
                  
                   selectedTab.val($(this).attr("href").substring(1));
               }
                   

           });
       }
   </script>

</body>
</html>