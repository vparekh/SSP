using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SSPWebUI.Data;

namespace SSPWebUI.Service
{
    public class ReviewSubmitRequest
    {
        public string ProtocolCKey { get; set; }
        public string ReviewerCKey { get; set; }
        public string Status { get; set; }
        public string DraftVersion { get; set; }
    }
    public class ReviewStatusController : ApiController
    {
        // PUT api/<controller>/5
        [HttpPut]
        public void UpdateStatus(ReviewSubmitRequest req)
        {           
           
            new Workflow().UpdateReviewStatus(decimal.Parse(req.ProtocolCKey), decimal.Parse(req.DraftVersion), decimal.Parse(req.ReviewerCKey), req.Status);
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}