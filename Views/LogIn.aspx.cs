using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SSPWebUI.Data;
using System.Web.Security;


namespace SSPWebUI.Views
{
    public partial class NewLogIn : System.Web.UI.Page
    {

        /*
         *         select e.protocolversionckey, protocolname, b.protocolauthorckey, a.authorckey, b.RoleCKey, RoleKey, Role, userid 
					from SSP_ProtocolAuthors a
                    join SSP_ProtocolAuthorRole b on a.ProtocolAuthorCKey=b.ProtocolAuthorCKey
                    join ssp_listofroles c on b.RoleCKey=c.RoleCkey 
					join ssp_user d on a.AuthorCKey=d.UserCkey	
					join SSP_ProtocolVersions e on a.ProtocolVersionCKey=e.ProtocolVersionCkey
					join SSP_Protocols f on e.ProtocolCkey=f.ProtocolCkey	
					order by protocolname, role 
         * */
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Add("user", null);
            if (Request.Params["signout"] == "true")
            {
                Session.Clear();
                Session.Abandon();
                FormsAuthentication.SignOut();
                Context.User = null;  //unless set to null log in status control does not change the status
                Response.Redirect("login.aspx",true);  //clear querystring parameter
            }
            if (!Page.IsPostBack)
            {
                Login1.DisplayRememberMe = false;
                
            }
        }

        protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
        {
            //validate user
            SSPUser user = new SSPUser();
            user.LogIn(Login1.UserName, Login1.Password);

            if (!user.Role.Contains("-1"))
            {
                e.Authenticated = true;
                Session["user"] = user;
                Session.Add("userckey", user.CKey);
                
                HttpCookie myCookie = new HttpCookie("UserSettings");
                myCookie["UserName"] = user.FirstName + " " + user.LastName;
                myCookie["UserCKey"] = user.CKey.ToString();
                myCookie.Expires = DateTime.Now.AddDays(365d);

                Response.Cookies.Add(myCookie);
                
                //moved from global
                SSPWebUI.Data.Dashboard.LoadDashboardData();
                Login1.DestinationPageUrl = "ProtocolEditor.aspx";
                
            }
            
        }

        protected void Login_Click(object sender, EventArgs e)
        {

        }
    }
}