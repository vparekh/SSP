using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraRichEdit.Commands;
using DevExpress.XtraRichEdit.Internal;
using System.Drawing;
using System.IO;
using System.Data;

namespace Output
{
    public class eCCDocx
    {
        public Dictionary<string, CKeyHierarchy> ancestry = new Dictionary<string, CKeyHierarchy>();
        
        string lastparent = "";
        List<string> combotext = new List<string>();

        public string CreateeCCContent(Document doc, string templateckey, string version)
        {
            string saveckey = string.Empty;
            string savetext = string.Empty;
            string saverequired = string.Empty;
            string saveitemtype = string.Empty;
            string html = "";

            
            ancestry = new Dictionary<string, CKeyHierarchy>();
                     

            System.Data.DataTable dt = SSPWebUI.Data.ChecklistTemplateItems.GetAllChecklistItemsForoutput(decimal.Parse(templateckey), 0, decimal.Parse(version)); //data.GeteCCBuilderView(templateckey, version);

            int dimensioncount = 0;
            string lastckey = "";
            //build ancestry dictionary
            foreach (DataRow dr in dt.Rows)
            {
                if (!ancestry.ContainsKey(dr["ChecklistTemplateItemCkey"].ToString()))
                {
                    CKeyHierarchy itm = new CKeyHierarchy();
                    itm.CKey = dr["ChecklistTemplateItemCkey"].ToString();

                    itm.PreviousSibling = lastckey;

                    //make sure parentckey is not deprecated
                    if (dr["ParentItemCKey"] != System.DBNull.Value && dr["ParentItemCKey"].ToString()!="")
                    {
                        itm.ParentCKey = dr["ParentItemCKey"].ToString();

                       
                    }
                    else
                    {
                        itm.ParentCKey = "";
                    }
                        

                    itm.Depth = 0;
                    itm.ItemType = int.Parse(dr["ItemTypeKey"].ToString());
                    itm.Required = bool.Parse(dr["AuthorityRequired"].ToString());
                    itm.ItemText = dr["visibletext"].ToString();
                    lastckey = itm.CKey;
                    //find parent ckey in ancestry and set the depth value
                    foreach (KeyValuePair<string, CKeyHierarchy> kvp in ancestry)
                    {

                        if (kvp.Key == itm.ParentCKey)
                        {
                            itm.Depth = kvp.Value.Depth + 1;
                        }
                    }

                    ancestry.Add(itm.CKey, itm);
                }
            }

            foreach (DataRow dr in dt.Rows)
            {
                
                string retval = "";
                
                if (!(dr["visibletext"].ToString().ToLower().Contains("greatest dimension") || dr["visibletext"].ToString().ToLower().Contains("additional dimension")) || int.Parse(dr["ItemTypeKey"].ToString())!=17)
                {
                    if (dimensioncount > 0)
                    {
                        retval = PrintItem(savetext, int.Parse(saveitemtype), bool.Parse(saverequired), dimensioncount, saveckey, doc);
                        if (retval.Length > 0)
                            html = html + retval;

                    }

                    dimensioncount = 0;
                    retval = PrintItem(dr["visibletext"].ToString(),
                        int.Parse(dr["ItemTypeKey"].ToString()),
                        bool.Parse(dr["AuthorityRequired"].ToString()),
                        0, dr["ChecklistTemplateItemCkey"].ToString(), doc);
                    if(retval.Length>0)
                        html = html + retval;

                }
                else
                {

                    dimensioncount++;
                    if (dimensioncount == 1)
                    {
                        saveckey = dr["ChecklistTemplateItemCkey"].ToString();
                        savetext = dr["visibletext"].ToString();
                        saverequired = dr["AuthorityRequired"].ToString();
                        saveitemtype = dr["ItemTypeKey"].ToString();
                    }
                }


            }

            return html;

        }

