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
    public enum FlashMessageType
        {
            Success,
            Danger,
            Error
        }

    public class ControllerBase : Controller
    {
        public void SetFlash(FlashMessageType type, string text)
        {
            TempData["FlashMessage.Type"] = type;
            TempData["FlashMessage.Text"] = text;
        }
    }
}
