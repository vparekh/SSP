using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SSPWebUI.Data;

namespace SSPWebUI.Views
{
    public partial class LeftNavigationPanel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            //DataTable dt = ProtocolsData.GetProtocols();
            //foreach(DataRow dr in dt.Rows)
            //{
            //    TableRow tr = new TableRow();
            //    TableCell tc = new TableCell();
            //    tc.Text = dr["ProtocolName"].ToString();
            //    tc.ID = dr["ProtocolCKey"].ToString();
            //    tr.Cells.Add(tc);
            //    tblProtocols.Rows.Add(tr);
            //}
        }
    }
}