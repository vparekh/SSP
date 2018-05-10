using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SSPWebUI.Data;

namespace SSPWebUI.Service
{
    public class ProcedureUpdateRequest
    {
        public string ProtocolCKey { get; set; }
        public string UserCKey { get; set; }
        public string Html { get; set; }
    }
    public class ProcedureController : ApiController
    {
        // GET api/<controller>
        public string GetProcedure(string ProtocolCKey, string Mode, string DraftVersion)
        {
            try
            {
                if (DraftVersion == null)
                    DraftVersion = "1";
                if (ProtocolCKey != null)
                    return new CoverPage().GetProcedure(decimal.Parse(ProtocolCKey), int.Parse(Mode), decimal.Parse(DraftVersion)).ProcedureDetails;
                else
                    return "";
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
        public void Post([FromBody]string value)
        {
        }

        [HttpPut]
        public void UpdateProcedure(ProcedureUpdateRequest req)
        {
            try
            {
                new CoverPage().saveProcedure(decimal.Parse(req.ProtocolCKey), decimal.Parse(req.UserCKey), req.Html);
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