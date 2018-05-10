using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using SSPWebUI.Data;
using System.Web.Script.Services;
using System.Data;
using System.Web.Script.Serialization;

namespace SSPWebUI.Service
{
    /// <summary>
    /// Summary description for SSPService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]

    public class ItemType
    {
        public string Code { get; set; }
        public string Label { get; set; }
    }
        
        public class Header
    {
        protected string Title {get;set;}
        protected string Subtitle {get;set;}
        protected string ProtocolVersionCKey {get;set;}
        protected string ProtocolHeaderCKey {get;set;}
        protected string AJCC_UICC_Version {get;set;}
        protected string UserCKey {get;set;}
    }
         

        public class SSPService : System.Web.Services.WebService
    {
        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string Hello(string name)
        {
            return "Hello, " + name;
        }

        //[WebMethod]
        //[ScriptMethod(UseHttpGet = true)]
        //public string ItemTypes()
        //{

        //    DataTable dt = Data.ItemTypes.GetItemTypes();
        //    string retval = "";
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        if (retval == "")
        //            retval = "\"" + dr["ItemTypeKey"].ToString() + "\":" + "\"" + dr["TypeShortName"].ToString().Trim() + "\"";
        //        else
        //            retval = retval + ",\"" + dr["ItemTypeKey"].ToString() + "\":" + "\"" + dr["TypeShortName"].ToString().Trim() + "\"";

        //    }


        //    return "{" + retval + "}";


        //}
        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ItemTypes()
        {

            DataTable dt = Data.ItemType.GetItemTypes();
            string retval = "";
            foreach (DataRow dr in dt.Rows)
            {
                // ComboBoxNote is the same as Note, skip
                if (dr["ItemTypeKey"].ToString()=="26")
                {
                    continue;
                }
                if (retval == "")
                    retval = "{\"key\":\"" + dr["ItemTypeKey"].ToString() + "\",\"label\":" + "\"" + dr["TypeShortName"].ToString().Trim() + "\"}";
                else
                    retval = retval + ",{\"key\":\"" + dr["ItemTypeKey"].ToString() + "\",\"label\":" + "\"" + dr["TypeShortName"].ToString().Trim() + "\"}";

            }


            return "[" + retval + "]";


        }
        //[WebMethod]
        //[ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        //public string ItemTypes()
        //{
            
            
        //    DataTable dt = Data.ItemTypes.GetItemTypes();
            
