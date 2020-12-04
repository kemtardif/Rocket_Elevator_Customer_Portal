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
        private readonly GraphQLHelper _helper;




        public HomeController(ILogger<HomeController> logger, UserManager<Client> userManager)
        {
            _logger = logger;
            _userManager = userManager;
            _helper = new GraphQLHelper();

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

        public async Task<IActionResult> Building()
        {
            Client user = await _userManager.GetUserAsync(User);
            string userId = user?.Id;

            var detailQuery = "query { customerQuery(id:" + userId + "){buildings{id admContactPhone admContactMail admContactName address{id address1}}}}";

           var response = await _helper.apiCall(detailQuery);


             if(response.IsSuccessStatusCode){
                
                var result = await response.Content.ReadAsStringAsync();
                JObject detailJSON =  JObject.Parse(result);

                ViewData["details"] = detailJSON["customerQuery"]["buildings"];
           
                return View();
             }

            SetFlash(FlashMessageType.Danger, "There was an error loading your page.");
            return LocalRedirect("/Home/Index");
        }

        public async Task<IActionResult> Products()
        {
            Client user = await _userManager.GetUserAsync(User);
            string userId = user?.Id;

            var elevatorQuery = "query { customerQuery(id:" + userId + "){elevators{id certOpe status dateCommissioning dateLastInspection column { battery { building { id }}} } }}";
            var columnQuery = "query { customerQuery(id:" + userId + "){columns{id typeBuilding amountFloorsServed status information battery{building{id}}}}}";
            var batteryQuery = "query { customerQuery(id:" + userId + "){batteries{ id status dateCommissioning dateLastInspection certOpe building{id}}} }";

            var response1 = await _helper.apiCall(elevatorQuery);

            if(response1.IsSuccessStatusCode){
                
                var result1 = await response1.Content.ReadAsStringAsync();
                JObject elevatorJSON =  JObject.Parse(result1);

                ViewData["elevators"] = elevatorJSON["customerQuery"]["elevators"];

                var response2 = await _helper.apiCall(columnQuery);

                if (response2.IsSuccessStatusCode){

                    var result2 = await response2.Content.ReadAsStringAsync();
                    JObject columnJSON =  JObject.Parse(result2);

                    ViewData["columns"] = columnJSON["customerQuery"]["columns"];

                   var response3 = await _helper.apiCall(batteryQuery);

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


            var json = Newtonsoft.Json.JsonConvert.SerializeObject(intervention);

            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
         
            var client = new HttpClient();
            var response = await client.PostAsync(url, content);


            if(response.IsSuccessStatusCode){
                SetFlash(FlashMessageType.Success, "Intervention form succesfully sent!");
                return LocalRedirect("/Home/Index");

            }else{
                SetFlash(FlashMessageType.Success, "There was an error sending your form.");
                return LocalRedirect("/Home/Intervention");
            }
        }

        [HttpPost]
        public async Task<IActionResult> sendInfo([FromForm] InformationModel information)
        {

            object address = new {
                id = information.addressId,
                address = information.address
            };

            object details = new {
                id = information.buildingId,
                adm_contact_name = information.name,
                adm_contact_phone = information.phone,
                adm_contact_mail = information.email

            };
            var url1 = "https://rocket-elevators-foundation-restapi.azurewebsites.net/api/Addresses/" + information.addressId;
            var url2 = "https://rocket-elevators-foundation-restapi.azurewebsites.net/api/Buildings/" + information.buildingId;

            var json1 = Newtonsoft.Json.JsonConvert.SerializeObject(address);
            var json2 = Newtonsoft.Json.JsonConvert.SerializeObject(details);


            var content1 = new StringContent(json1.ToString(), Encoding.UTF8, "application/json");
            content1.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var content2 = new StringContent(json2.ToString(), Encoding.UTF8, "application/json");
            content2.Headers.ContentType = new MediaTypeHeaderValue("application/json");
         
            var client = new HttpClient();
            var response1 = await client.PutAsync(url1, content1);


            if(response1.IsSuccessStatusCode){

                var response2 = await client.PutAsync(url2, content2);

                if(response2.IsSuccessStatusCode){
                    
                    SetFlash(FlashMessageType.Success, "Your informations were succesfully edited.");
                    return LocalRedirect("/Home/Index");
                }

                SetFlash(FlashMessageType.Danger, "There was an error editing your informations");
                return LocalRedirect("/Home/Building");

            }else{

                SetFlash(FlashMessageType.Danger, "There was an error editing your address");
                return LocalRedirect("/Home/Building");
            }
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
