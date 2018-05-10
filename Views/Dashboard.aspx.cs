using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SSPWebUI.Data;
using System.Data;


/*
 * truncate table SSP_ProtocolEditStatus

  insert into SSP_ProtocolEditStatus (Namespace, ProtocolVersionCKey, ProtocolAuthorCKey, AuthorCKey, Status)
  select 100004300 as Namespace, protocolversionckey, a.ProtocolAuthorCKey, AuthorCKey, 'N' as status 
  from ssp_protocolauthors a join SSP_ProtocolAuthorRole b on a.ProtocolAuthorCKey=b.ProtocolAuthorCKey
  where (RoleCKey=1.100004300 or roleckey=2.100004300)
 * */
namespace SSPWebUI.Views
{
    public partial class MainDashboard : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {

            System.Web.UI.HtmlControls.HtmlGenericControl crumbs =
                    (System.Web.UI.HtmlControls.HtmlGenericControl)Master.FindControl("breadcrumbs");
            crumbs.InnerHtml = "<span style='font-weight:bold;font-size:14px'>Dashboard >></span>";

                   

            if(!Page.IsPostBack)
            {
                LoadProtocolGroups();
                if (Session["group"] != null)
                    ddlProtocolGroups.SelectedValue = Session["group"].ToString();

                grdDashboard.PageSize = 50;
                BindData("ProtocolName", false);
            }

            grdDashboard.PageSize = 50;

          
            
        }

        protected void LoadProtocolGroups()
        {
            ddlProtocolGroups.DataSource = SSPWebUI.Data.ProtocolsData.GetProtocolGroups();
            ddlProtocolGroups.DataTextField = "ProtocolGroup";
            ddlProtocolGroups.DataValueField = "ProtocolGroupCKey";
            ddlProtocolGroups.DataBind();
            ddlProtocolGroups.SelectedIndex = 0;

        }

        protected void BindData(string OrderField, bool Desc)
        {
            string userid = (Page.Master.FindControl("hdnUserCKey") as HiddenField).Value;

            decimal userckey = ((SSPUser)Session["user"]).CKey;
            List<Dashboard> items;

            if (Session["group"] == null || Session["group"].ToString() == "0")
                items = Dashboard.DashboardData;
            else
                items = Dashboard.GetDashboardDataByProtocolGroup(decimal.Parse(Session["group"].ToString()));

            if (Session["group"] == null || Session["group"].ToString() == "0")
            {
                switch (OrderField)
                {
                    case "ProtocolName":
                        if (!Desc)
                            grdDashboard.DataSource = items.OrderBy(o => o.ProtocolName).ToList();
                        else
                            grdDashboard.DataSource = items.OrderByDescending(o => o.ProtocolName).ToList();
                        break;

                    case "ProtocolGroup":
                        if (!Desc)
                            grdDashboard.DataSource = items.OrderBy(o => o.ProtocolGroup).ToList();
                        else
                            grdDashboard.DataSource = items.OrderByDescending(o => o.ProtocolGroup).ToList();
                        break;
                    case "WebPostingDate":
                        if (!Desc)
                            grdDashboard.DataSource = items.OrderBy(o => o.WebpostingDate).ToList();
                        else
                            grdDashboard.DataSource = items.OrderByDescending(o => o.WebpostingDate).ToList();
                        break;
                }

            }
            else
            {
                switch (OrderField)
                {
                    case "ProtocolName":
                        if (!Desc)
                            grdDashboard.DataSource = items.OrderBy(o => o.ProtocolName).ToList();
                        else
                            grdDashboard.DataSource = items.OrderByDescending(o => o.ProtocolName).ToList();
                        break;

                    case "ProtocolGroup":
                        if (!Desc)
                            grdDashboard.DataSource = items.OrderBy(o => o.ProtocolGroup).ToList();
                        else
                            grdDashboard.DataSource = items.OrderByDescending(o => o.ProtocolGroup).ToList();
                        break;
                    case "WebPostingDate":
                        if (!Desc)
                            grdDashboard.DataSource = items.OrderBy(o => o.WebpostingDate).ToList();
                        else
                            grdDashboard.DataSource = items.OrderByDescending(o => o.WebpostingDate).ToList();
                        break;
                }
                //if(!Desc)
                //    grdDashboard.DataSource = Dashboard.GetDashboardData(userckey,
                //        decimal.Parse(Session["group"].ToString())).OrderBy(o => OrderField).ToList();
                //else
                //    grdDashboard.DataSource = Dashboard.GetDashboardData(userckey,
                //        decimal.Parse(Session["group"].ToString())).OrderByDescending(o => OrderField).ToList();
            }

            //grdDashboard.DataSource = items;
            grdDashboard.DataBind();



        }
        protected void lstStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void grdDashboard_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
               

                
                