        //    List<ItemType> types = new List<ItemType>();
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        ItemType t = new ItemType();
        //        t.Code = dr["ItemTypeKey"].ToString();
        //        t.Label = dr["TypeShortName"].ToString().Trim();
        //        types.Add(t);
        //    }
        //    JavaScriptSerializer js = new JavaScriptSerializer();
        //    Context.Response.Clear();
        //    Context.Response.ContentType = "application/json";         
        //    Context.Response.Write(js.Serialize(types));
        //  }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string UnitTypes()
        {

            DataTable dt = Data.AnswerUnits.GetAnswerUnitTypes();
            string retval = "";
            foreach (DataRow dr in dt.Rows)
            {
                if (retval == "")
                    retval = "\"" + dr["Unit_Id"].ToString() + "\":" + "\"" + dr["Unit_Name"].ToString().Trim() + "\"";
                else
                    retval = retval + ",\"" + dr["Unit_Id"].ToString() + "\":" + "\"" + dr["Unit_Name"].ToString().Trim() + "\"";

            }


            return "{" + retval + "}";


        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string UserTypes()
        {

            List<UserType> usertypes = Data.UserType.getUserTypes();
            string retval = "";
            foreach (UserType type in usertypes)
            {
                if (retval == "")
                    retval = "\"" + type.UserTypeCKey + "\":" + "\"" + type.UserTypeName + "\"";
                else
                    retval = retval + ",\"" + type.UserTypeCKey + "\":" + "\"" + type.UserTypeName + "\"";

            }
            return "{" + retval + "}";
            
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string QualificationTypes()
        {

            List<Qualification> qualtypes = Data.Qualification.getQualificationTypes();
            string retval = "";
            foreach (Qualification type in qualtypes)
            {
                if (retval == "")
                    retval = "\"" + type.QualificationCkey + "\":" + "\"" + type.QualificationDesc + "\"";
                else
                    retval = retval + ",\"" + type.QualificationCkey + "\":" + "\"" + type.QualificationDesc + "\"";

            }
            return "{" + retval + "}";

        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string NotesList(string parm)
        {
            if (parm == "") return "{}";
            List<Note> notes = new Data.Note().getProtocolNotes(decimal.Parse(parm),1,1);
            string retval = "";
            foreach (Note n in notes)
            {

                if (retval == "")
                    retval = "\"" + n.CKey + "\":" + "\"" + n.Title + "\"";
                else
                    retval = retval + ",\"" + n.CKey + "\":" + "\"" + n.Title + "\"";

            }

           //remove non-ascii
            retval = System.Text.RegularExpressions.Regex.Replace(retval, @"[^\u0000-\u007F]+", string.Empty);
           
            //remove control characters
            string output = new string(retval.Where(c => !char.IsControl(c)).ToArray());
            return "{" + retval + "}";

        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public bool CanDelete(string parm)
        {
            if (parm.StartsWith("*"))
                parm = parm.Replace("*", "");
            decimal ckey = 0;
            decimal.TryParse(parm, out ckey);
            if (ckey == 0)
                return false;
            if (ChecklistTemplateItems.CanDelete(decimal.Parse(parm), 0))
                return true;
            else
                return false;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public bool CanRestore(string parm)
        {
            if (ChecklistTemplateItems.CanRestore(decimal.Parse(parm)))
                return true;
            else
                return false;
        }


        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public bool UpdateHeaders(string Title, string Subtitle, string ProtocolVersionCKey, string ProtocolHeaderCKey, string AJCC_UICC_Version, string UserCKey)
        {
            ProtocolHeader hdr = new ProtocolHeader();
            hdr.HeaderKey = decimal.Parse(ProtocolHeaderCKey);
            hdr.Title = Title;
            hdr.Subtitle = Subtitle;
            hdr.ProtocolVersionCKey = decimal.Parse(ProtocolVersionCKey);
            hdr.BaseVersions = AJCC_UICC_Version;
            hdr.CreatedBy = decimal.Parse(UserCKey);

            new ProtocolHeaderData().Save(hdr);
            //save
            return true;
        }


        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string GetVisibleText(string parm)
        {
            parm = parm.Replace("*", "");

            DataTable dt = ChecklistTemplateItems.GetCheckListItem(decimal.Parse(parm),1,1);
            string retval = "";
            if (dt.Rows.Count > 0)
            {
                retval = dt.Rows[0]["VisibleText"].ToString();
            }
            return retval;

        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public  string RequiredStatus()
        {
            return "{\"1\":\"Yes\",\"0\":\"No\", \"2\":\"Conditional\"}";
        }

        [WebMethod]
       // [ScriptMethod(UseHttpGet = true, ResponseFormat=ResponseFormat.Xml)]
        [ScriptMethod(UseHttpGet = true)]
        public string getBookmarks(string parm)
        {

            HtmlAgilityPack.HtmlDocument htmldoc = new HtmlAgilityPack.HtmlDocument();
            

            if(parm.Length>0)
            {
                string NoteCKey = parm.Split('|')[0];
                string mode = parm.Split('|')[1];
                string version = parm.Split('|')[2];

                List<Reference> refs = new Reference().GetReferences(decimal.Parse(NoteCKey),int.Parse(mode),decimal.Parse(version));

                //parm is protocol version
                //List<Bookmark> bookmarks = new Bookmark().getBookmarks(decimal.Parse(NoteCKey),int.Parse(mode), decimal.Parse(version));
                string html = string.Empty;
                html = "<option/>";

                int i = 1;

                foreach (Reference temp in refs)
                //foreach (Bookmark temp in bookmarks)
                {
                    htmldoc.LoadHtml(temp.ReferencesContent);
                    
                    string innerText = htmldoc.DocumentNode.InnerText;
                    if (innerText.Length > 100)
                        innerText = innerText.Substring(0, 100) + " ...";
                    //html = html + "<option value = '" + temp.BookmarkCKey + "'>" + "Reference " + temp.ReferenceNumber + "</option>";
                    //html = html + "<option style='width:280px overflow:hidden; white-space: nowrap;text-overflow: ellipses;' value = '" + temp.ReferenceCKey + "'>" + temp.Number + " - " + innerText + "</option>";
                    //do not show numbers
                    html = html + "<option style='width:280px overflow:hidden; white-space: nowrap;text-overflow: ellipses;' value = '" + temp.ReferenceCKey + "'>" + i + " - " + innerText + "</option>";
                    i++;
                }
                html = "<p>Please select a reference to insert from the list below</p> <select style='width:300px' id='marks' onchange=showReferenceText()>" +
                html + "</select><hr/>";
                html = html + "<div id=referencepreview/>";
                return html;
            }
            else
            {
                return "";
            }
            //return "<root>" + html + "</root>";
        }

        [WebMethod]
        public string getReferenceText(string parm)
        {
            //string refefenceckey = parm;  //notesreferenceckey
            string refefenceckey = parm.Split('|')[0];
            string mode = parm.Split('|')[1];
            string version = parm.Split('|')[2];
            string retval = "";
            Reference obj = new Reference();
            decimal refckey = -1;
            decimal.TryParse(refefenceckey, out refckey);
            if (refckey > 0)
                retval = obj.GetReference(refckey, int.Parse(mode), decimal.Parse(version)).ReferencesContent;
            return retval;
        }

      

        [WebMethod]
        public string NoteTitle(string parm)
        {   
            string noteckey = parm.Split('|')[0];
            string version = parm.Split('|')[2];
            
            string pattern = "[0-9]+.100004300";
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(pattern);
            
            string retval="";

            if (reg.IsMatch(noteckey))
            {
                if (version == "999")   //work version
                {
                    retval = new Note().GetNote(decimal.Parse(noteckey), 1, 999).Title;
                }
                else
                {
                    retval = new Note().GetNote(decimal.Parse(noteckey), 0, decimal.Parse(version)).Title;
                }
            }
           
            
            return retval;
        }
    }
}
