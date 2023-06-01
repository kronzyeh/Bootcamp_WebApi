using Example.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Example.WebApi.Models
{
    public class RestaurantRest
    {
        public string Title { get; set; }
        public string Address { get; set; }
        public string OwnerName { get; set; }
        public int Seats { get; set; }
      
    }
}