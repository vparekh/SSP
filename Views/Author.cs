using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace SSPWebUI.Views
{
    public class Author
    {
        public string Name { get; set; }
        public decimal CKey { get; set; }
        public short Role { get; set; }  //1- primary, 2- secondary, 3- contributor, etc.
        public List<Author> getAuthors(decimal PrptocolCKey)
        {
            List<Author> authors = new List<Author>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["eCCData"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select protocolauthorckey, roleckey from ssp_protocolauthor_role";  //what is the use of ssp_protocolauthor??
                cmd.Connection = cn;
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();


                while (rdr.Read())
                {
                    decimal authorckey = (decimal)rdr["protocolauthorckey"];
                    decimal roleckey = (decimal)rdr["RolecKey"];
                    SqlCommand cmd1 = new SqlCommand();
                    cmd1.CommandText = "select firstname, lastname, middlename, qualification from ssp_users where userckey =  " + authorckey;
                    cmd1.Connection = cn;
                    //cannot execute another reader
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    Name = dt.Rows[0]["FirstName"].ToString() + " " + dt.Rows[0]["MiddleName"].ToString() + " " + dt.Rows[0]["LastName"].ToString() + ", " + dt.Rows[0]["Qualification"].ToString();
                    CKey = authorckey;

                    //get role here
                    Role = 0;  //unassigned
                    authors.Add(this);
                }
            }
            return authors;
        }
    }
}