        public string PrintItem(string Text, int ItemType, bool Required, int dimension, string CKey, Document doc = null)
        {
            
           

           
            //hidden text has visibletext = "", do not print a blank line, just return           
            if (Text.Trim().Length == 0) // && ItemType != 23)
            { 
                return ""; 
            }
 
            //remove ? from the beginning
            if (Text.Trim().StartsWith("?"))
                Text = Text.Replace("?", "");

            bool VerticalSpacing = false;
            //identify a question fill-in that is a child of an answer fill-in

            if (Text == "Distance from Closest Margin" || Text == "Specify Margin, if possible")
            {
                int check =  ancestry[ancestry[CKey].ParentCKey].ItemType;
                
            }

            if(ItemType == 26 && ancestry[CKey].ParentCKey == lastparent)
            {
                combotext.Add(Text);
                return "";
            }

            if(ancestry[CKey].ParentCKey != lastparent) 
            {
                if(ancestry[CKey].PreviousSibling!="")
                {
                CKeyHierarchy lastkey = ancestry[ancestry[CKey].PreviousSibling];
                //reset
                combotext = new List<string>();
                }
            }

            lastparent = ancestry[CKey].ParentCKey;
            
            //do not print this note
            if (Text.Contains("Additional dimensions (repeat section if more than one part)"))
            {
                return "";
                
            }
            
           
            
            
            string html = "";
            string newhtml = "";
            bool isBold = false;
            Paragraph p = null;
            
            //if parent is required the child is required too although if it is mnot flagged as required in the model
            if (ancestry[CKey].ParentCKey != "")
            {
                if (ancestry.ContainsKey(ancestry[CKey].ParentCKey))
                {
                    if (ancestry[ancestry[CKey].ParentCKey].Required == true)
                        Required = true;
                }
                
            }

            int depth = ancestry[CKey].Depth;
            //string padding = "";
            string separator = new string('-', 60);
            string fillin = " " + new string('_', 15);
            //string htmlpadding = "";
            
            string parent = ancestry[CKey].ParentCKey;
            CKeyHierarchy test = null;
            if (parent != "" && ancestry.ContainsKey(parent))
            { test = ancestry[parent]; }

            int margin = 0;
            
            for (int i = 0; i <= depth; i++)
            {
               
                margin = margin + 14;
                
            }


            //add additional padding for each answer in the hierarchy (to account for ___ in the front)
            while(test!=null && test.ParentCKey!=null && test.ParentCKey!="")
            {
                if(test.ItemType==6)
                {
                    margin = margin + 50;
                }
                test = ancestry[test.ParentCKey];

            }
            
            //question - single select (40 and multi-select (23) and header sections are bold
            //but it looks like user may not want to make some questions bold but wait for more input
            if (ItemType == 4 || ItemType == 23 || ItemType==24 || ItemType ==17)   //12/23/2015: making QF bold as well
            
            {
                VerticalSpacing = true;
                if(ancestry.ContainsKey(ancestry[CKey].ParentCKey))
                {
                    if (ancestry[CKey].ParentCKey != "" &&
                    (ItemType == 17 || ItemType == 4) &&
                    (ancestry[ancestry[CKey].ParentCKey].ItemType == 20 || ancestry[ancestry[CKey].ParentCKey].ItemType == 6))
            
                    {
                        VerticalSpacing = false;
                    }
                }
                
                isBold = true;
               
            }
            else
            {
            
                isBold = false;
            }
            
            
            //item type specific logic
            switch (ItemType)
            {
                case 24:
                    //header

                    if (Required)
                    {
            
  
                        newhtml = newhtml +  Text;
                    }
                    else
                    {
 
                        newhtml = newhtml + "+ " + Text;
                    }
                    
                    break;

                case 4:  //question - single select
                    if (Required)
                    {
                        
                        newhtml = newhtml +  Text;
                    }
                    else
                    {
                        
                        newhtml = newhtml + "+" + Text;
                    }

                    break;
                case 6:   //answer

                   
                    if (Required)
                    {
            
                        newhtml = newhtml + "<div><span>___</span><span style='margin-left:20px'>" + Text + "</span></div>";
                    }
                    else
                    {
                        newhtml = newhtml + "<div><span>+ ___</span><span style='margin-left:30px'>" + Text + "</span></div>";

                    }

                    break;
                case 23:  //question - multiselect
                    if (Required)
                    {
 
                        newhtml = newhtml +  Text ;
                    }
                    else
                    {


                        newhtml = newhtml + "+ " + Text ;
                    }

                    break;
                case 20:  //answer fillin
                    //do not need fill-in space if Text does not contain () at the end 
                    if (!Text.EndsWith(")"))
                        fillin = "";
                    else
                    {

                        fillin = ": " + fillin;
                        if (Text.ToLower().Equals("not specified"))
                            fillin = "";
                        if (Text.Equals("Cannot be determined")) // VP fixed the bug since this was stopping to create fill-in for Cannot be determined (explain)
                            fillin = "";
                        if (Text.ToLower().Contains("not identified"))
                            fillin = "";
                        if (Text.ToLower().Contains("not applicable"))
                            fillin = "";
                        if (Text.ToLower().Contains("indeterminate"))
                            fillin = "";
                    }
                    if (Required)
                    {   
                        //flattent greatest dimension and additional dimension
                        //System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"(\(.+\))");
                        System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"(\([mm]*[cm]*[g]*)\)");
                        System.Text.RegularExpressions.MatchCollection collection = reg.Matches(Text);
                        string unit = "";
                        if (collection.Count == 1)
                        {
                            unit = collection[0].Value.Replace("(", "").Replace(")", "");
                        }

                        //greatest dimension is sometimes answer fill-in
                        if ((Text.ToLower().Contains("greatest dimension") || Text.Contains("Specify")) && unit!="")
                        {
                            fillin = ": ___";
                            for (int i = 1; i < dimension; i++)
                            {
                                fillin = fillin + " x ___";  
                            }
                            fillin = fillin + " " + unit;
                            if (unit != "")
                                Text = Text.Replace("(" + unit + ")", "");

                            //html = html + htmlpadding + "___ " + Text +  fillin;

                            newhtml = newhtml + "<div><span>___</span><span style='margin-left:20px'>" + Text + fillin + "</span></div>";
                        }
                        else
                        {
            

                            newhtml = newhtml + "<div><span>___</span><span style='margin-left:20px'>" + Text + fillin + "</span></div>";
                        }
                 
                        }
                        

                    
                    else
                    {
                           
                            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"(\([mm]*[cm]*[g]*)\)");
                            System.Text.RegularExpressions.MatchCollection collection1 = reg1.Matches(Text);
                            string unit1 = "";
                        if (collection1.Count == 1)
                        {
                            unit1 = collection1[0].Value.Replace("(", "").Replace(")", "");
                        }
                     
                        //greatest dimension is sometimes answer fill-in
                        if ((Text.ToLower().Contains("greatest dimension") || Text.Contains("Specify")) && unit1 != "")
                        {
                            fillin = ": ___";
                            for (int i = 1; i < dimension; i++)
                            {
                                fillin = fillin + " x ___";  
                            }
                            fillin = fillin + " " + unit1;
                            if (unit1 != "")
                                Text = Text.Replace("(" + unit1 + ")", "");


                            newhtml = newhtml + "<div><span>+ ___</span><span style='margin-left:30px'>" + Text + fillin + "</span></div>";
                        }
                        else
                        {
                            newhtml = newhtml + "<div><span>+ ___</span><span style='margin-left:30px'>" + Text + fillin + "</span></div>";
                        }
                    }

