using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SSPWebUI.Data;
namespace SSPWebUI.Service
{
    public class ProtocolHeaderController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public ProtocolHeader GetHeader(decimal ProtocolCKey, int Mode, decimal DraftVersion)
        {
            try
            {
                ProtocolHeader hdr = new ProtocolHeaderData().Get(ProtocolCKey, Mode, DraftVersion);

                return hdr;
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

        /*
        public List<CoverPageCategory> GetCoverPageCategories()
        {
            return new CoverPage().GetCoverPageCategories();
        }

        public List<CoverPageUsage> GetCoverPageUsages()
        {
            return new CoverPage().GetCoverPageUsages();
        }

        public List<CoverPageData> GetCoverPageData(string ProtocolVersionCKey, string DraftVersion)
        {
            return new CoverPage().GetCoverPageData(decimal.Parse(ProtocolVersionCKey), decimal.Parse(DraftVersion));

        }
        */
         [HttpPost]
        public string Update(ProtocolHeader headerdata)
        {
            try
            {
                ProtocolHeaderData hdr = new ProtocolHeaderData();
                hdr.Save(headerdata);

                return "data updated";
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