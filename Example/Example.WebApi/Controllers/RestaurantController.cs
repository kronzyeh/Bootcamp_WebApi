using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.WebPages;
using Example.Model;
using Example.Service;
using Example.WebApi.Models;
using Npgsql;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Example.WebApi.Controllers
{
    public class RestaurantController : ApiController
    {
        // GET api/<controller>
        public HttpResponseMessage Get()
        {
            List<Restaurant> restaurants = new List<Restaurant>();
            List<RestaurantRest> restaurantsRest = new List<RestaurantRest>();
            Restaurant restaurant = new Restaurant();
            try
            {
                RestaurantService restaurantService = new RestaurantService();
                restaurants = restaurantService.GetRestaurants();
                foreach(Restaurant restaurantVar in restaurants)
                {
                    RestaurantRest restaurantRest = new RestaurantRest();
                    SetModelToRest(restaurantRest, restaurantVar);
                    restaurantsRest.Add(restaurantRest);
                }
                return Request.CreateResponse(HttpStatusCode.OK, restaurantsRest);
            }
            catch (Exception ex)
            {

                Trace.WriteLine(ex.Message.ToString());
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            }
        }

        // GET api/<controller>/5
        public HttpResponseMessage Get(Guid id)
        {
            Restaurant restaurant = new Restaurant();
            RestaurantRest restaurantRest = new RestaurantRest();
            try
            {
                RestaurantService restaurantService = new RestaurantService();
                restaurant = restaurantService.GetSpecificRestaurant(id);
                SetModelToRest(restaurantRest, restaurant);
                return Request.CreateResponse(HttpStatusCode.OK, restaurantRest);
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody] RestaurantRest restaurantRest)
        {
            Restaurant restaurant = new Restaurant();
            int rowsAffected;
            try
            {
                RestaurantService restaurantService = new RestaurantService();
                SetModelFromRest(restaurantRest, restaurant);
                rowsAffected = restaurantService.SaveRestaurant(restaurant);
                return Request.CreateResponse(HttpStatusCode.OK, rowsAffected);

            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

        }

        //// PUT api/<controller>/5
        public HttpResponseMessage Put(Guid id, [FromBody] RestaurantRest restaurantRest)
        {
            Restaurant restaurant = new Restaurant();
            int rowsAffected;
            try
            {
                RestaurantService restaurantService = new RestaurantService();
                SetModelFromRest(restaurantRest, restaurant);
                rowsAffected = restaurantService.UpdateRestaurant(id, restaurant);
                return Request.CreateResponse(HttpStatusCode.OK, rowsAffected);
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }



        //// DELETE api/<controller>/5
        public HttpResponseMessage Delete(Guid id)
        {
            int rowsAffected;
            try
            {
                RestaurantService restaurantService = new RestaurantService();
                rowsAffected = restaurantService.DeleteRestaurant(id);
                return Request.CreateResponse(HttpStatusCode.OK, rowsAffected);
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
        private void SetModelToRest(RestaurantRest restaurantRest, Restaurant restaurant)
        {
            restaurantRest.Title = restaurant.Title;
            restaurantRest.Address = restaurant.Address;
            restaurantRest.OwnerName = restaurant.OwnerName;
            restaurantRest.Seats = (int)restaurant.Seats;
        }
        private void SetModelFromRest(RestaurantRest restaurantRest, Restaurant restaurant)
        {
            restaurant.Title = restaurantRest.Title;
            restaurant.Address = restaurantRest.Address;
            restaurant.OwnerName = restaurantRest.OwnerName;
            restaurant.Seats = restaurantRest.Seats;
            restaurant.Id = Guid.NewGuid();
        }
    }
}