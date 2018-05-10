using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SSPWebUI.Data;
using Output;
namespace SSPWebUI.Views
{
    public partial class Compare : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                string ProtocolVersion = Request.QueryString["ProtocolVersionCKey"];
                
                DataTable dt = ProtocolsData.GetDraftVersions(decimal.Parse(ProtocolVersion));
                ddVersionLeft.Items.Add("");
                ddVersionRight.Items.Add("");
                foreach (DataRow dr in dt.Rows)
                {
                    ddVersionLeft.Items.Add(dr["DraftVersion"].ToString());
                    ddVersionRight.Items.Add(dr["DraftVersion"].ToString());
                }

                ddSection.Items.Add("Title");
                ddSection.Items.Add("Cover Page");
                ddSection.Items.Add("Authors");
                ddSection.Items.Add("Case Summary");
                ddSection.Items.Add("Notes and References");
            }
          
           if(ddVersionLeft.SelectedIndex==0 || ddVersionRight.SelectedIndex==0)
           {
               btnCompare.Visible = false;
           }
           else
           {
               btnCompare.Visible = true;
           }
        }

        protected void btnCompare_Click(object sender, EventArgs e)
        {
            
         
            right.InnerHtml = GetDiffHtml(left.InnerHtml, right.InnerHtml);
            
         
        }

        protected void ddVersionLeft_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ProtocolVersion = Request.QueryString["ProtocolVersionCKey"];
            string rightversion = ddVersionRight.SelectedItem.Text;
            string leftversion = ddVersionLeft.SelectedItem.Text;
            LoadHtmlData(left, leftversion);
            LoadHtmlData(right, rightversion); 
           
        }

        protected void ddVersionRight_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ProtocolVersion = Request.QueryString["ProtocolVersionCKey"];
            string rightversion = ddVersionRight.SelectedItem.Text;
            string leftversion = ddVersionLeft.SelectedItem.Text;
            LoadHtmlData(left, leftversion);
            LoadHtmlData(right, rightversion);          
           
        }

        private string GetDiffHtml(string text1, string text2)
        {
            HtmlDiff.HtmlDiff diffHelper = new HtmlDiff.HtmlDiff(text1, text2);
            // Lets add a block expression to group blocks we care about (such as dates)
            diffHelper.AddBlockExpression(new System.Text.RegularExpressions.Regex(@"[\d]{1,2}[\s]*(Jan|Feb)[\s]*[\d]{4}", System.Text.RegularExpressions.RegexOptions.IgnoreCase));
            return diffHelper.Build();

        }

        protected void ddSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            string rightversion = ddVersionRight.SelectedItem.Text;
            string leftversion = ddVersionLeft.SelectedItem.Text;
            LoadHtmlData(left, leftversion);
            LoadHtmlData(right, rightversion);           
        }

        protected void LoadHtmlData(System.Web.UI.HtmlControls.HtmlGenericControl ctl, string version)
        {
            string ProtocolVersion = Request.QueryString["ProtocolVersionCKey"];
             
            string section = ddSection.SelectedItem.Text;
            ctl.InnerHtml = "";

            if (version != "" && section == "Notes and References")
            {
                //string html = new SSPWebUI.Data.Note().GetMergedNotes(decimal.Parse(ProtocolVersion), decimal.Parse(version));
                string html = new HTMLHelper().GetNotes(ProtocolVersion, version, true,1);
                ctl.InnerHtml = html;
            }
            else if (version != "" && section == "Title")
            {
               ProtocolHeader hdr =  new SSPWebUI.Data.ProtocolHeaderData().Get(decimal.Parse(ProtocolVersion), 0, decimal.Parse(version));
               
                ctl.InnerHtml = "<p><b>Title:</b>" + hdr.Title + "</p>";
                ctl.InnerHtml = ctl.InnerHtml + "<p><b>Subtitle:</b>" + hdr.Subtitle + "</p>";
                ctl.InnerHtml = ctl.InnerHtml + "<p><b>Base Versions:</b><p><ul>";
                string[] baseversions = hdr.BaseVersions.Split(';');
                List<BaseVersion> baseversionslist = new BaseVersion().getAllVersions();
                foreach(string b in baseversions)
                {
                    foreach(BaseVersion basever in baseversionslist)
                    {
                        if(basever.Code==int.Parse(b))
                        {
                             ctl.InnerHtml = ctl.InnerHtml + "<li>" + basever.Label + "</li>";
                        }
                    }
                }
                ctl.InnerHtml = ctl.InnerHtml + "</ul>";               

            }
            else if (version != "" && section == "Cover Page")
            {
                ctl.InnerHtml = new SSPWebUI.Data.CoverPage().GetProcedure(decimal.Parse(ProtocolVersion), 0, decimal.Parse(version)).ProcedureDetails;
                
            }
                else if (version != "" && section == "Authors")
            {
                string html = "<table>";
                List<Author> authors = new SSPWebUI.Data.Author().getAuthors(decimal.Parse(ProtocolVersion), 0, decimal.Parse(version));
                html = html + "<tr><thead><th>Author</th><th>Role</th></thead><tbody>";
                    foreach(Author author in authors)
                    {
                        html = html + "<tr>";
                        html = html + "<td>" + author.Name + "</td>" + "<td>" + author.Role + "</td>";
                        html = html + "</tr>";
                    }
                    html = html + "</tbody></table>";
                    ctl.InnerHtml = html;
            }
            else if (version != "" && section == "Case Summary")
            {
                List<Checklist> checklists = ProtocolsData.GetChecklists(decimal.Parse(ProtocolVersion));
                foreach(Checklist c in checklists)
                {
                    //ctl.InnerHtml = ctl.InnerHtml + "<h2>" + c.Name + "</h2>";
                    HTMLHelper html = new HTMLHelper();
                    ctl.InnerHtml = ctl.InnerHtml + html.GetCaseSummary(c.ChecklistCKey.ToString(), version);
                }
                
                
            }
        }
    }
}