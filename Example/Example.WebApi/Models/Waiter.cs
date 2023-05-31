using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Example.WebApi.Models
{
    public class Waiter
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid RestaurantId { get; set; }
    }
}