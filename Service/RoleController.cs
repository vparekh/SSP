using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SSPWebUI.Data;

namespace SSPWebUI.Service
{
    public class RoleController : ApiController
    {
        // GET api/<controller>
        //public List<string> GetAll()
        //{
        //    List<string> roles = new List<string>();

        //    foreach( Role r in Role.getRoles())
        //    {
        //        roles.Add(r.RoleName);
        //    }
        //    return roles;
        //}
        public List<Role> GetAll()
        {
            List<Role> roles = new List<Role>();

            foreach (Role r in Role.getRoles())
            {
                roles.Add(r);
            }
            return roles;
        }

     public string GetRole(string UserCKey, string ProtocolCKey)
        {
            try
            {
                
                SSPUser user = SSPUser.GetUserByCKey(decimal.Parse(UserCKey));
                if (user.UserType == "6.100004300")
                    return "99";

                if (ProtocolCKey == null)
                    return "0";
                user.CKey = decimal.Parse(UserCKey);
                return user.GetProtocolRole(decimal.Parse(ProtocolCKey));
            }
         catch(Exception ex)
            {
                //throw exception
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                response.Content = new StringContent("GetRole: " + ex.Message);

                throw new HttpResponseException(response);
            }
            
        }
    }
}