using Microsoft.EntityFrameworkCore;
using System;

using System.Web.Mvc;

namespace Rocker_Elevator_Customer_Portal.Models
{
    public class InformationModel
    {

        public long id { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string address { get; set; }

        public long addressId { get; set; }
        public long buildingId { get; set; }



    }
}