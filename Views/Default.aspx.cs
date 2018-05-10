using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxTreeList;

public partial class _Default : System.Web.UI.Page {
    
    protected void Page_Load(object sender, EventArgs e) {
       
    }

    protected string GetCellText(TreeListDataCellTemplateContainer container) {
        var secondLevelHiddenColumns = new string[] { "PHONE1", "PHONE2" };
        var thirdLevelHiddenColumns = new string[] { "LOCATION", "BUDGET" };

        var colName = container.Column.FieldName;
        if(container.Level == 2 && secondLevelHiddenColumns.Contains(colName))
            return string.Empty;

        if(container.Level == 3 && thirdLevelHiddenColumns.Contains(colName))
            return string.Empty;
        
        return container.Text;
    }
}