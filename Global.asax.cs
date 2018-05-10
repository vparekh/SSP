using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using System.Reflection;
using System.Web.Routing;
using System.Web.Http;
namespace SSPWebUI
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            
            //try
            //{
            //    Assembly.Load("DevExpress.Web.ASPxTreeList.v16.1");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.ToString());
            //}

            //ScriptManager.ScriptResourceMapping.AddDefinition("jquery",
            //        new ScriptResourceDefinition
            //        {
            //            Path = "~/scripts/jquery-1.7.2.min.js",
            //            DebugPath = "~/scripts/jquery-1.7.2.min.js",
            //            CdnPath = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.4.1.min.js",
            //            CdnDebugPath = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.4.1.js"
            //        });

            RouteTable.Routes.MapHttpRoute(
                    "WithActionApiTwoParms",
                    "api/{controller}/{action}/{id}/{tag}"
                );

            RouteTable.Routes.MapHttpRoute(
                    "WithActionApiTwoParmsAlt",
                    "api/{controller}/{action}/{id}/{tag}/"
                );

            RouteTable.Routes.MapHttpRoute(
                     "WithActionApiOneParm",
                     "api/{controller}/{action}/{id}"
                 );

            //RouteTable.Routes.MapHttpRoute(
            //        name: "DefaultApi",
            //        routeTemplate: "api/{controller}/{id}",
            //        defaults: new { id = System.Web.Http.RouteParameter.Optional }
            //    );

            RouteTable.Routes.MapHttpRoute(
                    name: "DefaultApiNoparm",
                    routeTemplate: "api/{controller}/{action}"
                    
                );

            //load static objects
            
            SSPWebUI.Data.ItemType.LoadItemTypes();
            SSPWebUI.Data.BaseVersion.LoadBaseVersions();
            SSPWebUI.Data.DataType.LoadDataTypes();
            SSPWebUI.Data.AnswerUnits.LoadAnswerUnits();
          
            

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}