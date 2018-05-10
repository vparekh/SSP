using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SSPWebUI.Data;

namespace SSPWebUI.Service
{
    public class ReferenceController : ApiController
    {
        public class ReferenceSeq
        {
            public string CKeys { get; set; }
        }
        public class ReferenceDTO
        {
            public string Number { get; set; }
            public string ReferenceCKey { get; set; }
            public string NoteCKey { get; set; }
            public string ReferenceContent { get; set; }
            public string ProtocolVersionCKey { get; set; }
            public string UserCKey { get; set; }
        }
        
       

        public List<Reference> GetReference(string NoteCKey, string Mode, string DraftVersion )
        {
            if (NoteCKey != null)
                return new Reference().GetReferences(decimal.Parse(NoteCKey), int.Parse(Mode), decimal.Parse(DraftVersion));
            else
                return null;
        }

        // POST api/<controller>
        public void SaveReference(ReferenceDTO value)
        {
            if(value.ReferenceContent!=null)
            {
                string test = value.ReferenceContent;
                string refno = "0";
                if (value.Number.Length > 1)
                {
                    refno = value.Number;

                }
                new Reference().saveReferences(decimal.Parse(value.ProtocolVersionCKey), decimal.Parse(value.NoteCKey), 
                    decimal.Parse(value.ReferenceCKey), int.Parse(value.Number), 
                    value.ReferenceContent, decimal.Parse(value.UserCKey));
            }
          
        }
        
        //[Route("Delete/{id}/{tag}")]
        public int DeleteReference(string NoteReferenceCKey)
        {
            //tag is noteckey
            Reference refer = new Reference();
            refer.DeleteReference(decimal.Parse(NoteReferenceCKey));

            //must return a value, otherwise ajax DELETE does not work
            return 200;
        }
    }
}