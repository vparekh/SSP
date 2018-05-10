using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SSPWebUI.Data;

namespace SSPWebUI.Service
{
    
    public class BaseVersionController : ApiController
    {
        // GET api/<controller>
        public List<BaseVersion> GetVersions()
        {
            BaseVersion bv = new BaseVersion();
            try
            {
                List<BaseVersion> versions = bv.getAllVersions();
                return versions;
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