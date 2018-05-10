using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SSPWebUI.Data;

namespace SSPWebUI.Service
{
    public class EditSubmitRequest
    {
        public string ProtocolCKey { get; set; }
        public string DraftVersion { get; set; }
        public string AuthorCKey { get; set; }
        public string Status { get; set; }
    }

    //public class EditStatus
    //{
    //    public string Status { get; set; }
    //}

    public class EditStatusController : ApiController
    {
        [HttpPut]
        public void UpdateStatus(EditSubmitRequest req)
        {
            try
            {
                new Workflow().UpdateEditStatus(decimal.Parse(req.ProtocolCKey), decimal.Parse(req.AuthorCKey), req.Status);
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

        //[HttpGet]
        //public EditStatus GetEditStatus(string ProtocolCKey)
        //{
        //    string retval = "C"; 
        //    List<Data.EditStatus> statuses =   new Workflow().GetEditStatus(decimal.Parse(ProtocolCKey));
        //    foreach(Data.EditStatus status in statuses)
        //    {
        //        if (status.Status!="C")
        //        {
        //            retval = "S";
        //        }
        //    }
         
        //    return new EditStatus { Status = retval };
          
        //}
    }
}