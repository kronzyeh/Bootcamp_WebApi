using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Example.WebApi.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public int Items { get; set; }
        public float Price { get; set; }
        public Guid WaiterId { get; set; }
        public Guid RestaurantId { get; set; }

    }
}