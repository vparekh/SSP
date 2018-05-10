using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SSPWebUI.Data;

using DevExpress.Web.ASPxTreeList;
using DevExpress.Web.ASPxRichEdit;
using DevExpress.Web.Data;
using DevExpress.Web.Office;
using DevExpress.XtraRichEdit;
using DevExpress.Web;
using DevExpress.Office;

using Newtonsoft.Json;
using System.Data;


namespace SSPWebUI.Views
{
    public partial class CaseSummary : System.Web.UI.Page
    {
       

        protected void Page_Init(object sender, EventArgs e)
        {
            txtProtocolVersion.Text = Session["CurrentProtocol"].ToString();
             Master.ProtocolList.SelectedIndexChanged += ProtocolList_SelectedIndexChanged;
             Master.ProtocolList.SelectedValue = txtProtocolVersion.Text; 
             allowtreeedit.Value = "true";
            if(!Page.IsPostBack)
            {
                LoadChecklists();
                //must bind before caling LoadChecklistItems
                BindChecklists();
                LoadChecklistItems();
                BindChecklistTree();
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
                BindChecklists();
                BindChecklistTree();
            }
            if (Request.IsAuthenticated)
            {
                treeList.SettingsEditing.Mode = TreeListEditMode.Inline;
                treeList.SettingsEditing.AllowNodeDragDrop = true;
            }
            else
            {
                treeList.SettingsEditing.AllowNodeDragDrop = false;
            }
            treeList.SettingsEditing.Mode = TreeListEditMode.Inline;
            treeList.SettingsEditing.AllowNodeDragDrop = true;
        }

        void ProtocolList_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtProtocolVersion.Text = ((DropDownList)sender).SelectedValue;
            Session.Add("CurrentProtocol", txtProtocolVersion.Text);
            LoadChecklists();
            BindChecklists();
            LoadChecklistItems();            
            BindChecklistTree();

        }

        public void LoadChecklists()
        {
            List<Checklist> checklists = ProtocolsData.GetChecklists(decimal.Parse(txtProtocolVersion.Text));
            Session.Add("checklists", checklists);            
        }

        public void BindChecklists()
        {
            ddChecklists.DataSource = Session["checklists"];
            ddChecklists.DataTextField = "VisibleText";
            ddChecklists.DataValueField = "ChecklistCKey";
            ddChecklists.DataBind();
        }

        public void LoadChecklistItems()
        {
            string checklistckey = ddChecklists.SelectedValue;
            List<ChecklistTemplateVersion> versions = ChecklistTemplateVersion.GetChecklistTemplateVersions(decimal.Parse(checklistckey));
            //item at the top is the most recent version
            DataTable checklisttable = ChecklistTemplateItems.GetAllChecklistItems(versions[0].ChecklistTemplateVersionCKey);
            checklisttable.Columns.Add("NotesAlt");


            //update notes
            foreach (DataRow drcheck in checklisttable.Rows)
            {
                string notes = drcheck["Notes"].ToString();
                drcheck["NotesAlt"] = notes;
                string[] note = notes.Split(';');
                notes = "";
                foreach (string test in note)
                {
                    if (test.Length > 0)
                    {
                        if (notes == "")
                            notes = "<a onclick=OnNoteClick('" + test.Substring(5, 1) + "')>" + test + "</a>";
                        else
                            notes = notes + ", <a onclick=OnNoteClick('" + test.Substring(5, 1) + "')>" + test + "</a>";
                    }
                }
                if (notes.Length > 0)
                {
                    drcheck["Notes"] = notes;
                }
                if (drcheck["Required"].ToString() == "1")
                {
                    drcheck["Required"] = "Yes";
                }
                else if (drcheck["Required"].ToString() == "0")
                {
                    drcheck["Required"] = "No";
                }
                else
                {
                    drcheck["Required"] = "Conditional";
                }

            }

            DataRow dr = checklisttable.NewRow();

            //node for the Trash icon
            dr["ParentItemCKey"] = 0;
            dr["ChecklistTemplateItemCkey"] = "*";
            dr["visibletext"] = "-Deleted-";
            checklisttable.Rows.Add(dr);

            //add deleted items
            DataTable dtDeleted = ChecklistTemplateItems.GetDeletedChecklistItems(versions[0].ChecklistTemplateVersionCKey);



            foreach (DataRow drDeleted in dtDeleted.Rows)
            {

                var desRow = checklisttable.NewRow();
                var sourceRow = drDeleted;
                string key = drDeleted["ChecklistTemplateItemCkey"].ToString();
              
                desRow.ItemArray = sourceRow.ItemArray.Clone() as object[];
                decimal test = ChecklistTemplateItems.GetFirstAncestor(decimal.Parse(key), true);


                string notes = drDeleted["Notes"].ToString();
                desRow["NotesAlt"] = notes;
                string[] note = notes.Split(';');
                notes = "";
                foreach (string anote in note)
                {
                    if (anote.Length > 0)
                    {
                        if (notes == "")
                            notes = "<a onclick=OnNoteClick('" + anote.Substring(5, 1) + "')>" + anote + "</a>";
                        else
                            notes = notes + ", <a onclick=OnNoteClick('" + anote.Substring(5, 1) + "')>" + anote + "</a>";
                    }
                }

                desRow["Notes"] = notes;

                if (desRow["Required"].ToString() == "1")
                {
                    desRow["Required"] = "Yes";
                }
                else if (desRow["Required"].ToString() == "0")
                {
                    desRow["Required"] = "No";
                }
                else
                {
                    desRow["Required"] = "Conditional";
                }

                if (sourceRow["ParentItemCKey"] == System.DBNull.Value || (test == decimal.Parse(key)))
                    desRow["ParentItemCKey"] = "*";
                else
                    desRow["ParentItemCkey"] = "*" + sourceRow["ParentItemCkey"];


                desRow["ChecklistTemplateItemCkey"] = "*" + sourceRow["ChecklistTemplateItemCkey"];

                
                checklisttable.Rows.Add(desRow);
            }
            Session.Add("checklisttable", checklisttable);
        }

