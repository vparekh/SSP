using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SSPWebUI.Data;

namespace SSPWebUI.Service
{
    public class AuthorController : ApiController
    {
        // GET api/<controller>
        public List<Author> GetAll(string ProtocolVersion, string Mode, string DraftVersion)

        {
            if (ProtocolVersion == null)
                ProtocolVersion = "0";
            try
            {
                return new Author().getAuthors(decimal.Parse(ProtocolVersion), int.Parse(Mode), decimal.Parse(DraftVersion));
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

        
        [HttpGet]
        public List<SSPUser> GetAllAuthors()  //for dropdown
        {

            try
            {
                return SSPUser.getAllAuthors();
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
        // PUT api/<controller>/5
        public void UpdateAuthors(string ProtocolVersion, string author, string role, string protocolauthorroleckey, string userckey, Author auth)
        {
            //UpdateAuthors?ProtocolVersion
            try
            {
                new Author().updateProtocolAuthorRole(decimal.Parse(ProtocolVersion), decimal.Parse(protocolauthorroleckey), decimal.Parse(author), decimal.Parse(role), decimal.Parse(userckey));
            }
            catch(Exception ex)
            {

                var resp = new HttpResponseMessage(HttpStatusCode.ExpectationFailed)

                {

                    ReasonPhrase =  ex.Message

                };

                throw new HttpResponseException(resp);
                
            }
            
            //new Author().deleteAuthor(decimal.Parse(protocolauthorroleckey));
            //new Author().addAuthor(decimal.Parse(author), decimal.Parse(role), decimal.Parse(ProtocolVersion), decimal.Parse(userckey), );            
        }

        [HttpDelete]
        // DELETE api/<controller>/5
        public void DeleteAuthorRole(string protocolauthorroleckey)
        {
            try
            {
                new Author().deleteAuthor(decimal.Parse(protocolauthorroleckey));
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
        public void AddAuthorRole(string ProtocolVersion, string author, string role, string userckey )
        {  
            try
            {
                new Author().addAuthor(decimal.Parse(author), decimal.Parse(role), decimal.Parse(ProtocolVersion), decimal.Parse(userckey));
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