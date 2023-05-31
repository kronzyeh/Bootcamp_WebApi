using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Example.WebApi.Models
{
    public class OrderWaiter
    {
        public string WaiterFirstName { get; set; }
        public string WaiterLastName { get; set; }
        public double OrderPrice { get; set; }
    }
}