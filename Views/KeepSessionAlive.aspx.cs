using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SSPWebUI.Views
{
    public partial class KeepSessionAlive : System.Web.UI.Page
    {
        protected string WindowStatusText = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if(User.Identity.IsAuthenticated)
            {
                MetaRefresh.Attributes["content"] = Convert.ToString((Session.Timeout * 60) - 60) + ";url=KeepSessionAlive.aspx?q=" + DateTime.Now.Ticks;
                WindowStatusText = "Last refresh " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            }
        }
    }
}