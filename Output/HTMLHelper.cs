using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;
using SSPWebUI.Data;
using System.Globalization;
using System.Data.SqlClient;

namespace Output
{
    public class HTMLHelper
    {
        public Page CurrentPage { get; set; }
        private string BuildStagingTable(string xml)
        {
            string html = xml.Replace("staging-table", "table");
            html = html.Replace("<br/>", "");
            html = html.Replace("title", "tr");
            html = html.Replace("row", "tr");
            
            return html;
        }

        public string GetCaseSummary(string checklistckey, string draftversion)
        {
            eCCDocx doc = new eCCDocx();


            string html = doc.CreateeCCContent(null, checklistckey, draftversion);
            return html;
        }
        
        public string ProtocolPostingDate { get; set; }

        public string GetProcedure(string protocolversion, string draftversion)
        {
            return new SSPWebUI.Data.CoverPage().GetProcedure(decimal.Parse(protocolversion), 0, decimal.Parse(draftversion)).ProcedureDetails;
        }

        public string GetTitle(string protocolversion, string draftversion)
        {
            SSPWebUI.Data.ProtocolHeader hdr = new SSPWebUI.Data.ProtocolHeaderData().Get(decimal.Parse(protocolversion), 0, decimal.Parse(draftversion));

            ProtocolPostingDate = hdr.WebPostingDate.Value.ToString("MMMM", CultureInfo.InvariantCulture) + " " + hdr.WebPostingDate.Value.Year;
            string html =  "<h1>" + hdr.Title + "</h1>";
            
            //if(hdr.Subtitle!="")
            //    html = html  +  "<h2>" + hdr.Subtitle + "</h2>";
            html = html + "<table style='width:100%'><tr><td>" + "<span><b>Version</b>: " + hdr.ProtocolVersion + "</span></td>";


            if (hdr.WebPostingDate.Value != null)
            html = html + "<td align='right'><span><b>Protocol Posting Date</b>: " + hdr.WebPostingDate.Value.ToString("MMMM", CultureInfo.InvariantCulture) + " " + hdr.WebPostingDate.Value.Year + "</td></span></tr></table>";

            html = html + "<p><b>Base Versions:</b><p><ul>";
            string[] baseversions = hdr.BaseVersions.Split(';');
            List<SSPWebUI.Data.BaseVersion> baseversionslist = new SSPWebUI.Data.BaseVersion().getAllVersions();
            foreach (string b in baseversions)
            {
                foreach (BaseVersion basever in baseversionslist)
                {
                    if (b != "" && basever.Code == int.Parse(b))
                    {
                        html = html  + "<li>" + basever.Label + "</li>";
                    }
                }
            }
            html = html +  "</ul>";

            return html;
       
        }

        public string GetCopyright()
        {
            return "<b>Copyright paragraph here</b>";
        }

        public string GetNotes(string templateversion, string draftversion, bool FormatReferenceForHTML = true, int referenceoption = 1)
        {
            List<SSPWebUI.Data.Note> notes = new SSPWebUI.Data.Note().getProtocolNotes(decimal.Parse(templateversion), 0, decimal.Parse(draftversion));
            string mergedhtml = "";
            System.Text.RegularExpressions.Regex rex;
            System.Text.RegularExpressions.MatchCollection collection;

            foreach (SSPWebUI.Data.Note note in notes)
            {
                string notetitle = "<a id='" + note.CKey + "'/><h3>" + note.Title + "</h3>";
                if (note.Detail=="")
                {
                    continue;
                }
                mergedhtml = mergedhtml + notetitle + GetBodyText(note.Detail);
                
                //remove comments
                string replacevalue = "";
                bool stripcomments = true;
                if(stripcomments)
                {
                    rex = new System.Text.RegularExpressions.Regex(@"<a href=""[0-9]+.100004300"".+?</a>");  //note the lazy match +?
                    collection = rex.Matches(mergedhtml);

                    foreach (System.Text.RegularExpressions.Match m in collection)
                    {
                        if (m.Value.Contains("commentckey"))
                            mergedhtml = mergedhtml.Replace(m.Value, "");
                    }
                }


                
                if(referenceoption==1)
                {
                    

                     rex = new System.Text.RegularExpressions.Regex(@"href=""[0-9]+.100004300""");
                    
                     collection = rex.Matches(mergedhtml);

                    
                     

                    foreach (System.Text.RegularExpressions.Match m in collection)
                    {
                       
                        replacevalue = "href=" + "'#" + m.Value.Split('=')[1].Replace(@"""", "").Replace("'", "") + "_" + note.CKey + "'";
                                                
                        mergedhtml = mergedhtml.Replace(m.Value, replacevalue);
                    }

                 


                    List<SSPWebUI.Data.Reference> references =  new SSPWebUI.Data.Reference().GetReferences(decimal.Parse(note.CKey), 0, decimal.Parse(draftversion));

                    if (references.Count > 0)
                    {
                        mergedhtml = mergedhtml + "<h3>References</h3>";
                    }

                    string reference = "<table>";
                    foreach(SSPWebUI.Data.Reference refer in references)
                    {
                        reference = reference + "<tr>";
                        if(FormatReferenceForHTML)
                        {
                            reference = reference + "<td valign='top'> <a id=" + refer.ReferenceCKey + "_" + note.CKey + ">" + refer.Number + ". </a></td><td valign='top'>"
                                    + refer.ReferencesContent + "</td>";
                        }
                        else
                        {
                            mergedhtml = mergedhtml + "<div> <a name=" + refer.ReferenceCKey + "_" + note.CKey + "><span>" + refer.Number + ". </span>"
                                    + refer.ReferencesContent + "</a></div>";
                        }
                        reference = reference + "</tr>";
                    }

                    reference = reference + "</table>";
                    mergedhtml = mergedhtml + reference;
                }

                
            }
            
            return mergedhtml;
        }

        public string GetBodyText(string DocumentContent)
        {
            string pattern = "(?s)<body>(.*)</body>";  //(?s) includes line-breaks
            System.Text.RegularExpressions.Regex regx = new System.Text.RegularExpressions.Regex(pattern);
            System.Text.RegularExpressions.MatchCollection col = regx.Matches(DocumentContent);
            string retval = col[0].Groups[1].Value;
            return retval;
        }

        public string GetMiscText(string ProtocolVersionCKey, int Type)
        {
           return SSPWebUI.Data.ProtocolsData.GetMiscText(decimal.Parse(ProtocolVersionCKey), 1);
        }

        public string GetAuthors(string templateversion, string draftversion)
        {
            string html = "<p><b>Authors</b></p>";
            
            
            List<SSPWebUI.Data.Author> authors = new SSPWebUI.Data.Author().getAuthors(decimal.Parse(templateversion), 0, decimal.Parse(draftversion));
            if (authors.Count == 0)
                return "";
            string authorslist="";
            foreach (SSPWebUI.Data.Author author in authors)
            {
                if (author.RoleCKey == (decimal)1.100004300)
                {
                    authorslist = authorslist + ", " +  author.Name.Trim() + "*";

                }
                else
                {
                    authorslist = authorslist + ", " + author.Name.Trim();
                }
                
                
            }

            html = html + authorslist.Substring(1);

            html = html + "<br/><br/>With guidance from the CAP Cancer and CAP Pathology Electronic Reporting Committees.";
            html = html + "<br/><i>* Denotes primary author. All other contributing authors are listed alphabetically.</i>"; 
            return html;
        }

      
    }
}