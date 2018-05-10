using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SSPWebUI.Data;

namespace SSPWebUI.Service
{
    public class UserPassword
    {
        public string UserID { get; set; }
        public string Password { get; set; }
    }
    public class PasswordController : ApiController
    {
        [HttpPut]
        [ActionName("SetPassword")]  //PUT
        public void SetPassword(UserPassword Pwd)
        {
            try
            {
                SSPUser.UpdatePassword(Pwd.UserID, Pwd.Password);
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