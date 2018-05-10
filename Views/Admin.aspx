<%@ Page Title="" Language="C#" EnableEventValidation="false" MasterPageFile="~/Views/SSPStart.Master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="SSPWebUI.Views.Admin" %>

<%@ Register tagprefix="ssp" Tagname="usr" src="~/UserControls/UserForm.ascx" %>
<%@ Register tagprefix="ssp" Tagname="aff" src="~/UserControls/AffiliationForm.ascx" %>
<%@ Register tagprefix="ssp" Tagname="protocol" src="~/UserControls/ProtocolForm.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Menu" runat="server">
    <span id="tabid" style="display: none">admin</span>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Body" runat="server">
<style type="text/css">
#nav {
    margin-top:20px;   
    width:350px;
    float:left;
    }

#nav ul {
    list-style:none;
    background-color: #f1f1f1;
    width:300px;
    margin: 0;
    padding: 0;
    }

#nav ul li {
    line-height:40px; 
    }

#nav ul li a {
    margin:4px;
    color:black;
    padding:4px;
    display:block;
    font-size:16px;
   
    text-decoration:none;
    }

#nav ul li a:hover {
    background-color: #111;
    color:white;
    }

#nav ul li .active {
    background-color: #4CAF50;
   color:white;
    }

table#tblUser1 td {
        padding:4px
    }
</style>

    
    <script type="text/javascript">

        $(document).ready(function () {
            var link = $('#createuser');
            CreateUserForm(link);

           
        })

        function SetActive(obj)
        {
            $("#nav ul li a").each(function () {
                if ($(this).attr("id") == $(obj).attr("id"))
                    $(this).addClass("active");
                else
                    $(this).removeClass("active");
            })
        }

        function CreateUserForm(obj)
        {
            $('#divprotocol').hide();

            $.get("../Service/SSPService.asmx/UserTypes", function (data) {

                var $xml = $(data);
                myTypes = $xml.find("string").text();
                jsonUserTypes = jQuery.parseJSON(myTypes);

                $('#lstUserTypes').empty();

                $('#lstUserTypes').append(
                        $('<option></option>').val("").html("")
                    );

                $.each(jsonUserTypes, function (val, text) {
                    $('#lstUserTypes').append(
                        $('<option></option>').val(val).html(text)
                    );
                });


            }); //get itemtype

            $.get("../Service/SSPService.asmx/QualificationTypes", function (data) {

                var $xml = $(data);
                myTypes = $xml.find("string").text();
                jsonQualTypes = jQuery.parseJSON(myTypes);

                $('#lstQualification').empty();
                $('#lstQualification').append(
                        $('<option></option>').val("").html("")
                    );

                $.each(jsonQualTypes, function (val, text) {
                    $('#lstQualification').append(
                        $('<option></option>').val(val).html(text)
                    );
                });


                SetActive(obj);
                $('#divuser').show();
            });

            
        }
        function CreateAffiliation(obj) {
            SetActive(obj);
            $('#divuser').hide();
            //$('#AdminContainer').html('<h2>Create Affiliation</h2>');
        }
        function CreateAuthorRole(obj) {
            SetActive(obj);
            $('#divuser').hide();
            //$('#AdminContainer').html('<h2>Create Author Role</h2>');
        }
        function CreateRelease(obj) {
            SetActive(obj);
            $('#divuser').hide();
            //$('#AdminContainer').html('<h2>Create Release</h2>');
        }
        function CreateProtocolGroup(obj) {
            SetActive(obj);
            $('#divuser').hide();
            //$('#AdminContainer').html('<h2>Create Protocol Group</h2>');
        }
        function CreateProtocol(obj) {
            SetActive(obj);
            $('#divuser').hide();
            $('#divprotocol').show();
            //$('#AdminContainer').html('<h2>Create Protocol</h2>');
        }
        function CreateProtocolRelease(obj) {
            SetActive(obj);
            $('#divuser').hide();
            //$('#AdminContainer').html('<h2>Create Protocol Release</h2>');
        }
       
    </script>
  <menu id="nav" >
      <ul>
         <%-- <li><a class="active" href="#home">Home</a></li>--%>
          <li><a onclick="CreateUserForm(this)" id="createuser" href="#">Create User</a></li>
          <li><a onclick="CreateAffiliation(this)" id="createaffiliation" href="#"">Create Affiliation</a></li>
          <li><a onclick="CreateAuthorRole(this)" id="createauthorrole" href="#">Create Author Role</a></li>
          <li><a onclick="CreateRelease(this)" id="createrelease" href="#">Create Release</a></li>
          <li><a onclick="CreateProtocolGroup(this)" id="createprotocolgroup" href="#">Create Protocol Group</a></li>
          <li><a onclick="CreateProtocol(this)" id="createprotocol" href="#">Create Protocol</a></li>
          <li><a onclick="CreateProtocolRelease(this)" id="createprotocolrelease" href="#">Create Protocol Release</a></li>
     </ul>
  </menu> 

<div id="AdminContainer" style="padding:1px 16px;float:left">
    <!--<input type="hidden" id="hdnUserCKey" />-->
    <div id="divuser" style="display:none;margin-top:20px">
        
        <ssp:usr id="userForm" runat="server" />
    </div>
  <div id="divprotocol" style="display:none;margin-top:20px">
      <ssp:protocol ID="protocolForm" runat="server" />
  </div>
</div>
 


    <%--<div id="content" style="overflow:auto;">  Admin functions here</div>--%>
</asp:Content>
