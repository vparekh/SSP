using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Configuration;

namespace SSPWebUI.Model
{
    public class ItemType
    {
      /*  public int ItemTypeKey { get; set; }
        public string TypeName { get; set; }
        public string TypeShortName { get; set; }*/

    }

    //public class ItemTypes
    //{

    //    [DataObjectMethod(DataObjectMethodType.Select)]
    //    public static DataTable GetItemTypes()
    //    {
            
    //        DataSet ds = new DataSet();
    //        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["eCCData"].ConnectionString);
    //        using (SqlDataAdapter da = new SqlDataAdapter(@"select ItemTypeKey, TypeName , TypeShortName , TypePreferredShortName from ListOfItemTypes where TypeShortName is not null order by sortorder", con))
    //        {
    //            con.Open();
    //            da.Fill(ds, "ItemTypes");

    //            //add an empty type
    //            DataRow dr = ds.Tables["ItemTypes"].NewRow();
    //            dr["ItemTypeKey"] = System.DBNull.Value;
    //            dr["TypeName"] = "";
    //            dr["TypeShortName"] = System.DBNull.Value;
    //            ds.Tables["ItemTypes"].Rows.Add(dr);

    //            con.Close();
    //            return ds.Tables["ItemTypes"];
    //        }
    //    }
    //    public static int GetItemTypeByShortName(string shortName)
    //    {
    //        using(SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["eCCData"].ConnectionString))
    //        {
    //            using (SqlCommand cmd = new SqlCommand(@"select ItemTypeKey from ListOfItemTypes where TypeShortName = '" + shortName + "'", con))
    //            {
    //                con.Open();
    //                var retval = cmd.ExecuteScalar();
    //                con.Close();
    //                return int.Parse(retval.ToString());
                    
    //        }
    //    }
    //    }
    //}
}