                    break;
                case 17:   //question fillin
                    //initialize fillin space
                    fillin = ": " + fillin;

                    //suppress fill-in space for these items
                    if (Text.ToLower().Equals("not specified"))
                            fillin = "";
                    if (Text.ToLower().Contains("cannot be determined"))
                            fillin = "";
                    if (Text.ToLower().Contains("not identified"))
                            fillin = "";
                    if (Text.ToLower().Contains("not applicable"))
                           fillin = "";
                    if (Text.ToLower().Contains("indeterminate"))
                            fillin = "";
                    if (Required)
                    {
                        System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"(\(.+\))");
                        System.Text.RegularExpressions.MatchCollection collection = reg.Matches(Text);
                        string unit = "";
                        if (collection.Count == 1)
                        {
                            unit = collection[0].Value.Replace("(", "").Replace(")", "");
                        }
                        
                        if ((Text.ToLower().Contains("greatest dimension") || Text.ToLower().Contains("additional dimension") || Text.ToLower().Contains("specify weight") || Text.ToLower().Contains("Specify")) && (dimension > 1 || unit!=""))
                        {
                            fillin = ": ___";
                            for (int i = 1; i < dimension; i++)
                            {
                                fillin = fillin + " x ___";  
                            }
                            fillin = fillin + " " + unit;
                            if (unit != "")
                                Text = Text.Replace("(" + unit + ")", "");
                        }
                        else
                        {

                            
                        }
                        
