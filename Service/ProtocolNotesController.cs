using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SSPWebUI.Data;

namespace SSPWebUI.Service
{
    public class NotesSeq
    {
        public string CKeys { get; set; }
    }
    public class NoteTitle
    {
        public string ProtocolCKey { get; set; }
        public string UserCKey { get; set; }
        public string NoteCKey { get; set; }
        public string Title { get; set; }
    }

    public class ProtocolNotesController : ApiController
    {
       
        
        //[Route("GetTags/{id}")]
        public List<string> GetTags(decimal id)
        {
            try
            {
                Note note = new Note();
                return note.getTags(id, 1, 1);
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

        public List<Note> GetNotes(string ProtocolID, string Mode, string DraftVersion)
        {
            try
            {
                return new Note().getProtocolNotes(decimal.Parse(ProtocolID), int.Parse(Mode), decimal.Parse(DraftVersion));
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

        public Note GetNote(decimal id)
        {
            try
            {
                return new Note().GetNote(id, 1, 1);
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

        public string GetMergedNotes(string ProtocolVersionCKey, string DraftVersion)
        {
            try
            {
                return new Note().GetMergedNotes(decimal.Parse(ProtocolVersionCKey), decimal.Parse(DraftVersion));
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
        //public Note GetNote(string protocolid, string tag)
        //{
        //    return new Note().GetNote(decimal.Parse(protocolid), tag);
        //}

        [HttpPut]
        [ActionName("Resequence")]
        public string Resequence(NotesSeq CKeys)
        {
            try
            {
                new Note().Resequence(CKeys.CKeys);
                return "success";

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
        [HttpPost]
        [ActionName("CreateUpdateNote")]
        public string CreateUpdateNote(NoteTitle Title)
        {
            try
            {
                if (Title.NoteCKey == null)
                {
                    decimal noteckey = new Note().CreateNote(decimal.Parse(Title.ProtocolCKey), decimal.Parse(Title.UserCKey), Title.Title);
                    return noteckey.ToString();
                }
                else
                {
                    new Note().UpdateNoteTitle(decimal.Parse(Title.NoteCKey), decimal.Parse(Title.UserCKey), Title.Title);
                    return Title.NoteCKey;
                }

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

              

        [Route("Delete/{id}")]
        public List<string> Delete(decimal id)
        {
            try
            {
                Note note = new Note();
                return note.DeleteNote(id);
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
        
    }
}