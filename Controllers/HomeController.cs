using System;
using System.Web;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rocker_Elevator_Customer_Portal.Models;
using Rocker_Elevator_Customer_Portal.Areas.Identity.Data;
using System.Text.Json;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;




namespace Rocker_Elevator_Customer_Portal.Controllers 
{
    [Authorize]
    public class HomeController :  ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<Client> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<Client> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    
        public IActionResult Intervention()
        {
            return View();
        }

        public async Task<IActionResult> Products()
        {
            Client user = await _userManager.GetUserAsync(User);
            string userId = user?.Id;

            var elevatorQuery = "query { customerQuery(id:" + userId + "){elevators{id certOpe status dateCommissioning dateLastInspection column { battery { building { id }}} } }}";
            var columnQuery = "query { customerQuery(id:" + userId + "){columns{id typeBuilding amountFloorsServed status information battery{building{id}}}}}";
            var batteryQuery = "query { customerQuery(id:" + userId + "){batteries{ id status dateCommissioning dateLastInspection certOpe building{id}}} }";

            var url = "https://rocket-elevators-graphql.azurewebsites.net/graphql";


            object query1 = new {
                query = elevatorQuery
            };
            object query2 = new {
                query = columnQuery
            };
            object query3 = new {
                query = batteryQuery
            };

            var json1 = Newtonsoft.Json.JsonConvert.SerializeObject(query1);
            var json2 = Newtonsoft.Json.JsonConvert.SerializeObject(query2);
            var json3 = Newtonsoft.Json.JsonConvert.SerializeObject(query3);

            var content1 = new StringContent(json1.ToString(), Encoding.UTF8, "application/json");
            var content2 = new StringContent(json2.ToString(), Encoding.UTF8, "application/json");
            var content3 = new StringContent(json3.ToString(), Encoding.UTF8, "application/json");

            content1.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content2.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            content3.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var client = new HttpClient();

            var response1 = await client.PostAsync(url, content1);

            if(response1.IsSuccessStatusCode){
                
                var result1 = await response1.Content.ReadAsStringAsync();
                JObject elevatorJSON =  JObject.Parse(result1);
                ViewData["elevators"] = elevatorJSON["customerQuery"]["elevators"];

                var response2 = await client.PostAsync(url, content2);

                if (response2.IsSuccessStatusCode){
                    var result2 = await response2.Content.ReadAsStringAsync();
                    JObject columnJSON =  JObject.Parse(result2);
                    ViewData["columns"] = columnJSON["customerQuery"]["columns"];

                    var response3 = await client.PostAsync(url, content3);

                    if(response3.IsSuccessStatusCode){
                        var result3 = await response3.Content.ReadAsStringAsync();
                        JObject batteryJSON =  JObject.Parse(result3);
                        ViewData["batteries"] = batteryJSON["customerQuery"]["batteries"];

    
                        return View();

                    }
                }
            }
            SetFlash(FlashMessageType.Danger, "There was an error loading your page.");
              return LocalRedirect("/Home/Index");
            
        }
        [HttpPost]
        public async Task<IActionResult> submitForm([FromForm] Intervention intervention)
        {

            intervention.author_id = intervention.customer_id;
            var url = "https://rocket-elevators-foundation-restapi.azurewebsites.net/api/Interventions";

            Console.Write("##############");
            Console.Write( intervention.author_id);
            Console.Write("##############");
            Console.Write( intervention.employee_id);
            Console.Write("##############");


            var json = Newtonsoft.Json.JsonConvert.SerializeObject(intervention);

            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
         
            var client = new HttpClient();
            var response = await client.PostAsync(url, content);

            Console.Write("####################");
            Console.Write(response.IsSuccessStatusCode);
            Console.Write("####################");

            if(response.IsSuccessStatusCode){
                SetFlash(FlashMessageType.Success, "Intervention form succesfully sent!");
                return LocalRedirect("/Home/Index");

            }else{
                SetFlash(FlashMessageType.Success, "There was an error sending your form.");
                return LocalRedirect("/Home/Intervention");
            }
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
