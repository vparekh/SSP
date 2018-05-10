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
    public class SubmitRequest
    {
        public string ProtocolCKey { get; set; }  //ckey
        public string ProtocolVersionCKey { get; set; }
        public string ProtocolName { get; set; }
        public string ProtocolGroupCKey { get; set; }  //groupckey
        public string ProtocolVersion { get; set; }  //user entered version
        public string UserCkey { get; set; }  //user who created this protocol
        public string CloneChecklistCKey { get; set; }    //clone cjecklist when creating new protocol
        public string Action { get; set; }
        //public string ProtocolOldName { get; set; }
        //public string ProtocolOldVersion { get; set; }
    }

    public class ProtocolVersion
    {
        public string ProtocolCKey { get; set; }
        public string ProtocolVersionCKey { get; set; }
        public string ProtocolVersionLabel { get; set; }
    }
    public class DraftVersion
    {
        public string Version { get; set; }
    }
    public class ProtocolsController : ApiController
    {

        // GET api/<controller>
        public List<SubmitRequest> Get()
        {
            DataTable dt = ProtocolsData.GetProtocols();
            List<SubmitRequest> protocols = new List<SubmitRequest>();
            foreach (DataRow dr in dt.Rows)
            {
                SubmitRequest p = new SubmitRequest();
                p.ProtocolCKey = dr["protocolckey"].ToString();
                p.ProtocolName = dr["protocolname"].ToString();
                protocols.Add(p);

            }
            return protocols;
        }

        
        public List<DraftVersion> GetDraftVersions(string ProtocolVersion)
        {
            List<DraftVersion> versions = new List<DraftVersion>();
            DataTable dt = ProtocolsData.GetDraftVersions(decimal.Parse(ProtocolVersion));
            foreach(DataRow dr in dt.Rows)
            {
                DraftVersion obj = new DraftVersion();
                obj.Version = dr["DraftVersion"].ToString();
                versions.Add(obj);
            }
            return versions;
        }

        public List<ProtocolVersion> GetProtocolVersions(string ProtocolCKey)
        {
            
            DataTable dt = ProtocolsData.GetProtocolVersions(decimal.Parse(ProtocolCKey));
            List<ProtocolVersion> versions = new List<ProtocolVersion>();
            ProtocolVersion obj;
            foreach (DataRow dr in dt.Rows)
            {
                obj = new ProtocolVersion();
                obj.ProtocolVersionLabel = dr["ProtocolVersion"].ToString();
                obj.ProtocolVersionCKey = dr["ProtocolVersionCKey"].ToString();
                obj.ProtocolCKey = ProtocolCKey;
                versions.Add(obj);
            }

            obj = new ProtocolVersion();
            obj.ProtocolCKey = ProtocolCKey;
            obj.ProtocolVersionCKey = "0";
            obj.ProtocolVersionLabel = "--New Version--";
            versions.Add(obj);
            return versions;
        }

        public string GetProtocolName(string ProtocolVersionCKey)
        {
          string name =  ProtocolsData.GetProtocolName(decimal.Parse(ProtocolVersionCKey));
          return name;
        }

        public List<SubmitRequest> Get(string ProtocolGroup)
        {
            DataTable dt = ProtocolsData.GetProtocols(decimal.Parse(ProtocolGroup)); ;
            List<SubmitRequest> protocols = new List<SubmitRequest>();
            foreach (DataRow dr in dt.Rows)
            {
                SubmitRequest p = new SubmitRequest();
                p.ProtocolCKey = dr["protocolckey"].ToString();
                p.ProtocolName = dr["protocolname"].ToString();
                protocols.Add(p);

            }
            return protocols;
        }

        public List<SubmitRequest> GetProtocolsByGroup(string ProtocolGroup)
        {
            DataTable dt = ProtocolsData.GetProtocolsTableData(decimal.Parse(ProtocolGroup)); ;
            List<SubmitRequest> protocols = new List<SubmitRequest>();
            SubmitRequest p = new SubmitRequest();
            foreach (DataRow dr in dt.Rows)
            {
                p = new SubmitRequest();
                p.ProtocolCKey = dr["protocolckey"].ToString();
                p.ProtocolName = dr["protocolname"].ToString();
                protocols.Add(p);

            }
            p = new SubmitRequest();
            p.ProtocolName = "--New Protocol--";
            p.ProtocolCKey = "0";
            protocols.Add(p);
            return protocols;
        }
        // GET api/<controller>/5
       

        // POST api/<controller>
        public void UpdateProtocol(SubmitRequest p)
        {
            try
            {
                if (p.Action == "addprotocol")  //new protocol
                {
                    if (!new Data.ProtocolsData().ProtocolNameExists(p.ProtocolName))
                    {
                        new Data.ProtocolsData().AddProtocol(decimal.Parse(p.ProtocolGroupCKey), p.ProtocolVersion, decimal.Parse(p.UserCkey), p.ProtocolName, decimal.Parse(p.CloneChecklistCKey));
                    }
                    else
                    {
                        var resp = new HttpResponseMessage(HttpStatusCode.ExpectationFailed)

                        {

                            ReasonPhrase = "Protocol Name: " + p.ProtocolName + " already exists. Please select a differeent name."

                        };

                        throw new HttpResponseException(resp);
                    }
                }

                else if (p.Action == "addversion")  //new version
                {
                    if (!new Data.ProtocolsData().ProtocolVersionExists(decimal.Parse(p.ProtocolCKey), p.ProtocolVersion))
                    {
                        new Data.ProtocolsData().AddProtocol(decimal.Parse(p.ProtocolGroupCKey), p.ProtocolVersion, decimal.Parse(p.UserCkey), p.ProtocolName, decimal.Parse(p.CloneChecklistCKey));
                    }

                }
                else if (p.Action == "update") //update
                {
                    //rename version/protocolname
                    new Data.ProtocolsData().UpdateProtocolName(decimal.Parse(p.ProtocolCKey), p.ProtocolName);
                    new Data.ProtocolsData().UpdateProtocolVersion(decimal.Parse(p.ProtocolVersionCKey), p.ProtocolVersion);
                }
                else if (p.Action == "deleteprotocol")
                {
                    new Data.ProtocolsData().DeleteProtocol(decimal.Parse(p.ProtocolCKey));
                }
           
            }
            catch (Exception ex)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.ExpectationFailed)

                {

                    ReasonPhrase = ex.Message

                };

                throw new HttpResponseException(resp);

            }
            
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