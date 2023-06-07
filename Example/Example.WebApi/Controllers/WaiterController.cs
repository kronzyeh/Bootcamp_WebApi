using Example.Common;
using Example.Model;
using Example.Service;
using Example.Service.Common;
using Example.WebApi.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Example.WebApi.Controllers
{
    public class WaiterController : ApiController
    {
        private readonly string _connectionString = "Server=localhost;Port=5432;Database=restaurant_database;User Id=postgres;Password=tomo;";
        // GET api/<controller>
        private readonly IWaiterService waiterService;
        public WaiterController(IWaiterService waiterService)
        {
            this.waiterService = waiterService;
        }
        public async Task<HttpResponseMessage> Get(string orderBy = "LastName", int pageSize = 3, int pageNumber = 1, string sortOrder = "Desc")
        {
            Paging paging = new Paging
            {
                PageSize = pageSize,
                PageNumber = pageNumber,
            };
            Sorting sorting = new Sorting
            {
                OrderBy = orderBy,
                SortOrder = sortOrder
            };
            Filter filter = new Filter
            {

            };
            List<Waiter> waiters = new List<Waiter>();
            List<WaiterRest> waitersRest = new List<WaiterRest>();
            Waiter waiter = new Waiter();
            try
            {
              
                waiters = await waiterService.GetWaiters(paging, sorting, filter);
                waitersRest = SetModelToRest(waiters);
                return Request.CreateResponse(HttpStatusCode.OK, waitersRest);
            }
            catch (Exception ex)
            {

                Trace.WriteLine(ex.Message.ToString());
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            }
        }
        private List<WaiterRest> SetModelToRest(List<Waiter> waiters)
        {
            List<WaiterRest> restWaiters = new List<WaiterRest>();

            foreach (Waiter waiter in waiters)
            {
                WaiterRest waiterRest = new WaiterRest();
                waiterRest.FirstName = waiter.FirstName;
                waiterRest.LastName = waiter.LastName;
                waiterRest.RestaurantName = waiter.RestaurantName;
                restWaiters.Add(waiterRest);
            }
            return restWaiters;

        }
    }
}