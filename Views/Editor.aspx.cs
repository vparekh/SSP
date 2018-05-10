using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using SSPWebUI.Data;
using DevExpress.Web.ASPxTreeList;
using DevExpress.Web.ASPxRichEdit;
using DevExpress.Web.Data;
using DevExpress.Web.Office;
using DevExpress.XtraRichEdit;
using DevExpress.Web;
using DevExpress.Office;

namespace SSPWebUI.Views
{
    public partial class Editor : System.Web.UI.Page
    {
         protected void Page_PreRender(object sender, EventArgs e)
        {
          

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            hdnUserCKey.Value = ((SSPUser)Session["user"]).CKey.ToString();
            if(Request.IsAuthenticated)
            {
                lblUserName.Text = ((SSPUser)Session["user"]).Name;

            }
            lblDate.Text = DateTime.Now.ToString();
            lblGreeting.Text = "Welcome to the CAP Cancer Reporting Tool - Single Source Product";

            if (!Page.IsPostBack)
            {
                PopulateAllTabs();
            }

            else
            {
                string test = hfTab.Value;
            }

            string role = ((SSPUser)Session["user"]).GetProtocolRole(decimal.Parse(Protocols.SelectedValue));
            if (role == "1")
            {                
                showViewComments.Visible = true;
                btnSaveAll.Visible = true;
                btnSubmit.Visible = true;
            }
            else if (role=="0")
            {                
                showViewComments.Visible = false;
                btnSaveAll.Visible = false;
                btnSubmit.Visible = false;
            }
            else
            {                
                showViewComments.Visible = true;
                btnSaveAll.Visible = false;
                btnSubmit.Visible = false;
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            Protocols.DataSource = SSPWebUI.Data.ProtocolsData.GetProtocols();
            Protocols.DataTextField = "ProtocolName";
            Protocols.DataValueField = "ProtocolCKey";
            Protocols.DataBind();
            Protocols.SelectedIndex = 1;
            Session["CurrentProtocol"] = Protocols.SelectedValue;

            SSPUser user =  (SSPUser)Session["user"];
            //set protocol role
            //user.Role = user.GetProtocolRole(decimal.Parse(Protocols.SelectedValue));

            //hidden field
            hdnUserCKey.Value = user.CKey.ToString();

        }

        protected void PopulateAllTabs()
        {
            //MyAuthors.CurrentProtocolVersion.Value = Protocols.SelectedValue;
            //MyAuthors.PopulateControl();
            //MyNote.CurrentProtocolVersion.Value = Protocols.SelectedValue;
            //MyNote.PopulateControl("A");
            //hdr.CurrentProtocolVersion.Value = Protocols.SelectedValue;
            //hdr.PopulateControl();
            //mySummary.CurrentProtocolVersion.Value = Protocols.SelectedValue;
            //mySummary.PopulateControl();
            //MyReference.CurrentProtocolVersion.Value = Protocols.SelectedValue;
            //MyReference.PopulateControl(1);
            //MyProcedure1.CurrentProtocolVersion.Value = Protocols.SelectedValue;
            //MyProcedure1.PopulateControl();
        }

        protected void Protocols_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateAllTabs();
            //((SSPUser)Session["user"]).Role=((SSPUser)Session["user"]).GetProtocolRole(decimal.Parse(Protocols.SelectedValue));

        }
    }
}