                        newhtml = newhtml +  Text + fillin ;
                    }
                    else
                    {
                        
                        System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"(\(.+\))");
                        System.Text.RegularExpressions.MatchCollection collection = reg.Matches(Text);
                        string unit = "";
                        if (collection.Count == 1)
                        {
                            unit = collection[0].Value.Replace("(", "").Replace(")", "");
                        }
                        if ((Text.ToLower().Contains("greatest dimension") || Text.ToLower().Contains("additional dimension")) && dimension > 1)
                        {
                            fillin = ": ___";
                            for (int i = 1; i < dimension; i++)
                            {
                                fillin = fillin + " x ___";
                            }
                            fillin = fillin + " " + unit;
                            
                            if (unit != "")
                                Text = Text.Replace("(" + unit + ")", "");
                            
                            newhtml = newhtml + "+ " + Text + fillin;
                        }
                        else
                        {

                            if (!Text.ToLower().Contains("comment(s)"))
                            {

                                newhtml = newhtml + "+ "  + Text + fillin;   //2/89/2018 - added left padding
                            }
                            else
                            {

                                newhtml = newhtml + "+ " + Text;    //2/89/2018 - added left padding
                            }
                        }
                        
                    }
                    break;
                
                case 12:  //note
                    
                    if(combotext.Count>0)
                    {
                    
                        foreach(string combo in combotext)
                        {

                            //html  = html + htmlpadding + combo + "<br/>";
                            
                        }
                      //italic
                        html = "<span style='Font-Style:Italic'>" + html + "</span>";
                        newhtml = newhtml +  html ;
                    }
            
                    newhtml = "<span style='Font-Style:Italic'>" + Text + "</span>";
                    break;
                case 26: //combo note

                    //combo notes appear among answer choices, user has requested to concatenate combo notes with the note that appear at the end
                    if (Required)
                    {
                        newhtml = newhtml +  Text ;
                    }
                    else
                    {
                        newhtml = newhtml +  Text;
                    }
                    break;
                default:

                    break;
            }
            if (isBold && VerticalSpacing)
            {

                newhtml = "<div style='margin-top:8px;margin-bottom:0px;margin-left:" + margin.ToString() + "px;" + "'><b>" + newhtml + "</b></div>";
            }
            else if (isBold)
            {
                newhtml = "<div style='margin-top:3px;margin-bottom:0px;margin-left:" + margin.ToString() + "px;" + "'><b>" + newhtml + "</b></div>";
            }
            else
            {
                newhtml = "<div style='margin-top:3px;margin-bottom:0px;margin-left:" + margin.ToString() + "px;" + "'>" + newhtml + "</div>";
            }
            return newhtml;
            

        }

        //public string PrintItemNew(string Text, int ItemType, bool Required, int dimension, string CKey, Document doc = null)
        //{
        //    //prints questions, answers and notes. Printing header sections also but users may want to suppress certain header sections but wait
        //    //for more instruction
        //    //if (ItemType == 24) return "";

        //    //hidden text has visibletext = "", do not print a blank line, just return           
        //    if (Text.Trim().Length == 0) // && ItemType != 23)
        //    {
        //        return "";
        //    }

        //    //remove ? from the beginning
        //    if (Text.Trim().StartsWith("?"))
        //        Text = Text.Replace("?", "");

        //    bool VerticalSpacing = false;
        //    //identify a question fill-in that is a child of an answer fill-in

        //    if (Text == "Distance from Closest Margin" || Text == "Specify Margin, if possible")
        //    {
        //        int check = ancestry[ancestry[CKey].ParentCKey].ItemType;

        //    }
        //    if (ItemType == 26 && ancestry[CKey].ParentCKey == lastparent)
        //    {
        //        combotext.Add(Text);
        //        return "";
        //    }

        //    if (ancestry[CKey].ParentCKey != lastparent)
        //    {
        //        if (ancestry[CKey].PreviousSibling != "")
        //        {
        //            CKeyHierarchy lastkey = ancestry[ancestry[CKey].PreviousSibling];
        //            //reset
        //            combotext = new List<string>();
        //        }
        //    }

        //    lastparent = ancestry[CKey].ParentCKey;
        //    //add links to notes in the html output only
        //    /*
        //    if (doc == null)
        //    {
                
        //        //check for notes
        //        System.Text.RegularExpressions.Regex rex = new System.Text.RegularExpressions.Regex(@"\(Notes? [A-Z].*\)");
        //        System.Text.RegularExpressions.Match match = rex.Match(Text);
        //        if(match.Success)
        //        {
        //            string pretext = Text.Substring(0, match.Index);
        //            string notetext = match.Value;
        //            rex = new System.Text.RegularExpressions.Regex("( [A-Z],?)");
        //            //rex = new System.Text.RegularExpressions.Regex("( [A-Z],?[^a-z])");
        //            //rex = new System.Text.RegularExpressions.Regex("( ([A-Z])[ ,)])");
        //            //rex = new System.Text.RegularExpressions.Regex("( [A-Z][^a-z])");
                    
        //            System.Text.RegularExpressions.MatchCollection collection = rex.Matches(notetext);
        //            foreach(System.Text.RegularExpressions.Match m in collection)
        //            {
        //                if(m.Value.Contains("N"))
        //                {
        //                    int check = m.Index;
        //                    if (match.Value.TrimStart().Substring(check + 2,1) != " ")
        //                        continue;
                            
        //                }
        //                else if (m.Value.Contains(","))
        //                {
                            
        //                    notetext = notetext.Replace(m.Value, "<a href='#note" + m.Value.ToLower().Trim().Replace(",", "") + "'>" + m.Value.Replace(",", "") + "</a>,");
        //                }
        //                else
        //                {
        //                    notetext = notetext.Replace(m.Value, "<a href='#note" + m.Value.ToLower().Trim() + "'>" + m.Value + "</a>");
        //                }
        //                //string replace = m.Groups[m.Groups.Count - 1].Value;
        //                //if (!m.Value.ToLower().Trim().EndsWith(")"))
        //                //    notetext = notetext.Replace(m.Value, "<a href='#note" + replace + "'> " + replace + "</a>");
        //                //else
        //                //    notetext = notetext.Replace(m.Value, "<a href='#note" + m.Value.Trim().Substring(0, 1) + "'> " + m.Value.Substring(0,m.Value.Length-1) + "</a>)");
        //            }
        //            Text = pretext + notetext;
        //        }
              
        //    }
        //    */

        //    //do not print this note
        //    if (Text.Contains("Additional dimensions (repeat section if more than one part)"))
        //    {
        //        return "";

        //    }




        //    string html = "";
        //    string newhtml = "";
        //    bool isBold = false;
        //    Paragraph p = null;

        //    //if parent is required the child is required too although if it is mnot flagged as required in the model
        //    if (ancestry[CKey].ParentCKey != "")
        //    {
        //        if (ancestry.ContainsKey(ancestry[CKey].ParentCKey))
        //        {
        //            if (ancestry[ancestry[CKey].ParentCKey].Required == true)
        //                Required = true;
        //        }

        //    }

        //    int depth = ancestry[CKey].Depth;
        //    string padding = "";
        //    string separator = new string('-', 60);
        //    string fillin = " " + new string('_', 15);
        //    string htmlpadding = "";
        //    float docindent = 0.0f;
        //    string parent = ancestry[CKey].ParentCKey;
        //    CKeyHierarchy test = null;
        //    if (parent != "" && ancestry.ContainsKey(parent))
        //    { test = ancestry[parent]; }

        //    int margin = 0;

        //    for (int i = 0; i <= depth; i++)
        //    {
        //        padding = padding + new string(' ', 4);
        //        htmlpadding = htmlpadding + "&nbsp;&nbsp;&nbsp;&nbsp;";
        //        margin = margin + 14;
        //        docindent = docindent + .04f;
        //    }

        //    if (Text.ToLower().Contains("extent of tumor"))
        //    {
        //        int check = 0;
        //    }
        //    if (doc != null)
        //    {
        //        p = doc.Paragraphs.Append();
        //        ParagraphProperties pp = doc.BeginUpdateParagraphs(p.Range);
        //        pp.LineSpacingType = ParagraphLineSpacing.Exactly;
        //        p.LineSpacing = 13;
        //        p.LeftIndent = DevExpress.Office.Utils.Units.InchesToDocumentsF(docindent);
        //    }

        //    //question - single select (40 and multi-select (23) and header sections are bold
        //    //but it looks like user may not want to make some questions bold but wait for more input
        //    if (ItemType == 4 || ItemType == 23 || ItemType == 24 || ItemType == 17)   //12/23/2015: making QF bold as well
        //    {
        //        VerticalSpacing = true;
        //        if (ancestry.ContainsKey(ancestry[CKey].ParentCKey))
        //        {
        //            if (ancestry[CKey].ParentCKey != "" &&
        //            (ItemType == 17 || ItemType == 4) &&
        //            (ancestry[ancestry[CKey].ParentCKey].ItemType == 20 || ancestry[ancestry[CKey].ParentCKey].ItemType == 6))
        //            //if (ancestry[CKey].ParentCKey != "" && (ItemType == 17 ) && (ancestry[ancestry[CKey].ParentCKey].ItemType == 20 || ancestry[ancestry[CKey].ParentCKey].ItemType == 6))
        //            {
        //                VerticalSpacing = false;
        //            }
        //        }

        //        if (doc != null)
        //        {
        //            //add extract vertical space
        //            if (VerticalSpacing)
        //                p = doc.Paragraphs.Append();

        //            p.LeftIndent = DevExpress.Office.Utils.Units.InchesToDocumentsF(docindent);
        //            ParagraphProperties pp = doc.BeginUpdateParagraphs(p.Range);
        //            pp.LineSpacingType = ParagraphLineSpacing.Exactly;
        //            p.LineSpacing = 13;
        //            CharacterProperties cp = doc.BeginUpdateCharacters(p.Range);
        //            cp.FontName = "Arial";
        //            cp.FontSize = 10;
        //            cp.Bold = true;
        //            cp.Italic = false;
        //            doc.EndUpdateCharacters(cp);
        //        }
        //        isBold = true;

        //    }
        //    else
        //    {
        //        if (doc != null)
        //        {

        //            CharacterProperties cp = doc.BeginUpdateCharacters(p.Range);
        //            cp.FontName = "Arial";
        //            cp.FontSize = 10;
        //            cp.Bold = false;
        //            if (ItemType == 12)
        //            {
        //                cp.Italic = true;
        //            }
        //            else
        //            {
        //                cp.Italic = false;
        //            }
        //            doc.EndUpdateCharacters(cp);
        //        }
        //        isBold = false;
        //    }


        //    //item type specific logic
        //    switch (ItemType)
        //    {
        //        case 24:
        //            //header

        //            if (Required)
        //            {
        //                if (doc != null)
        //                    //doc.AppendText(padding + Text);
        //                    doc.AppendText(Text);

        //                html = html + htmlpadding + Text;
        //                newhtml = newhtml + Text;
        //            }
        //            else
        //            {
        //                if (doc != null)
        //                    //doc.AppendText(padding + "+ " + Text);
        //                    doc.AppendText("+ " + Text);
        //                html = html + htmlpadding + "+ " + Text;
        //                newhtml = newhtml + "+ " + Text;
        //            }

        //            break;

        //        case 4:  //question - single select
        //            if (Required)
        //            {
        //                if (doc != null)
        //                    //doc.AppendText(padding + Text);
        //                    doc.AppendText(Text);
        //                html = html + htmlpadding + Text;
        //                newhtml = newhtml + Text;
        //            }
        //            else
        //            {
        //                if (doc != null)
        //                    //doc.AppendText(padding + "+ " + Text);
        //                    doc.AppendText("+ " + Text);
        //                html = html + htmlpadding + "+ " + Text;
        //                newhtml = newhtml + "+" + Text;
        //            }

        //            break;
        //        case 6:   //answer

        //            if (parent.Trim() != "")
        //                test = ancestry[parent];

        //            if (Required)
        //            {
        //                if (doc != null)
        //                    //doc.AppendText(padding + "___ " + Text);
        //                    doc.AppendText("___ " + Text);

        //                html = html + htmlpadding + "___ " + HtmlWrap(Text, 500);
        //                //newhtml = newhtml + "___ " + Text;
        //                newhtml = newhtml + "<div style='float:left;'>___</div><div style='margin-left:20px'>" + Text + "</div>";
        //            }
        //            else
        //            {

        //                if (doc != null)
        //                    //doc.AppendText(padding + "+ ___ " + Text);
        //                    doc.AppendText("+ ___ " + Text);

        //                html = html + htmlpadding + "+ ___ " + HtmlWrap(Text, 500);
        //                //newhtml = newhtml + "+" + " ___ " + Text ;
        //                newhtml = newhtml + "<div style='float:left;'>+ ___</div><div style='margin-left:30px'>" + Text + "</div>";

        //            }

        //            break;
        //        case 23:  //question - multiselect
        //            if (Required)
        //            {
        //                if (doc != null)
        //                    //doc.AppendText(padding + Text);
        //                    doc.AppendText(Text);

        //                html = html + htmlpadding + Text;
        //                newhtml = newhtml + Text;
        //            }
        //            else
        //            {

        //                if (doc != null)
        //                    //doc.AppendText(padding + "+ " + Text);
        //                    doc.AppendText("+ " + Text);

        //                html = html + htmlpadding + "+ " + Text;
        //                newhtml = newhtml + "+ " + Text;
        //            }

        //            break;
        //        case 20:  //answer fillin
        //            //do not need fill-in space if Text does not contain () at the end 
        //            if (!Text.EndsWith(")"))
        //                fillin = "";
        //            else
        //            {

        //                fillin = ": " + fillin;
        //                if (Text.ToLower().Equals("not specified"))
        //                    fillin = "";
        //                if (Text.Equals("Cannot be determined")) // VP fixed the bug since this was stopping to create fill-in for Cannot be determined (explain)
        //                    fillin = "";
        //                if (Text.ToLower().Contains("not identified"))
        //                    fillin = "";
        //                if (Text.ToLower().Contains("not applicable"))
        //                    fillin = "";
        //                if (Text.ToLower().Contains("indeterminate"))
        //                    fillin = "";
        //            }
        //            if (Required)
        //            {
        //                //flattent greatest dimension and additional dimension
        //                //System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"(\(.+\))");
        //                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"(\([mm]*[cm]*[g]*)\)");
        //                System.Text.RegularExpressions.MatchCollection collection = reg.Matches(Text);
        //                string unit = "";
        //                if (collection.Count == 1)
        //                {
        //                    unit = collection[0].Value.Replace("(", "").Replace(")", "");
        //                }

        //                //greatest dimension is sometimes answer fill-in
        //                if ((Text.ToLower().Contains("greatest dimension") || Text.Contains("Specify")) && unit != "")
        //                {
        //                    fillin = ": ___";
        //                    for (int i = 1; i < dimension; i++)
        //                    {
        //                        fillin = fillin + " x ___";
        //                    }
        //                    fillin = fillin + " " + unit;
        //                    if (unit != "")
        //                        Text = Text.Replace("(" + unit + ")", "");

        //                    if (doc != null)
        //                        //doc.AppendText(padding + "___ " + Text +  fillin);
        //                        doc.AppendText("___ " + Text + fillin);
        //                    html = html + htmlpadding + "___ " + Text + fillin;
        //                    //newhtml = newhtml +  "___ " + Text + fillin ;
        //                    newhtml = newhtml + "<div style='float:left;'>___</div><div style='margin-left:20px'>" + Text + fillin + "</div>";
        //                }
        //                else
        //                {
        //                    if (doc != null)
        //                        //doc.AppendText(padding + "___ " + Text + fillin);
        //                        doc.AppendText("___ " + Text + fillin);

        //                    html = html + htmlpadding + "___ " + Text + fillin;
        //                    //newhtml = newhtml +  "___ " + Text + fillin;
        //                    newhtml = newhtml + "<div style='float:left;'>___</div><div style='margin-left:20px'>" + Text + fillin + "</div>";
        //                }

        //            }



        //            else
        //            {

        //                System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"(\([mm]*[cm]*[g]*)\)");
        //                System.Text.RegularExpressions.MatchCollection collection1 = reg1.Matches(Text);
        //                string unit1 = "";
        //                if (collection1.Count == 1)
        //                {
        //                    unit1 = collection1[0].Value.Replace("(", "").Replace(")", "");
        //                }

        //                //greatest dimension is sometimes answer fill-in
        //                if ((Text.ToLower().Contains("greatest dimension") || Text.Contains("Specify")) && unit1 != "")
        //                {
        //                    fillin = ": ___";
        //                    for (int i = 1; i < dimension; i++)
        //                    {
        //                        fillin = fillin + " x ___";  // doc.AppendText(fillin + "X");
        //                    }
        //                    fillin = fillin + " " + unit1;
        //                    if (unit1 != "")
        //                        Text = Text.Replace("(" + unit1 + ")", "");

        //                    if (doc != null)
        //                        //doc.AppendText(padding + "+___ " + Text + fillin);
        //                        doc.AppendText("+___ " + Text + fillin);
        //                    html = html + htmlpadding + "+___ " + Text + fillin;
        //                    //newhtml = newhtml +  "+___ " + Text + fillin ;
        //                    newhtml = newhtml + "<div style='float:left;'>+ ___</div><div style='margin-left:30px'>" + Text + fillin + "</div>";
        //                }
        //                else
        //                {
        //                    if (doc != null)
        //                        //doc.AppendText(padding + "+___ " + Text + fillin);
        //                        doc.AppendText("+___ " + Text + fillin);
        //                    html = html + htmlpadding + "+___ " + Text + fillin;
        //                    //newhtml = newhtml +  "+___ " + Text + fillin ;
        //                    newhtml = newhtml + "<div style='float:left;'>+ ___</div><div style='margin-left:30px'>" + Text + fillin + "</div>";
        //                }
        //            }

        //            break;
        //        case 17:   //question fillin
        //            //initialize fillin space
        //            fillin = ": " + fillin;

        //            //suppress fill-in space for these items
        //            if (Text.ToLower().Equals("not specified"))
        //                fillin = "";
        //            if (Text.ToLower().Contains("cannot be determined"))
        //                fillin = "";
        //            if (Text.ToLower().Contains("not identified"))
        //                fillin = "";
        //            if (Text.ToLower().Contains("not applicable"))
        //                fillin = "";
        //            if (Text.ToLower().Contains("indeterminate"))
        //                fillin = "";
        //            if (Required)
        //            {
        //                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"(\(.+\))");
        //                System.Text.RegularExpressions.MatchCollection collection = reg.Matches(Text);
        //                string unit = "";
        //                if (collection.Count == 1)
        //                {
        //                    unit = collection[0].Value.Replace("(", "").Replace(")", "");
        //                }

        //                if ((Text.ToLower().Contains("greatest dimension") || Text.ToLower().Contains("additional dimension") || Text.ToLower().Contains("specify weight") || Text.ToLower().Contains("Specify")) && (dimension > 1 || unit != ""))
        //                {
        //                    fillin = ": ___";
        //                    for (int i = 1; i < dimension; i++)
        //                    {
        //                        fillin = fillin + " x ___";  // doc.AppendText(fillin + "X");
        //                    }
        //                    fillin = fillin + " " + unit;
        //                    if (unit != "")
        //                        Text = Text.Replace("(" + unit + ")", "");
        //                }
        //                else
        //                {

        //                    //doc.AppendText(Text + fillin);
        //                }

        //                if (doc != null)
        //                    //doc.AppendText(padding + Text + fillin);
        //                    doc.AppendText(Text + fillin);
        //                html = html + htmlpadding + Text + fillin;
        //                newhtml = newhtml + Text + fillin;
        //            }
        //            else
        //            {

        //                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"(\(.+\))");
        //                System.Text.RegularExpressions.MatchCollection collection = reg.Matches(Text);
        //                string unit = "";
        //                if (collection.Count == 1)
        //                {
        //                    unit = collection[0].Value.Replace("(", "").Replace(")", "");
        //                }
        //                if ((Text.ToLower().Contains("greatest dimension") || Text.ToLower().Contains("additional dimension")) && dimension > 1)
        //                {
        //                    fillin = ": ___";
        //                    for (int i = 1; i < dimension; i++)
        //                    {
        //                        fillin = fillin + " x ___";
        //                    }
        //                    fillin = fillin + " " + unit;

        //                    if (unit != "")
        //                        Text = Text.Replace("(" + unit + ")", "");

        //                    if (doc != null)
        //                        //doc.AppendText(padding + "+ " + Text + fillin);
        //                        doc.AppendText("+ " + Text + fillin);
        //                    html = html + htmlpadding + "+ " + Text + fillin;
        //                    newhtml = newhtml + "+ " + Text + fillin;
        //                }
        //                else
        //                {

        //                    if (doc != null)
        //                    {
        //                        if (!Text.ToLower().Contains("comment(s)"))
        //                            //doc.AppendText(padding + "+ " + Text + fillin);
        //                            doc.AppendText("+ " + Text + fillin);
        //                        else
        //                            //doc.AppendText(padding + "+ " + Text + fillin);
        //                            doc.AppendText("+ " + Text + fillin);
        //                    }
        //                    if (!Text.ToLower().Contains("comment(s)"))
        //                    {
        //                        html = html + htmlpadding + "+ " + Text + fillin;
        //                        newhtml = newhtml + "+ " + Text + fillin;
        //                    }
        //                    else
        //                    {
        //                        html = html + htmlpadding + "+ " + Text + fillin;
        //                        newhtml = newhtml + "+ " + Text;
        //                    }
        //                }

        //            }
        //            break;

        //        case 12:  //note

        //            if (combotext.Count > 0)
        //            {

        //                foreach (string combo in combotext)
        //                {

        //                    if (doc != null)
        //                    {
        //                        //doc.AppendText(padding + combo);
        //                        doc.AppendText(combo);
        //                        p = doc.Paragraphs.Append();

        //                    }
        //                    html = html + htmlpadding + combo + "<br/>";

        //                }
        //                //italic
        //                html = "<span style='Font-Style:Italic'>" + html + "</span>";
        //                newhtml = newhtml + html;
        //            }
        //            if (Required)
        //            {
        //                if (doc != null)
        //                    //doc.AppendText(padding + Text);
        //                    doc.AppendText(Text);

        //            }
        //            else
        //            {
        //                if (doc != null)
        //                    //doc.AppendText(padding + Text);
        //                    doc.AppendText(Text);
        //            }
        //            html = html + htmlpadding + Text;
        //            html = "<span style='Font-Style:Italic'>" + html + "</span>";
        //            newhtml = "<span style='Font-Style:Italic'>" + Text + "</span>";
        //            break;
        //        case 26: //combo note

        //            //combo notes appear among answer choices, user has requested to concatenate combo notes with the note that appear at the end
        //            if (Required)
        //            {
        //                if (doc != null)
        //                    //doc.AppendText(padding + Text);
        //                    doc.AppendText(Text);
        //                html = html + htmlpadding + Text;
        //                newhtml = newhtml + Text;
        //            }
        //            else
        //            {
        //                if (doc != null)
        //                    //doc.AppendText(padding + "+ " + Text);
        //                    doc.AppendText("+ " + Text);
        //                html = html + htmlpadding + "+ " + Text;
        //                newhtml = newhtml + Text;
        //            }
        //            break;
        //        default:
        //            //if (Required)
        //            //doc.AppendText("Error:" + padding + ItemType.ToString() + " - " + Text);
        //            //else
        //            //doc.AppendText("Error:" + padding + "+" + ItemType.ToString() + " - " + Text);

        //            break;
        //    }
        //    if (isBold && VerticalSpacing)
        //    {

        //        html = "<p style='margin-top:8px;margin-bottom:0px;'><b>" + html + "</b></p>";
        //        newhtml = "<div style='margin-top:8px;margin-bottom:0px;margin-left:" + margin.ToString() + "px;" + "'><b>" + newhtml + "</b></div>";
        //    }
        //    else if (isBold)
        //    {
        //        html = "<b>" + html + "</b><br />";
        //        newhtml = "<div style='margin-top:3px;margin-bottom:0px;margin-left:" + margin.ToString() + "px;" + "'><b>" + newhtml + "</b></div>";
        //    }
        //    else
        //    {
        //        //likely to overflow
        //        //split on ___
        //        html = html + "<br />";
        //        newhtml = "<div style='margin-top:3px;margin-bottom:0px;margin-left:" + margin.ToString() + "px;" + "'>" + newhtml + "</div>";
        //    }
        //    return newhtml;
        //    //return html;

        //}
        private string HtmlWrap(string Text, int width)
        {
            return "<span style='display:inline-block;width:" + width.ToString() + "px;vertical-align:top;word-wrap:break-word;'>" + Text + "</span>";
        }
    }
    }