        public void BindChecklistTree()
        {                   

            treeList.DataSource = Session["checklisttable"];

            treeList.DataBind();
        }

        protected void ddChecklists_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            BindChecklistTree();

        }

        protected void treeList_CustomDataCallback(object sender, TreeListCustomDataCallbackEventArgs e)
        {
            string[] arguments = e.Argument.Split('|');
            if (arguments[0] == "GET")
            {
                string field = arguments[1];
                string key = arguments[2];
                TreeListNode node = treeList.FindNodeByKeyValue(key);
                e.Result = node[field].ToString();

            }
        }
     
       

        protected void treeList_NodeUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {


            ASPxTextBox txt = (ASPxTextBox)treeList.FindEditCellTemplateControl((TreeListDataColumn)treeList.Columns["VisibleText"], "VisibleTextbox");

            ASPxTextBox ConditionText = (ASPxTextBox)treeList.FindEditCellTemplateControl((TreeListDataColumn)treeList.Columns["Condition"], "ConditionText");


            string ckey = e.Keys[0].ToString();
            string parentckey = treeList.FocusedNode.ParentNode.Key;
            string vistext = txt.Value.ToString();
            string condition = "";

            if (ConditionText == null)
                condition = "";
            else
                condition = ConditionText.Value == null ? "" : ConditionText.Value.ToString();

            DropDownList itemtypelist = (DropDownList)treeList.FindEditCellTemplateControl((TreeListDataColumn)treeList.Columns["ItemType"], "ItemTypeList");


            int type = int.Parse(itemtypelist.SelectedValue);
            DropDownList requiredlist = (DropDownList)treeList.FindEditCellTemplateControl((TreeListDataColumn)treeList.Columns["Required"], "RequiredList");


            string required = requiredlist.SelectedValue;
            int iRequired = 0;

            if (required == "Yes")
                iRequired = 1;
            else if (required == "No")
                iRequired = 0;
            else if (required == "Conditional")
                iRequired = 2;

            ASPxDropDownEdit edtInline = (ASPxDropDownEdit)treeList.FindEditCellTemplateControl((TreeListDataColumn)treeList.
                Columns["Notes"], "ASPxDropDownEdit1");
            string notes = edtInline.Text;

            bool isDirty = true;



            if (isDirty)
            {
                ChecklistTemplateItems.UpdateChecklistItem(
                   decimal.Parse(ckey),
                   vistext,
                   type,
                   iRequired,
                   decimal.Parse(txtProtocolVersion.Text),
                   notes, condition
               );
            }




            e.Cancel = true;
            LoadChecklists();
            BindChecklists();
            LoadChecklistItems();
            BindChecklistTree();
            treeList.CancelEdit();

        }

        protected void treeList_NodeInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            if (e.NewValues["VisibleText"] == null)
            {
                e.Cancel = true;
                treeList.CancelEdit();
                return;
            }


            string testval = e.NewValues["VisibleText"].ToString();
            ASPxDropDownEdit edtInline = (ASPxDropDownEdit)treeList.FindEditCellTemplateControl((TreeListDataColumn)treeList.
                    Columns["Notes"], "ASPxDropDownEdit1");
            testval = edtInline.Text;
            DropDownList itemtypelist = (DropDownList)treeList.FindEditCellTemplateControl((TreeListDataColumn)treeList.Columns["ItemType"], "ItemTypeList");
            if (itemtypelist.SelectedValue == "")
            {
                e.Cancel = true;
                treeList.CancelEdit();
                return;
            }

            int type = int.Parse(itemtypelist.SelectedValue);
            DropDownList requiredlist = (DropDownList)treeList.FindEditCellTemplateControl((TreeListDataColumn)treeList.Columns["Required"], "RequiredList");
            if (requiredlist.SelectedValue == "")
            {
                e.Cancel = true;
                treeList.CancelEdit();
                return;
            }


            bool required = bool.Parse(requiredlist.SelectedValue == "Yes" ? "True" : "False");
            e.Cancel = true;

            string parentKey = e.NewValues[treeList.ParentFieldName].ToString(); // treeList.FocusedNode.ParentNode.Key;

            string checklistckey = ddChecklists.SelectedValue;
            List<ChecklistTemplateVersion> versions = ChecklistTemplateVersion.GetChecklistTemplateVersions(decimal.Parse(checklistckey));
            decimal versionckey = versions[0].ChecklistTemplateVersionCKey;



            int sortorder = ChecklistTemplateItems.GetSortOrder(decimal.Parse(parentKey));
           
            BindChecklistTree();
            treeList.CancelEdit();

        }

        protected void treeList_ProcessDragNode(object sender, TreeListNodeDragEventArgs e)
        {
            bool isDragNodeExpanded = e.Node.Expanded;
            bool isDropNodeExpanded = e.NewParentNode.Expanded;

            if (e.NewParentNode.Key == "*")
            {
              
                    ChecklistTemplateItems.DeleteChecklistItem(decimal.Parse(e.Node.Key));  //soft delete    status = status | 4           
                
            }
            else
            {
                
                decimal parentckey = decimal.Parse(e.NewParentNode.Key.Replace("*", ""));
                decimal nodekey = decimal.Parse(e.Node.Key);              

                string checklistckey = ddChecklists.SelectedValue;
                List<ChecklistTemplateVersion> versions = ChecklistTemplateVersion.GetChecklistTemplateVersions(decimal.Parse(checklistckey));
                decimal versionckey = versions[0].ChecklistTemplateVersionCKey;

                ChecklistTemplateItems.DragDropItem(versionckey.ToString(), nodekey.ToString(), parentckey.ToString());

                if(e.Node.Key==e.NewParentNode.Key)
                {
                   
                }
                else
                {

                }
            }

            //e.Node.Expanded = isDragNodeExpanded;
            //e.NewParentNode.Expanded = false; //isDropNodeExpanded;
            
            e.Handled = true;

            LoadChecklists();
            BindChecklists();
            LoadChecklistItems();
            BindChecklistTree();

          
        }

        

        protected void treeList_CustomCallback(object sender, TreeListCustomCallbackEventArgs e)
        {
            string ckey = e.Argument;
            string[] arguments = e.Argument.Split('|');
            if (arguments[0] == "INSERT")
            {
                string vistext = arguments[1];
                string type = arguments[2];
                string required = arguments[3];
                string notes = arguments[4];
                string parent = arguments[5];
                string condition = arguments[6];
                string checklistckey = ddChecklists.SelectedValue;
                List<ChecklistTemplateVersion> versions = ChecklistTemplateVersion.GetChecklistTemplateVersions(decimal.Parse(checklistckey));
                decimal templateversionckey = versions[0].ChecklistTemplateVersionCKey;
                DropDownList Protocols = (DropDownList)Master.FindControl("ContentPlaceHolder_Menu").FindControl("protocols");
                decimal protocolversion = decimal.Parse(txtProtocolVersion.Text);
                int sortorder = 0;


                //current item is the parent of new item; sort order of new item is 1 larger than that of parent, so that it becomes the first child when ordered 

                if (parent == "")
                    sortorder = 9999999;  //large number so that it goes to the bottom
                else
                    sortorder = ChecklistTemplateItems.GetSortOrder(decimal.Parse(parent));

                if (parent == "")
                {
                    ChecklistTemplateItems.AddChecklistItem(templateversionckey, protocolversion, null, vistext, null, null,
                        null, null, int.Parse(type), int.Parse(required), condition, false, false, false, false, sortorder + 1, notes);
                }
                else
                {
                    ChecklistTemplateItems.AddChecklistItem(templateversionckey, protocolversion, decimal.Parse(parent), vistext, null, null,
                        null, null, int.Parse(type), int.Parse(required), condition, false, false, false, false, sortorder + 1, notes);
                }
                

            }
            else if (arguments[0] == "RESTORE")
            {
              string retval =  ChecklistTemplateItems.RestoreChecklistItem(decimal.Parse(arguments[1]));
               
            }
            else if (arguments[0] == "DELETE")
            {
                string key = arguments[1];
                if (ChecklistTemplateItems.CanDelete(decimal.Parse(key), 0))
                {
                    ChecklistTemplateItems.DeleteChecklistItemHard(decimal.Parse(key));

                }
            }
            else if (arguments[0] == "SETIMAGE")
            {
                TreeListNode node = treeList.FindNodeByKeyValue(arguments[1]);

            }

            LoadChecklists();
            BindChecklists();
            LoadChecklistItems();
            BindChecklistTree();
            
        }

        protected void treeList_NodeDeleting(object sender, ASPxDataDeletingEventArgs e)
        {

            if (ChecklistTemplateItems.CanDelete(decimal.Parse(e.Keys[0].ToString()), 0))
            {
                ChecklistTemplateItems.DeleteChecklistItemHard(decimal.Parse(e.Keys[0].ToString()));
                BindChecklistTree();
            }
            e.Cancel = true;
        }


        protected void treeList_HtmlDataCellPrepared(object sender, TreeListHtmlDataCellEventArgs e)
        {
          
            TreeListNode node = treeList.FindNodeByKeyValue(e.NodeKey);
            TreeListNode parentNode = node.ParentNode;
            bool canRestore = true;
            if (parentNode != null && parentNode["status"] !=null )
            {
                int pstatus = -1;
                int.TryParse(parentNode["status"].ToString(),out pstatus);

               if(pstatus>=4)
               {
                   canRestore = false;
               }
            }
            string status = node["status"].ToString();
            if (e.Column.FieldName == "VisibleText")
            {

                string fileName = string.Empty;
                ASPxImage imgTrash = null;
                ASPxImage imgRestore = null;
                ASPxImage imgNew = null;

                e.Cell.ToolTip = e.NodeKey;

                if (e.NodeKey == "*")  //place holder for the trash icon
                {
                    fileName = "~/Images/recycle1.png";
                    imgTrash = (ASPxImage)treeList.FindDataCellTemplateControl(e.NodeKey, e.Column, "Trash");
                    imgTrash.Visible = true;
                }
                else if (e.NodeKey.Contains("*") & e.NodeKey != "*")  //deleted items
                {
                    //make restore visible
                    if(canRestore)
                    {
                        imgRestore = (ASPxImage)treeList.FindDataCellTemplateControl(e.NodeKey, e.Column, "Restore");
                        imgRestore.Visible = true;    
                    }
                                

                }              
                else if (status=="1") //status =1 - new item
                {
                    imgNew = (ASPxImage)treeList.FindDataCellTemplateControl(e.NodeKey, e.Column, "New");
                    imgNew.Visible = true;
                  
                }

             

            }
        }

      

        protected bool IsAddImageVisible(object obj)
        {
            TreeListDataCellTemplateContainer container = (TreeListDataCellTemplateContainer)obj;
            if (container.NodeKey.Contains("*") | container.NodeKey == "*")
                return false;
            else
                return true;
        }

        protected bool IsRestoreImageVisible(object obj)
        {
            TreeListDataCellTemplateContainer container = (TreeListDataCellTemplateContainer)obj;
            if (container.NodeKey.Contains("*") & container.NodeKey != "*")
                return true;
            else
                return false;
        }

        protected bool IsDeleteImageVisible(object obj)
        {
            TreeListDataCellTemplateContainer container = (TreeListDataCellTemplateContainer)obj;
            string nodekey = container.NodeKey.Replace("*", "");
            if (nodekey.Length > 0)
            {
                if (ChecklistTemplateItems.CanDelete(decimal.Parse(nodekey), 0))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        protected bool IsTrashImageVisible(object obj)
        {
            TreeListDataCellTemplateContainer container = (TreeListDataCellTemplateContainer)obj;
            if (container.NodeKey == "*")
                return true;
            else
                return false;
        }

        protected void btnExpand_ServerClick(object sender, EventArgs e)
        {
            treeList.ExpandAll();
        }

        protected void btnCollapse_ServerClick(object sender, EventArgs e)
        {
            treeList.CollapseAll();
        }
    }
}