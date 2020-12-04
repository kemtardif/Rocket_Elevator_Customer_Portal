using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;

namespace Rocker_Elevator_Customer_Portal.Models
{
    public class RESTHelper
    {
        private readonly HttpClient _client;

        public RESTHelper ()
        {
            _client = new HttpClient();
 
        }

        public async Task<HttpResponseMessage> putCall (string url, object _object)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(_object);
            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _client.PutAsync(url, content);

            return response;
        }

        public async Task<HttpResponseMessage> postCall (string url, Intervention intervention)
        {
            intervention.author_id = intervention.customer_id;

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(intervention);

            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _client.PostAsync(url, content);

            return response;
        }


    }
}
