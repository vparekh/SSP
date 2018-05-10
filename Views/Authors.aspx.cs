using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SSPWebUI.Data;
namespace SSPWebUI.Views
{
    public partial class Authors : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.ProtocolList.SelectedIndexChanged += ProtocolList_SelectedIndexChanged;
            if (!Page.IsPostBack)
            {
                //user came here from another page
                txtProtocolVersion.Text = Session["CurrentProtocol"].ToString();
            }
            else
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
                txtProtocolVersion.Text = Master.ProtocolList.SelectedValue; //post back
                

            }

            if(!Request.IsAuthenticated)
            {
                grdAuthors.Columns[2].Visible = false;
                grdAuthors.Columns[3].Visible = false;
            }
            LoadAuthors();
            LoadRoles();
            BindAuthors();
        }

        void ProtocolList_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtProtocolVersion.Text = ((DropDownList)sender).SelectedValue;
            Session.Add("CurrentProtocol", txtProtocolVersion.Text);
        }
        protected void grdAuthors_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {


        }
        protected void grdAuthors_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        protected void grdAuthors_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //hfTab.Value = "authors";
            grdAuthors.EditIndex = e.NewEditIndex;
            BindAuthors();

        }
        protected void grdAuthors_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

        }

        protected void lnkRemove_Click(object sender, EventArgs e)
        {

        }

        protected void lbAdd_Click(object sender, EventArgs e)
        {
            string authorckey = lstAddAuthors.SelectedValue;
            string role = lstAddRoles.SelectedValue;
            decimal decauthorckey = 0;
            decimal.TryParse(authorckey, out decauthorckey);
            if (decauthorckey == 0) return;

            decimal decroleckey = 0;
            decimal.TryParse(role, out decroleckey);
            if (decroleckey == 0) return;

            Author author = new Author();
            //author.addAuthor(decimal.Parse(authorckey), decimal.Parse(role), decimal.Parse(txtProtocolVersion.Text));
            BindAuthors();
            lstAddAuthors.SelectedIndex = 0;
            lstAddRoles.SelectedIndex = 0;
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            
        }

        protected void grdAuthors_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToLower() == "delete")
            {
                // get the categoryID of the clicked row
                string test = e.CommandArgument.ToString();
                string authorckey = test.Split('-')[0];
                string roleckey = test.Split('-')[1];
                Author author = new Author();

                author.deleteAuthor(decimal.Parse(authorckey), decimal.Parse(txtProtocolVersion.Text), decimal.Parse(roleckey));
               // hfTab.Value = "authors";
                BindAuthors();
            }
            else if (e.CommandName.ToLower() == "add")
            {
                Control control = null;
                if (grdAuthors.FooterRow != null)
                {
                    control = grdAuthors.FooterRow;

                }
                else
                {
                    control = grdAuthors.Controls[0].Controls[0];

                }
                //hfTab.Value = "authors";
                string authorckey = ((DropDownList)(control.FindControl("lstAuthors"))).SelectedValue;
                string role = ((DropDownList)(control.FindControl("lstRoles"))).SelectedValue;

                Author author = new Author();
               // author.addAuthor(decimal.Parse(authorckey), decimal.Parse(role), decimal.Parse(txtProtocolVersion.Text));
                BindAuthors();
            }
            else if (e.CommandName.ToLower() == "update")
            {

                //write the update command here
                string test = e.CommandArgument.ToString();
                string authorckey = test.Split('-')[0];
                string roleckey = test.Split('-')[1];
                Author author = new Author();

                author.deleteAuthor(decimal.Parse(authorckey), decimal.Parse(txtProtocolVersion.Text), decimal.Parse(roleckey));

                LinkButton btn = e.CommandSource as LinkButton;
                GridViewRow row = btn.NamingContainer as GridViewRow;
                if (row == null)
                {
                    return;
                }
                DropDownList lstAuthors = row.FindControl("lstAuthors") as DropDownList;
                DropDownList lstRoles = row.FindControl("lstRoles") as DropDownList;

                authorckey = lstAuthors.SelectedValue;
                roleckey = lstRoles.SelectedValue;
                // hfTab.Value = "authors";
                //author.addAuthor(decimal.Parse(authorckey), decimal.Parse(roleckey), decimal.Parse(txtProtocolVersion.Text));
                grdAuthors.EditIndex = -1;
                BindAuthors();

            }
            else if (e.CommandName.ToLower() == "cancel")
            {
                grdAuthors.EditIndex = -1;
                //hfTab.Value = "authors";
                BindAuthors();
            }

        }
        protected void grdAuthors_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ////Find the DropDownList in the Row
                //DropDownList lstAuthors = (e.Row.FindControl("lstAuthors") as DropDownList);

                //lstAuthors.DataSource = SSPUser.getAll(); //new Author().getAuthors(decimal.Parse(hdnProtocolVersionCKey.Value));
                //lstAuthors.DataTextField = "Name";
                //lstAuthors.DataValueField = "CKey";
                //lstAuthors.DataBind();
                //lstAuthors.Items.Insert(0, new ListItem("Please select"));
                ////lstAuthors.Attributes.Add("onchange", "setAuthor('" + lstAuthors.SelectedValue + "')");

                //DropDownList lstRoles = (e.Row.FindControl("lstRoles") as DropDownList);
                //lstRoles.DataSource = Role.getRoles(); //new Author().getAuthors(decimal.Parse(hdnProtocolVersionCKey.Value));
                //lstRoles.DataTextField = "RoleName";
                //lstRoles.DataValueField = "RoleCKey";
                //lstRoles.DataBind();
                //lstRoles.Items.Insert(0, new ListItem("Please select"));
                //lstRoles.Attributes.Add("onchange", "setRole('" + lstRoles.SelectedValue +"')");

                //lstAuthors.Items.FindByValue(country).Selected = true;
            }
            else if (e.Row.RowType == DataControlRowType.EmptyDataRow)
            {
                DropDownList lstAuthors = (e.Row.FindControl("lstAuthors") as DropDownList);
                lstAuthors.DataSource = SSPUser.getAllAuthors(); //new Author().getAuthors(decimal.Parse(hdnProtocolVersionCKey.Value));
                lstAuthors.DataTextField = "Name";
                lstAuthors.DataValueField = "CKey";
                lstAuthors.DataBind();
                lstAuthors.Items.Insert(0, new ListItem("Please select"));

                DropDownList lstRoles = (e.Row.FindControl("lstRoles") as DropDownList);
                lstRoles.DataSource = Role.getRoles();
                lstRoles.DataTextField = "RoleName";
                lstRoles.DataValueField = "RoleCKey";
                lstRoles.DataBind();
                lstRoles.Items.Insert(0, new ListItem("Please select"));
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {

                LinkButton EditButton = (LinkButton)e.Row.FindControl("lnkEdit");
                LinkButton RemoveButton = (LinkButton)e.Row.FindControl("lnkRemove");
                Author dataRecord = (Author)e.Row.DataItem;
                if (EditButton != null && dataRecord != null)
                {
                    if (!Request.IsAuthenticated)
                    {
                        EditButton.Visible = false;
                        RemoveButton.Visible = false;
                    }
                }

                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    DropDownList lstAuthors = (e.Row.FindControl("lstAuthors") as DropDownList);
                    lstAuthors.DataSource = SSPUser.getAllAuthors();
                    lstAuthors.DataTextField = "Name";
                    lstAuthors.DataValueField = "CKey";
                    lstAuthors.DataBind();

                    DropDownList lstRoles = (e.Row.FindControl("lstRoles") as DropDownList);
                    lstRoles.DataSource = Role.getRoles();
                    lstRoles.DataTextField = "RoleName";
                    lstRoles.DataValueField = "RoleCKey";
                    lstRoles.DataBind();

                    Author author = e.Row.DataItem as Author;
                    lstAuthors.SelectedValue = author.AuthorCKey.ToString();
                    lstRoles.SelectedValue = author.RoleCKey.ToString();
                }
            }
        }

        protected void lstAuthors_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["author"] = (sender as DropDownList).SelectedValue;
        }

        protected void lstRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["role"] = (sender as DropDownList).SelectedValue;
        }

        public void BindAuthors()
        {
            Author author = new Author();
            grdAuthors.DataSource = null;
            List<Author> lstAuthors = author.getAuthors(decimal.Parse(txtProtocolVersion.Text));
            if (lstAuthors.Count > 0)
                grdAuthors.DataSource = lstAuthors;
            grdAuthors.DataBind();

        }

        public void LoadAuthors()
        {
            lstAddAuthors.DataSource = SSPUser.getAllAuthors();
            lstAddAuthors.DataTextField = "Name";
            lstAddAuthors.DataValueField = "CKey";
            lstAddAuthors.DataBind();
            lstAddAuthors.Items.Insert(0, new ListItem("Please select"));
        }

        public void LoadRoles()
        {
            lstAddRoles.DataSource = Role.getRoles();
            lstAddRoles.DataTextField = "RoleName";
            lstAddRoles.DataValueField = "RoleCKey";
            lstAddRoles.DataBind();
            lstAddRoles.Items.Insert(0, new ListItem("Please select"));
        }
    }
}