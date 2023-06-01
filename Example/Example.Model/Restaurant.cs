using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Example.Model;
using Example.Model.Common;

namespace Example.Model
{
    public class Restaurant : IRestaurant
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public int? Seats { get; set; }
        public string Address { get; set; }
        public string OwnerName { get; set; }
    }
}