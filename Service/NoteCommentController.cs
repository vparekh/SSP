using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//using SSPWebUI.Model;
using SSPWebUI.Data;

namespace SSPWebUI.Service
{
    
    public class NoteCommentController : ApiController
    {
        // GET api/<controller>
        public NoteComment GetComment(decimal NoteCommentCKey, string DraftVersion)
        {
            try
            {
                SSPWebUI.Data.NoteCommentData db = new Data.NoteCommentData();
                SSPWebUI.Data.NoteComment retval = db.GetComment(NoteCommentCKey);
                if (retval == null)
                {
                    var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    response.Content = new StringContent("No comment with notecommentckey = " + NoteCommentCKey + " found.");

                    throw new HttpResponseException(response);


                }
                return retval;

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

        

        [HttpPost]
        public string AddComment(Data.NoteComment value)
        {
            try
            {
                string version = value.Version;
                Data.NoteCommentData db = new Data.NoteCommentData();


                string ckey = db.Add(value);

                return ckey;
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

        
        [HttpPut]
        public HttpResponseMessage UpdateComment(Data.NoteComment value)
        {
            Data.NoteCommentData db = new Data.NoteCommentData();
            string version = value.Version;
            try
            {
                db.Update(value,decimal.Parse(value.Version));
            }
            catch(Exception ex)
            {
                HttpError error = new HttpError("UpdateComment: " + ex.Message);
                return this.Request.CreateErrorResponse(
                           HttpStatusCode.BadRequest,
                           error);
            }
            


            HttpResponseMessage msg = new HttpResponseMessage();
            msg.ReasonPhrase = "Comment updated.";

            return this.Request.CreateResponse(
                           HttpStatusCode.OK,
                           msg);

            
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}