                 string protocolversionckey = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "ProtocolVersionCKey"));
                if(protocolversionckey!="")
                {
                    DropDownList ddlReviewStatus = (DropDownList)e.Row.FindControl("ddlReviewStatus");

                    DropDownList ddlVersions = (DropDownList)e.Row.FindControl("ddlVersions");
                    DataTable dt = ProtocolsData.GetDraftVersions(decimal.Parse(protocolversionckey));
                    ddlVersions.DataSource = dt;
                    ddlVersions.DataTextField = "DraftVersion";
                    ddlVersions.DataValueField = "DraftVersion";
                    ddlVersions.DataBind();
                    ddlVersions.SelectedIndex = 0;

                     string DraftVersion = ddlVersions.SelectedValue.ToString();

                    List<ReviewStatus> reviewstatuses = new List<ReviewStatus>();
                    if(DraftVersion!="")
                         reviewstatuses = new Workflow().GetWorkflowProtocolReviewStatus(decimal.Parse(protocolversionckey), decimal.Parse(ddlVersions.SelectedValue.ToString()));
                    //ddlReviewStatus.ForeColor = System.Drawing.Color.Black;
                    List<Author> authors = new Author().getAuthors(decimal.Parse(protocolversionckey), 0, 999);  //get the work version
                    

                    if (reviewstatuses.Count > 0)
                    {
                        foreach (Author reviewer in authors)
                        {
                            ReviewStatus status = reviewstatuses.Where(x => x.ReviewerCKey == reviewer.AuthorCKey && reviewer.RoleCKey == (decimal)3.100004300).FirstOrDefault();
                            if (status != null)
                            {
                                if (status.Status != "3.100004300")
                                    reviewer.Name = reviewer.Name + " - Pending";
                                else
                                    reviewer.Name = reviewer.Name + " - Done";
                            }
                            else
                            {
                                //must have been addeded after draft was released

                                //reviewer.Name = reviewer.Name + " - Pending";
                                //new Workflow().AddProtocolReviewer(decimal.Parse(protocolversionckey), ((SSPUser)Session["user"]).CKey);
                                //if ((int)reviewer.RoleCKey == 3)
                                //{
                                //    reviewer.Name = reviewer.Name + " - Pending";
                                //}

                            }
                        }
                    }
                    else
                    {

                    }

                    
                    var reviewers =authors.Where(x => x.RoleCKey == (decimal)3.100004300).OrderBy(x=>x.Name);

                    ddlReviewStatus.DataSource = reviewers;
                    ddlReviewStatus.DataTextField = "Name";
                    ddlReviewStatus.DataValueField = "AuthorCKey";
                    ddlReviewStatus.DataBind();


                    DropDownList ddlEditStatus = (DropDownList)e.Row.FindControl("ddlEditStatus");
                    List<ProtocolStatus> editstatuses=new List<ProtocolStatus>();
                    if(DraftVersion!="")
                        editstatuses= new Workflow().GetWorkflowProtocolEditStatus(decimal.Parse(protocolversionckey), decimal.Parse(ddlVersions.SelectedValue.ToString()));


                    if (editstatuses.Count > 0)
                    {
                        foreach (Author editor in authors)
                        {
                            ProtocolStatus status = editstatuses.Where((x => x.EditorCKey == editor.AuthorCKey && ((int)editor.RoleCKey == 1 || (int)editor.RoleCKey == 2))).FirstOrDefault();
                            //ProtocolStatus status = editstatuses.Where(x => x.EditorCKey == editor.AuthorCKey).FirstOrDefault();
                            if (status != null)
                            {
                                if (status.Status != "4.100004300")
                                {

                                    editor.Name = editor.Name + " - Pending review";
                                }
                                else
                                {
                                    editor.Name = editor.Name + " - Done";
                                }
                            }

                            else  //author has not submitted draft yet
                            {
                                // if ((int)editor.RoleCKey == 1 || (int)editor.RoleCKey == 2)
                                //     editor.Name = editor.Name + " - Pending";
                                //new Workflow().AddProtocolAuthor(decimal.Parse(protocolversionckey), ((SSPUser)Session["user"]).CKey);
                            }

                        }
                    }
                    else
                    {
                        /*
                        foreach (Author editor in authors)
                        {
                            editor.Name = editor.Name + " - Pending";
                        }*/
                    }

                    var editors = authors.Where(x => x.RoleCKey == (decimal)1.100004300 || x.RoleCKey == (decimal)2.100004300).OrderBy(x => x.Name);
                    ddlEditStatus.DataSource = editors;
                    ddlEditStatus.DataTextField = "Name";
                    ddlEditStatus.DataValueField = "AuthorCKey";
                    ddlEditStatus.DataBind();


                    DropDownList ddleCCStatus = (DropDownList)e.Row.FindControl("ddleCCStatus");
                    DropDownList ddlPublicationStatus = (DropDownList)e.Row.FindControl("ddlPublicationStatus");
                    ddleCCStatus.Items.Add("Not Available");
                    ddlPublicationStatus.Items.Add("Not Available");

                }
            }
        }

        protected void grdDashboard_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //grdDashboard.EditIndex = e.NewEditIndex;
            //BindData();

        }

        protected void grdDashboard_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            //grdDashboard.EditIndex = -1;
            //BindData();
        }

        protected void grdDashboard_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }

        protected void grdDashboard_RowCommand(object sender, GridViewCommandEventArgs e)
        {
     
        }

        protected void grdDashboard_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdDashboard.PageIndex = e.NewPageIndex;
            BindData("ProtocolName", false);
        }

        protected void grdDashboard_Sorting(object sender, GridViewSortEventArgs e)
        {
            switch (e.SortExpression)
            {
                case "ProtocolName":
                    
                        if (e.SortDirection == SortDirection.Ascending)
                        {
                            BindData(e.SortExpression, false);
                            //grdDashboard.DataSource = Dashboard.DashboardData.OrderBy(o => o.ProtocolName).ToList();
                            //grdDashboard.DataBind();
                        }
                        else
                        {
                            BindData(e.SortExpression, true);
                            //grdDashboard.DataSource = Dashboard.DashboardData.OrderByDescending(o => o.ProtocolName).ToList();
                            //grdDashboard.DataBind();
                        }
                        break;


                case "ProtocolGroup":

                        if (e.SortDirection == SortDirection.Ascending)
                        {
                            BindData(e.SortExpression, false);
                            //grdDashboard.DataSource = Dashboard.DashboardData.OrderBy(o => o.ProtocolName).ToList();
                            //grdDashboard.DataBind();
                        }
                        else
                        {
                            BindData(e.SortExpression, true);
                            //grdDashboard.DataSource = Dashboard.DashboardData.OrderByDescending(o => o.ProtocolName).ToList();
                            //grdDashboard.DataBind();
                        }
                        break;
                        //if (e.SortDirection == SortDirection.Ascending)
                        //{

                        //    grdDashboard.DataSource = Dashboard.DashboardData.OrderBy(o => o.ProtocolGroup).ToList();
                        //    grdDashboard.DataBind();
                        //}
                        //else
                        //{
                        //    grdDashboard.DataSource = Dashboard.DashboardData.OrderByDescending(o => o.ProtocolGroup).ToList();
                        //    grdDashboard.DataBind();
                        //}
                        //break;
                    
               
                case "WebPostingDate":
                    {
                        if (e.SortDirection == SortDirection.Ascending)
                        {
                            BindData(e.SortExpression, false);
                            //grdDashboard.DataSource = Dashboard.DashboardData.OrderBy(o => o.ProtocolName).ToList();
                            //grdDashboard.DataBind();
                        }
                        else
                        {
                            BindData(e.SortExpression, true);
                            //grdDashboard.DataSource = Dashboard.DashboardData.OrderByDescending(o => o.ProtocolName).ToList();
                            //grdDashboard.DataBind();
                        }
                    }
                    break;
            }
        }

        protected void ddlProtocolGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session.Add("group", ddlProtocolGroups.SelectedValue);
            BindData("ProtocolName", false);
           
        }

        protected void ddlVersions_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlVersions = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlVersions.Parent.Parent;

            Label lbl = (Label)row.FindControl("lblProtocoCKey");
            string protocolversionckey = lbl.Text; // Convert.ToString(DataBinder.Eval(row.DataItem, "ProtocolVersionCKey"));
            List<Author> authors = new Author().getAuthors(decimal.Parse(protocolversionckey), 0, 999);  //get the work version
            List<ReviewStatus> reviewstatuses = new Workflow().GetWorkflowProtocolReviewStatus(decimal.Parse(protocolversionckey), decimal.Parse(ddlVersions.SelectedValue.ToString()));


            if (reviewstatuses.Count > 0)
            {
                foreach (Author reviewer in authors)
                {
                    ReviewStatus status = reviewstatuses.Where(x => x.ReviewerCKey == reviewer.AuthorCKey && reviewer.RoleCKey == (decimal)3.100004300).FirstOrDefault();
                    if (status != null)
                    {
                        if (status.Status != "3.100004300")
                            reviewer.Name = reviewer.Name + " - Pending";
                        else
                            reviewer.Name = reviewer.Name + " - Done";
                    }
                    else
                    {
                        //must have been addeded after draft was released

                        //reviewer.Name = reviewer.Name + " - Pending";
                        //new Workflow().AddProtocolReviewer(decimal.Parse(protocolversionckey), ((SSPUser)Session["user"]).CKey);
                        //if ((int)reviewer.RoleCKey == 3)
                        //{
                        //    reviewer.Name = reviewer.Name + " - Pending";
                        //}

                    }
                }
            }
            else
            {

            }



            DropDownList ddlReviewStatus = (DropDownList)row.FindControl("ddlReviewStatus");
            var reviewers = authors.Where(x => x.RoleCKey == (decimal)3.100004300).OrderBy(x => x.Name);

            ddlReviewStatus.DataSource = reviewers;
            ddlReviewStatus.DataTextField = "Name";
            ddlReviewStatus.DataValueField = "AuthorCKey";
            ddlReviewStatus.DataBind();

            DropDownList ddlEditStatus = (DropDownList)row.FindControl("ddlEditStatus");
            List<ProtocolStatus> editstatuses = new Workflow().GetWorkflowProtocolEditStatus(decimal.Parse(protocolversionckey), decimal.Parse(ddlVersions.SelectedValue.ToString()));


            if (editstatuses.Count > 0)
            {
                foreach (Author editor in authors)
                {
                    ProtocolStatus status = editstatuses.Where((x => x.EditorCKey == editor.AuthorCKey && ((int)editor.RoleCKey == 1 || (int)editor.RoleCKey == 2))).FirstOrDefault();
                    //ProtocolStatus status = editstatuses.Where(x => x.EditorCKey == editor.AuthorCKey).FirstOrDefault();
                    if (status != null)
                    {
                        if (status.Status != "4.100004300")
                        {

                            editor.Name = editor.Name + " - Pending review";
                        }
                        else
                        {
                            editor.Name = editor.Name + " - Done";
                        }
                    }

                    else  //author has not submitted draft yet
                    {
                        // if ((int)editor.RoleCKey == 1 || (int)editor.RoleCKey == 2)
                        //     editor.Name = editor.Name + " - Pending";
                        //new Workflow().AddProtocolAuthor(decimal.Parse(protocolversionckey), ((SSPUser)Session["user"]).CKey);
                    }

                }
            }
            else
            {
                /*
                foreach (Author editor in authors)
                {
                    editor.Name = editor.Name + " - Pending";
                }*/
            }


            var editors = authors.Where(x => x.RoleCKey == (decimal)1.100004300 || x.RoleCKey == (decimal)2.100004300 || x.RoleCKey == (decimal)9.100004300).OrderBy(x => x.Name);
            ddlEditStatus.DataSource = editors;
            ddlEditStatus.DataTextField = "Name";
            ddlEditStatus.DataValueField = "AuthorCKey";
            ddlEditStatus.DataBind();

        }
    }
}