using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SSPWebUI.Data;
using System.Data;

namespace SSPWebUI.Service
{
   
    public class ChecklistItem
    {
        public string CKey { get; set; }
        public string VisibleText { get; set; }
        public int ItemType { get; set; }
        public int AnswerUnit { get; set; }
        public int SelectionDisablesChildren { get; set; }
        public int SelectionDisablesSiblings { get; set; }
        public int MustImplement { get; set; }
        public int MustAnswer { get; set; }
        public int ReportOption { get; set; }
        public string Notes { get; set; }
        public string NotesHyperlinked { get; set; }
        public string NoteCKeys { get; set; }
        public string ProtocolVersionCKey { get; set; }
        public string Comments { get; set; }
        public int RequiredStatus { get; set; }
        public string Condition { get; set; }
    }
    public class ChecklistController : ApiController
    {
        // GET api/<controller>
        public List<Checklist> GetChecklists()
        {
            try
            {
                return ProtocolsData.GetChecklists();
            }
            catch(Exception ex)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.ExpectationFailed)

                {

                    ReasonPhrase = ex.Message

                };

                throw new HttpResponseException(resp);

            }
            

           
        }

        // GET api/<controller>/5
        public List<Checklist> Get(string ProtocolCKey)
        {

            try
            {
                if (ProtocolCKey != null)
                    return ProtocolsData.GetChecklists(decimal.Parse(ProtocolCKey));
                else
                    return null;
            }
            catch(Exception ex)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.ExpectationFailed)

                {

                    ReasonPhrase = ex.Message

                };

                throw new HttpResponseException(resp);
            }
            

            
        }

        public ChecklistItem GetChecklistItem(string ItemCKey, int Mode, decimal DraftVersion)
        {

            try
            {
                ChecklistItem item = new ChecklistItem();


                item.CKey = "";
                item.ItemType = 0;
                item.VisibleText = "";
                item.AnswerUnit = 0;
                item.MustAnswer = 0;
                item.MustImplement = 0;
                item.SelectionDisablesChildren = 0;
                item.SelectionDisablesSiblings = 0;
                item.Notes = "";
                item.NotesHyperlinked = "";
                item.NoteCKeys = "";
                item.ReportOption = 2;
                item.Comments = "";
                item.Condition = "";

                if (ItemCKey == "null" || ItemCKey == "*" || ItemCKey == "" || ItemCKey == null)
                {
                    return item;
                }

                ItemCKey = ItemCKey.Replace("*", "");
                DataTable dt = ChecklistTemplateItems.GetCheckListItem(decimal.Parse(ItemCKey), Mode, DraftVersion);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];

                    item.CKey = dr["ChecklistTemplateItemCkey"].ToString();


                    //comboboxnote (26) to note (12) as users don't want to distinguish between the two
                    if (int.Parse(dr["ItemTypeKey"].ToString()) != 26)
                    {
                        item.ItemType = int.Parse(dr["ItemTypeKey"].ToString());
                    }
                    else
                    {
                        item.ItemType = 12;
                    }



                    item.VisibleText = dr["VisibleText"].ToString();
                    item.AnswerUnit = int.Parse(dr["AnswerUnits_Id"].ToString());
                    item.MustAnswer = int.Parse(dr["MustAnswer"].ToString());
                    item.MustImplement = int.Parse(dr["MustImplement"].ToString());
                    item.SelectionDisablesChildren = int.Parse(dr["SelectionDisablesChildren"].ToString());
                    item.SelectionDisablesSiblings = int.Parse(dr["SelectionDisablesSiblings"].ToString());
                    item.Notes = dr["NotesAlt"].ToString();
                    item.NotesHyperlinked = dr["Notes"].ToString();
                    item.NoteCKeys = dr["NoteCKeys"].ToString();
                    item.RequiredStatus = dr["Authorityrequired"] == System.DBNull.Value ? 1 : int.Parse(dr["Authorityrequired"].ToString());
                    item.ReportOption = dr["showInReport_Id"] == System.DBNull.Value ? 2 : int.Parse(dr["showInReport_Id"].ToString());
                    item.Comments = dr["comments"] == System.DBNull.Value ? "" : dr["comments"].ToString();
                    item.Condition = dr["condition"].ToString();
                }

                return item;

            }
            catch(Exception ex)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.ExpectationFailed)

                {

                    ReasonPhrase = ex.Message

                };

                throw new HttpResponseException(resp);
            }
            
            
        }

        // POST api/<controller>
        //[HttpPost]
        //public void Add(string visibletext, string ChecklistTemplateVersionCKey, string ProtocolVersionCKey, 
        //    string ParentCKey)
        //{
        //    ChecklistItem item = new ChecklistItem();
            
        //    ChecklistTemplateItems.AddChecklistItem(decimal.Parse(ChecklistTemplateVersionCKey),
        //        decimal.Parse(ProtocolVersionCKey), decimal.Parse(ParentCKey), visibletext, null, null, null, null, null, null, null, null, null,
        //        null, 0, "", null, null, null,null);
        //}

        // PUT api/<controller>/5
        [HttpPut]
        public void Update(ChecklistItem item)
        {

            try
            {
                ChecklistTemplateItems.UpdateChecklistItemEx(decimal.Parse(item.CKey),
               item.VisibleText, item.ItemType, item.SelectionDisablesChildren,
               item.SelectionDisablesSiblings, item.AnswerUnit, decimal.Parse(item.ProtocolVersionCKey), item.NoteCKeys, item.Condition,
               item.MustImplement, item.MustAnswer, item.ReportOption, item.RequiredStatus, item.Comments);

            }
            catch(Exception ex)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.ExpectationFailed)

                {

                    ReasonPhrase = ex.Message

                };

                throw new HttpResponseException(resp);
            }
           
            
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}