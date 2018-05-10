using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Http;
using System.Json;  //Install-Package System.Json -Version 4.0.20126.16343
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Net;

namespace SSPWebUI.Views
{
    public partial class CodeBehindWebAPI : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterAsyncTask(new PageAsyncTask(LoadSomeData));
        }

        public async Task LoadSomeData()
        {
            WebClient client = new WebClient();
            var hdr = client.DownloadStringTaskAsync("http://localhost:49648/api/protocolheader?protocolversionckey=197.100004300");
            await Task.WhenAll(hdr);
            
            JObject j = JObject.Parse(hdr.Result);
            string test = "";
            foreach (var protocol in j)
            {
                test = test + protocol.Key + " = " + protocol.Value.ToString();
            }
            TextBox1.Text = test;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            //do not use

            //install-package Microsoft.AspNet.WebApi.Client
            // Create an HttpClient instance 
            HttpClient client = new HttpClient();
            string test = "";
            // Send a request asynchronously continue when complete 
            client.GetAsync("http://localhost:49648/api/protocolheader?protocolversionckey=197.100004300").ContinueWith( 
                (requestTask) => 
            { 
                // Get HTTP response from completed task. 
                HttpResponseMessage response = requestTask.Result; 

                // Check that response was successful or throw exception 
                response.EnsureSuccessStatusCode(); 
       
                // Read response asynchronously as JsonValue
                response.Content.ReadAsAsync<JObject>().ContinueWith( 
                        (readTask) => 
                        {
                            
                            foreach (var protocol in readTask.Result) 
                            { 
                                
                                test = test + protocol.Key + "=" + protocol.Value.ToString();
                                

                            }
                            
                        });
                
                });
            
        }
    }
}