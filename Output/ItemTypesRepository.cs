using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel;
using System.Configuration;

namespace SSPWebUI.Model
{
    public class ItemTypesRepository: IItemTypesRepository
    {
        private List<ItemType> _types = new List<ItemType>();

        public ItemTypesRepository()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["eCCData"].ConnectionString);
            using (SqlDataAdapter da = new SqlDataAdapter(@"select ItemTypeKey, TypeName , TypeShortName , TypePreferredShortName from ListOfItemTypes where TypeShortName is not null order by sortorder", con))
            {
                con.Open();
                da.Fill(dt);
                ItemType type = null;
                foreach (DataRow dr in dt.Rows)
                {
                    type = new ItemType();
                    type.ItemTypeKey = int.Parse(dr["ItemTypeKey"].ToString());
                    type.TypeName = dr["ItemType"].ToString();
                    type.TypeShortName = dr["TypeShortName"].ToString();
                    this.Add(type);
                }

                //add an empty row


            }
            con.Close();
        }

        public IEnumerable<ItemType>GetAll()
        {          

            return _types;
        }

        public void Add(ItemType type)
        {
            _types.Add(type);
        }

        public ItemType Get(int id)
        {
            return _types.Find(p => p.ItemTypeKey == id);
        }

    }
}