using Example.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Example.Model
{
    public class Waiter : IWaiter
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid RestaurantId { get; set; }
        public string RestaurantName { get; set; }
    }
}