<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogIn.aspx.cs" Inherits="SSPWebUI.Views.NewLogIn" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" >

<head id="Head1" runat="server">
        <%--<meta http-equiv="X-UA-Compatible" content="IE=edge" />--%>
        <title>Login</title>
        <meta http-equiv="content-type" content="text/html;charset=utf-8" />   
        <link type="text/css" rel="stylesheet" href="../Content/core/core.css" />
        <link type="text/css" rel="stylesheet" href="../content/core.css"/>
        <link type="text/css" rel="stylesheet" href="../content/nav.css"/>

        <link rel="stylesheet" href="../content/menu.css"/>

         <style type="text/css">              
            h1{
                text-align:center;
            } 
            .bg { 
                /* The image used */
                background-image: url("../images/dotsbg.jpg");

                /* Full height */
                height: 100%; 

                /* Center and scale the image nicely */
                background-position: center;
                background-repeat: no-repeat;
                background-size: cover;
            } 
            div.center {
                width: 300px;
                height: 220px;
                margin-top:20px;
                background-color: transparent;
                /*position: absolute;*/
                top:0;
                bottom: 0;
                left: 0;
                right: 0;
                /*margin: auto;*/
                margin-right:auto;
                margin-left:auto;
            }
        </style>
        <script type="text/javascript">
            function clearform()
            {
                document.getElementById("UserName").value = "";
                document.getElementById("Password").value = "";
            }
            function validate() {
                var userName = document.getElementById("UserNameRequired");
                var password = document.getElementById("PasswordRequired");

                if (username == null || username == "") {
                    alert("Please enter the username.");
                    return false;
                }
                if (password == null || password == "") {
                    alert("Please enter the password.");
                    return false;
                }
            }
            </script>
    </head>

   

    <body class="bg">
        <form id="form1" runat="server">
          
        <div style="width:100%" id="container">
          
            <div  style="width:75%;margin:50px auto">
                <div id="masthead" class="clearfix">
                <div id="branding">
                    <img alt="CAP Logo" src="../Images/CAPLogo.PNG" />
                </div>
            </div>
            <div  >
                <div>
                    <div id="rbox">                        
                        <h1>Welcome to the Cancer Reporting Tools – Single Source Product Webpage.</h1>
                        <div class="center">
                        <asp:Login id="Login1" runat="server" OnAuthenticate="Login1_Authenticate">
                            <LayoutTemplate>
                                <table>
                                    <tr>
                                        <td style="background-color:navy; color:white; text-align:center" >
                                            <b>Login</b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:center">
                                            Please enter your user name and password to log in.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        User name:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ClientIDMode="Static" id="UserName" runat="server"></asp:TextBox>
                                                        <asp:requiredfieldvalidator id="UserNameRequired" runat="server" value="Enter User Name" ControlToValidate="UserName" Text="*"></asp:requiredfieldvalidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Password:</td>
                                                    <td>
                                                        <asp:TextBox ClientIDMode="Static" id="Password" runat="server" textMode="Password"></asp:TextBox>
                                                        <asp:requiredfieldvalidator id="PasswordRequired" runat="server" value="Enter Password" ControlToValidate="Password" Text="*"></asp:requiredfieldvalidator>
                                                    </td>
                                                </tr>                                    
                                            </table>
                                        </td>                            
                                    </tr>
                                    <tr>
                                        <td style="text-align:center">
                                            <asp:button id="Login" CommandName="Login" runat="server" Text="Login" ></asp:button>                                            
                                            <input  type="button" value="Clear" onclick="clearform()"  />
										</td>
                                    </tr>
                                    <tr>
                                        <td ><asp:Literal id="FailureText" runat="server"></asp:Literal></td>
                                    </tr>
                                </table>

                                 <div id="forgotPassword" style="text-align:center">
                                               
                                     <br> 
                                     <br>                   
                                 <b> Forgot your password? </b> <br>
					                Contact CAP eCC at <i><b><a href="mailto:capecc@cap.org"> capecc@cap.org </a></b></i> <br>
                                     OR Call @<i><b>847-832-7700</b></i>. 
                                
                                
                                  <br>
                                </div>

                            </LayoutTemplate>
                        </asp:Login>                            
                  
                    </div> <!-- End of rbox400 -->

                </div>
            </div>
            
            </div>
        </div>
    </div>
           
            
        </form>
    </body>
</html>
