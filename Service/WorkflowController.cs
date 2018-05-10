using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SSPWebUI.Data;

namespace SSPWebUI.Service
{

    public class Workflowtatus
    {
        public string Status { get; set; }
    }

    public class WorkflowController : ApiController
    {
        // GET api/<controller>
        [HttpPut]
        public void SubmitDraft(EditSubmitRequest req)
        {
         try
         {
             new Workflow().SubmitDraft(decimal.Parse(req.ProtocolCKey),  decimal.Parse(req.AuthorCKey));
         }
            catch(Exception ex)
         {
             var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
             response.Content = new StringContent(ex.Message);

             throw new HttpResponseException(response);
         }
            
           
        }

        [HttpPut]
        public void SubmitReview(ReviewSubmitRequest req)
        {

            new Workflow().SubmitReview(decimal.Parse(req.ProtocolCKey), decimal.Parse(req.DraftVersion), decimal.Parse(req.ReviewerCKey));

        }

        [HttpGet]
        public List<Data.ProtocolStatus> GetWorkflowStatus(string ProtocolCKey)
        {
            
            List<Data.ProtocolStatus> statuses = new Workflow().GetWorkflowProtocolStatus(decimal.Parse(ProtocolCKey));
          
            if(statuses.Count==0)  //work has not started yet
            {
                Data.ProtocolStatus s = new ProtocolStatus();
                s.DraftVersion = "" ;
                s.EditorCKey = 0;
                s.ProtocolCKey = decimal.Parse(ProtocolCKey);
                s.Status = "";
                statuses.Add(s); 
            }
            
            return statuses;
        }

        [HttpGet]
        public ReviewStatus GetWorkflowReviewStatus(string ProtocolCKey, string DraftVersion, string ReviewerCKey)
        {
           
            ReviewStatus  status = new Workflow().GetWorkflowProtocolReviewStatus(decimal.Parse(ProtocolCKey), decimal.Parse(DraftVersion), decimal.Parse(ReviewerCKey));
                        
            if(status==null)
            {
                status = new ReviewStatus();
                status.ProtocolCKey=decimal.Parse(ProtocolCKey);
                status.Status = "0";
            }
            return status;
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}