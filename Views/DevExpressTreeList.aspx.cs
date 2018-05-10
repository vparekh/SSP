using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxTreeList;

namespace SSPWebUI.Views
{
    public partial class DevExpressTreeList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Array values = Enum.GetValues(typeof(TreeListEditMode));
                foreach (object value in values)
                    cmbMode.Items.Add(Enum.GetName(typeof(TreeListEditMode), value), (int)value);
                cmbMode.Value = treeList.SettingsEditing.Mode.ToString();
                //AccessDataSource1..Restore();
                treeList.DataBind();
                treeList.ExpandToLevel(3);
                treeList.Columns[1].Width = 100;
                //treeList.StartEdit("2");
            }
            treeList.SettingsEditing.AllowNodeDragDrop = chkDragging.Checked;
        }

        //protected string GetCellText(TreeListDataCellTemplateContainer container)
        //{
        //    var secondLevelHiddenColumns = new string[] { "PHONE1", "PHONE2" };
        //    var thirdLevelHiddenColumns = new string[] { "LOCATION", "BUDGET" };

        //    var colName = container.Column.FieldName;
        //    if (container.Level == 2 && secondLevelHiddenColumns.Contains(colName))
        //        return string.Empty;

        //    if (container.Level == 3 && thirdLevelHiddenColumns.Contains(colName))
        //        return string.Empty;

        //    return container.Text;
        //}

        protected void cmbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            treeList.SettingsEditing.Mode = (TreeListEditMode)cmbMode.SelectedItem.Value;
        }
    }
}