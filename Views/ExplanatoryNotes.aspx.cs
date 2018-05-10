using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using DevExpress.Web.ASPxTreeList;
using DevExpress.Web.ASPxRichEdit;
using DevExpress.Web.Data;
using DevExpress.Web.Office;
using DevExpress.XtraRichEdit;
using DevExpress.Web;
using DevExpress.Office;

using System.Data.SqlClient;
using System.IO;
using SSPWebUI.Data;

using Newtonsoft.Json;

namespace SSPWebUI.Views
{
    public partial class ExplanatoryNotes : System.Web.UI.Page
    {
        string SessionKey = "EditedDocuemntID";

        protected string EditedDocuemntID
        {
            get { return (string)Session[SessionKey] ?? string.Empty; }
            set { Session[SessionKey] = value; }
        }
        protected void DemoRichEdit_PreRender(object sender, EventArgs e)
        {
            RERFontSizeCommand myRERFontSizeCommand = (RERFontSizeCommand)reNotes.RibbonTabs[1].Groups.Find(g => g is RERFontGroup).Items.Find(i => i is RERFontSizeCommand);
            myRERFontSizeCommand.Items.Clear();
            myRERFontSizeCommand.Items.AddRange(
                new List<ListEditItem> {
                new ListEditItem() { Value = 8, Text = "8" },
                new ListEditItem() { Value = 10, Text = "10" },
                new ListEditItem() { Value = 12, Text = "12" },
                new ListEditItem() { Value = 14, Text = "14" }
            }
                );

            RERFontNameCommand myRERFontNameCommand = (RERFontNameCommand)reNotes.RibbonTabs[1].Groups.Find(g => g is RERFontGroup).Items.Find(i => i is RERFontNameCommand);
            myRERFontNameCommand.Items.Clear();
            myRERFontNameCommand.Items.AddRange(
                new List<ListEditItem> {
                new ListEditItem() { Value = "Arial", Text = "Arial" },
                new ListEditItem() { Value = "Verdana", Text = "Verdana" },
                new ListEditItem() { Value = "Calibri", Text = "Calibri" }
            }
                );
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           
            string note = "";
            Master.ProtocolList.SelectedIndexChanged += ProtocolList_SelectedIndexChanged;
            
                if (!Page.IsPostBack)
                {
                    //reNotes.ActiveTabIndex = 1;
                    //user came here from another page
                    txtProtocolVersion.Text = Session["CurrentProtocol"].ToString();
                    LoadTags(note);
                   
                    if (Request.QueryString["note"] == null)
                    {
                        note = "A";
                    }
                    else
                    {
                        note = Request.QueryString["note"];

                    }
                    OpenNote(decimal.Parse(txtProtocolVersion.Text), note);
                }
                else
                {

                    //highlight the current tab
                    if (ScriptManager.GetCurrent(this.Page).IsInAsyncPostBack)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "UpdateMsg", 
                            @"$(document).ready(function(){ var tabID = '#' + $('#tabid').text();
                            $(tabID).addClass('current');});", true);

                    }
                    else
                    {
                        // regular full page postback occured
                        // custom logic accordingly                
                    }
                    txtProtocolVersion.Text = Master.ProtocolList.SelectedValue; //post back
                    note = lstTags.SelectedValue;
                }             
                
               
               
            
        }

        void LoadTags(string NoteTag)
        {           
            
            List<string> tags = new Note().getTags(decimal.Parse(txtProtocolVersion.Text));
            lstTags.Items.Clear();
            if (Request.IsAuthenticated)
                lstTags.Items.Add("-- New --");

            int index = -1;
            int i = 0;
            foreach (string test in tags)
            {
                if (test.Trim() == NoteTag.Trim())
                    index = i;
                lstTags.Items.Add(test);
                i++;
            }

            lstTags.SelectedIndex = index;
        }

        void ProtocolList_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtProtocolVersion.Text = ((DropDownList)sender).SelectedValue;
            Session.Add("CurrentProtocol", txtProtocolVersion.Text);
            string note = "";
            if (Request.QueryString["note"] == null)
            {
                note = "A";
            }
            else
            {
                note = Request.QueryString["note"];

            }
            OpenNote(decimal.Parse(txtProtocolVersion.Text), note);
        }

        protected void OpenNote(decimal ProtocolVersionCKey, string NoteTag)
        {


            Note note;
            note = new Note().GetNote(ProtocolVersionCKey, NoteTag);

            txtNoteTitle.Text = note.Title;

            List<string> tags = note.getTags(ProtocolVersionCKey);
            lstTags.Items.Clear();
            if (Request.IsAuthenticated)
                lstTags.Items.Add("-- New --");

            int index = -1;
            int i = 0;
            foreach (string test in tags)
            {
                if (test.Trim() == NoteTag.Trim())
                    index = i;
                lstTags.Items.Add(test);
                i++;
            }
            if (Request.IsAuthenticated)
                lstTags.SelectedIndex = index + 1;
            else
                lstTags.SelectedIndex = index;

            Guid g;

            g = Guid.NewGuid();
            RichEditDocumentServer server = new RichEditDocumentServer();
            server.Document.DefaultCharacterProperties.FontName = "Arial";
            server.Document.DefaultCharacterProperties.FontSize = 12;
            EditedDocuemntID = g.ToString();
            reNotes.Open(
                 EditedDocuemntID,
                 DocumentFormat.Html,
                 () =>
                 {
                     byte[] docBytes = null;

                     docBytes = System.Text.Encoding.ASCII.GetBytes(note.Detail);
                     return new MemoryStream(docBytes);
                 }
             );

        }

        protected void reNotes_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            string[] parameters = e.Parameter.Split('|');
            if (parameters[0] == "open")
            {

                if (parameters.Length > 2)
                    OpenNote(decimal.Parse(parameters[1]), parameters[2].Trim());
                else
                    OpenNote(decimal.Parse(parameters[1]), "A");
            }
            else
            {
                ASPxRichEdit rich = sender as ASPxRichEdit;

                using (MemoryStream ms = new MemoryStream())
                {
                    rich.SaveCopy(ms, DocumentFormat.Html);

                    ms.Position = 0;

                    
                    decimal protocolversion = decimal.Parse(txtProtocolVersion.Text);


                    string user = Request.Cookies["UserSettings"]["UserCKey"].ToString(); // Session["userckey"].ToString();
                    int index = lstTags.SelectedIndex;
                    string tag = lstTags.Items[index].Text.Trim();
                    if (tag == "-- New --")
                    {
                        //generate new tag
                        tag = "";
                    }
                    Note note = new Note();

                    note.saveNote(protocolversion, decimal.Parse(user), tag, txtNoteTitle.Text, ms);
                    lstTags.DataSource = note.getTags(protocolversion);
                }
            }


        }

        protected void btnNewNote_Click(object sender, EventArgs e)
        {
            //clear rich edit
            Guid g = Guid.NewGuid();

            EditedDocuemntID = g.ToString();
            RichEditDocumentServer server = new RichEditDocumentServer();
            server.Document.DefaultCharacterProperties.FontName = "Arial";
            server.Document.DefaultCharacterProperties.FontSize = 12;
            reNotes.Open(
                 EditedDocuemntID,
                 DocumentFormat.Html,
                 () =>
                 {
                     return new MemoryStream(System.Text.Encoding.ASCII.GetBytes(""));
                 }
             );

            lstTags.SelectedIndex = 0;
            txtNoteTitle.Text = "";
        }
        protected void lstTags_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            OpenNote(decimal.Parse(txtProtocolVersion.Text), lstTags.SelectedValue);

        }

        
    }
}