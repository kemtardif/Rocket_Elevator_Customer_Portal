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
using System.Text.Json;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;


namespace Rocker_Elevator_Customer_Portal.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
        [HttpPost]
        public async Task<IActionResult> submitForm(long customer_id, long building_id, long? battery_id, long? column_id, long? elevator_id, long employee_id, string report)
        {
                Intervention intervention = new Intervention() {
                    author_id = customer_id,
                    customer_id = customer_id,
                    result = "Incomplete",
                    status = "Pending",
                    building_id = building_id,
                    battery_id = battery_id,
                    column_id = column_id,
                    elevator_id = elevator_id,
                    employee_id = employee_id,
                    report = report
                };

            var url = "https://rocket-elevators-foundation-restapi.azurewebsites.net/api/Interventions";


            var json = Newtonsoft.Json.JsonConvert.SerializeObject(intervention);


            Console.Write("#################");
            Console.Write(json);
            Console.Write("##################");


            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
         
            var client = new HttpClient();
            var response = await client.PostAsync(url, content);

            Console.Write("#################");
            Console.Write(response);
            Console.Write("##################");

            if(response.IsSuccessStatusCode){

                return LocalRedirect("/Home/Index");

            }else{

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
