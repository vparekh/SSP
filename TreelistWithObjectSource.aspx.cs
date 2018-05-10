using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web.ASPxTreeList;
using DevExpress.Web.Data;
namespace SSPWebUI
{
    public partial class TreelistWithObjectSource : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                treeList.DataBind();
                treeList.ExpandAll();
                treeList.StartEdit("2");
            }
            treeList.SettingsEditing.Mode = TreeListEditMode.EditForm;
            treeList.SettingsEditing.AllowNodeDragDrop = true;

        }
        protected void treeList_NodeUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            e.NewValues["VisibleText"] = e.NewValues["VisibleText"] + " sfjshdfjs";
        }
        protected void treeList_NodeInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            e.NewValues["VisibleText"] = "sfjsdhfjsd";
        }
    }
}