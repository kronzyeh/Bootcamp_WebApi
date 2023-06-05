using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.UI.WebControls;
using System.Web.WebPages;
using Example.Common;
using Example.Model;
using Example.Service;
using Example.Service.Common;
using Example.WebApi.Models;
using Npgsql;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Example.WebApi.Controllers
{
    public class RestaurantController : ApiController
    {
        protected readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }
       
        public async Task<HttpResponseMessage> Get(int minimumSeats=0, string orderBy= "title" , int pageSize=10, int pageNumber = 1, string sortOrder = "Desc")
        {
            PageDetails pageDetails = new PageDetails();
            Paging paging = new Paging();
            paging.PageSize = pageSize;
            paging.PageNumber = pageNumber;
            Sorting sorting = new Sorting();
            sorting.SortOrder = sortOrder;
            sorting.OrderBy = orderBy;
            Filter filter = new Filter();
            filter.MinimumSeats = minimumSeats;
            List<RestaurantRest> restaurantsRest = new List<RestaurantRest>();
            List<Restaurant> restaurants = new List<Restaurant>();


            try
            {

                pageDetails = await _restaurantService.GetRestaurants(paging, sorting, filter);
                restaurantsRest = SetModelToRest(restaurants);
                return Request.CreateResponse(HttpStatusCode.OK, pageDetails);
            }
            catch (Exception ex)
            {

                Trace.WriteLine(ex.Message.ToString());
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            }


        }

        // GET api/<controller>/5
        public async Task<HttpResponseMessage> Get(Guid id)
        {
            Restaurant restaurant = new Restaurant();
            RestaurantRest restaurantRest = new RestaurantRest();
            try
            {
                restaurant = await _restaurantService.GetSpecificRestaurant(id);
                List<Restaurant> restaurants = new List<Restaurant>{
                    restaurant
                };
                List<RestaurantRest> restaurantsRest = new List<RestaurantRest>();

                restaurantsRest = SetModelToRest(restaurants);
                return Request.CreateResponse(HttpStatusCode.OK, restaurantsRest);
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/<controller>
        public async Task<HttpResponseMessage> Post([FromBody] RestaurantRest restaurantRest)
        {
            Restaurant restaurant = new Restaurant();
            int rowsAffected;
            try
            {   
                restaurant = SetModelFromRest(restaurantRest);
                rowsAffected = await _restaurantService.SaveRestaurant(restaurant);
                return Request.CreateResponse(HttpStatusCode.OK, rowsAffected);

            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

        }

        //// PUT api/<controller>/5
        public async Task<HttpResponseMessage> Put(Guid id, [FromBody] RestaurantRest restaurantRest)
        {
            Restaurant restaurant = new Restaurant();
            int rowsAffected;
            try
            {
                restaurant = SetModelFromRest(restaurantRest);
                rowsAffected = await _restaurantService.UpdateRestaurant(id, restaurant);
                return Request.CreateResponse(HttpStatusCode.OK, rowsAffected);
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }



        //// DELETE api/<controller>/5
        public async Task<HttpResponseMessage> Delete(Guid id)
        {
            int rowsAffected;
            try
            {
                rowsAffected = await _restaurantService.DeleteRestaurant(id);
                return Request.CreateResponse(HttpStatusCode.OK, rowsAffected);
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
        private List<RestaurantRest> SetModelToRest(List<Restaurant> restaurants)
        {
            List<RestaurantRest> restRestaurants = new List<RestaurantRest>();

            foreach (Restaurant restaurant in restaurants)
            {
                RestaurantRest restaurantRest = new RestaurantRest();
                restaurantRest.Title = restaurant.Title;
                restaurantRest.Address = restaurant.Address;
                restaurantRest.OwnerName = restaurant.OwnerName;
                restaurantRest.Seats = (int)restaurant.Seats;
                restRestaurants.Add(restaurantRest);
            }
            return restRestaurants;

        }
        private Restaurant SetModelFromRest(RestaurantRest restaurantRest)
        {
            Restaurant restaurant = new Restaurant
            {
                Title = restaurantRest.Title,
                Address = restaurantRest.Address,
                OwnerName = restaurantRest.OwnerName,
                Seats = restaurantRest.Seats,
                Id = Guid.NewGuid()
            };

            return restaurant;
        }
    }
}