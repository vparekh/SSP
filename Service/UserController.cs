using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SSPWebUI.Data;

namespace SSPWebUI.Service
{
    public class UserController : ApiController
    {
        // GET api/<controller>
        public List<SSPUser> GetUsers()
        {
           return SSPUser.getAll();
          
        }

        // GET api/<controller>/5
        public SSPUser GetUserById(string UserID)
        {
            return SSPUser.GetUserById(UserID);
            
        }

        // POST api/<controller> -- create
        [HttpPost]
        [ActionName("Create")]  //POST
        public HttpResponseMessage Post(SSPUser user)
        {
            //check if user exists
            if(SSPUser.GetUserById(user.UserID).UserID!=null)
            {
                var message = string.Format("User with UserID = {0} already exists", user.UserID);
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.NotFound, err);
            }
            else
            {
                SSPUser.CreateUser(user);
                
                var message = string.Format("User with UserID = {0} already exists", user.UserID);
                //HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.OK,"User created");
            }
            
        }

        // PUT api/<controller>/5 - update
        [HttpPut]
        [ActionName("Update")]  //PUT
        public void Put(SSPUser user)
        {
            SSPUser.UpdateUser(user);
        }

       

        // DELETE api/<controller>/5
        public int Delete(string UserID)
        {
            SSPUser.DeleteUser(UserID);
            //must return a value, otherwise ajax DELETE does not work
            return 200;
        }
    }
}