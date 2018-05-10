using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SSPWebUI.Views
{
    public partial class Admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Web.UI.HtmlControls.HtmlGenericControl crumbs =
                   (System.Web.UI.HtmlControls.HtmlGenericControl)Master.FindControl("breadcrumbs");
            crumbs.InnerHtml = "<span style='font-weight:bold;font-size:14px'>Admin >></span>";

            if(Page.IsPostBack)
            {
                //highlight the current tab
                if (ScriptManager.GetCurrent(this.Page).IsInAsyncPostBack)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "UpdateMsg",
                        @"$(document).ready(function(){ var tabID = '#' + $('#tabid').text();
                            $(tabID).addClass('current');});", true);

                }
                else
                {
                    // regular full page postback occured
                    // custom logic accordingly                
                }
            }
        }
    }
}