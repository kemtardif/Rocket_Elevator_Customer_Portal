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
    public class GraphQLHelper
    {
        private readonly HttpClient _client;

        private string _url;

        public GraphQLHelper ()
        {
            _client = new HttpClient();
            _url = "https://rocket-elevators-graphql.azurewebsites.net/graphql";
 
        }

        public async Task<HttpResponseMessage> apiCall (string _query)
        {
            object query = new {
                query = _query
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(query);
            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _client.PostAsync(_url, content);

            return response;
        }


    }
}