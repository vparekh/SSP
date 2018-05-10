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

    public class ProtocolGroup
    {
        public string code { get; set; }
        public string label { get; set; }
    }
    public class ProtocolGroupController : ApiController
    {
        
        // GET api/<controller>
        public List<ProtocolGroup> Get()
        {            
           DataTable dt = ProtocolsData.GetProtocolGroups();
           List<ProtocolGroup> protocols = new List<ProtocolGroup>();
            foreach(DataRow dr in dt.Rows)
            {
                ProtocolGroup p = new ProtocolGroup();
                p.code = dr["ProtocolGroupCKey"].ToString();
                p.label = dr["ProtocolGroup"].ToString();
                protocols.Add(p);
            }
            return protocols;
        }


        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
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