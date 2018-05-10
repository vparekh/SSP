using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using DevExpress.Web.ASPxTreeList;
using DevExpress.Web;
using System.Data;
using System.Data.SqlClient;
using DevExpress.Web.Data;

namespace SSPWebUI
{
    public partial class TreeListTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if(!Page.IsPostBack)
            {
                DataTable test = getTreeData("184.100004300");
                TreeListNode root = treeList.AppendNode(1, null);
                root["ItemText"] = "ChecklistItems";

                treeList.SettingsEditing.AllowNodeDragDrop = true;
                treeList.ParentFieldName = "ParentItemCKey";
                root.Expanded = true;
                //select parentit
                foreach (DataRow dr in test.Rows)
                {
                    if (dr["ParentItemCkey"] == System.DBNull.Value)
                    {


                        TreeListNode temp = treeList.AppendNode(decimal.Parse(dr["ChecklistTemplateItemCkey"].ToString()), root);
                        temp["ItemText"] = dr["VisibleText"].ToString();
                        temp["MinRepetitions"] = dr["MinRepetitions"].ToString();
                        temp["MaxRepetitions"] = dr["MaxRepetitions"].ToString();
                        temp.Expanded = true;
                    }
                    else
                    {
                        decimal parent = decimal.Parse(dr["ParentItemCKey"].ToString());
                        TreeListNode temp = treeList.AppendNode(decimal.Parse(dr["ChecklistTemplateItemCkey"].ToString()), treeList.FindNodeByKeyValue(parent.ToString()));
                        temp["ItemText"] = dr["VisibleText"].ToString();
                        temp["MinRepetitions"] = dr["MinRepetitions"].ToString();
                        temp["MaxRepetitions"] = dr["MaxRepetitions"].ToString();
                        temp.Expanded = true;

                    }

                }
            }

            
        }

        protected void treeList_NodeUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            e.NewValues["Text"] = ExtractMemoValue();
        }
        protected void treeList_NodeInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            e.NewValues["Text"] = ExtractMemoValue();
        }
        string ExtractMemoValue()
        {
            ASPxPageControl pageControl = treeList.FindEditFormTemplateControl("tabs") as ASPxPageControl;
            ASPxMemo memo = pageControl.TabPages[0].FindControl("vistext") as ASPxMemo;
            return memo.Text;
        }

        private void treeList_ShownEditor(object sender, EventArgs e)
        {
            
            //if (((TreeList)sender).FocusedNode[columnItemValue].ToString() == "1") //Your condition
            //    ((TreeList)sender).ActiveEditor.Properties.ReadOnly = true;
        }
        private DataTable getTreeData(string templateckey)
        {
            SqlConnection cn = new SqlConnection("server=uappecc;initial catalog=PERC_TEST_eCC;Integrated Security=SSPI");
            SqlCommand cmd = new SqlCommand("select * from ChecklistTemplateItems where ChecklistTemplateVersionCKey = " + templateckey + " order by sortorder");
            cmd.Connection = cn;
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            System.Data.DataTable dt = new System.Data.DataTable();
            ad.Fill(dt);
            return dt;
        }
    }
}