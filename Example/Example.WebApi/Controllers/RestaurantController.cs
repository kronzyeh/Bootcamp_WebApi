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
        public async Task<HttpResponseMessage> Get()
        {
            List<Restaurant> restaurants = new List<Restaurant>();
            List<RestaurantRest> restaurantsRest = new List<RestaurantRest>();
            Restaurant restaurant = new Restaurant();
            try
            {
                RestaurantService restaurantService = new RestaurantService();
                restaurants = await restaurantService.GetRestaurants();
                restaurantsRest = SetModelToRest(restaurants);
                return Request.CreateResponse(HttpStatusCode.OK, restaurantsRest);
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
                RestaurantService restaurantService = new RestaurantService();
                restaurant = await restaurantService.GetSpecificRestaurant(id);
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
                RestaurantService restaurantService = new RestaurantService();
                restaurant = SetModelFromRest(restaurantRest);
                rowsAffected = await restaurantService.SaveRestaurant(restaurant);
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
                RestaurantService restaurantService = new RestaurantService();
                restaurant = SetModelFromRest(restaurantRest);
                rowsAffected = await restaurantService.UpdateRestaurant(id, restaurant);
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
                RestaurantService restaurantService = new RestaurantService();
                rowsAffected = await restaurantService.DeleteRestaurant(id);
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
                Restaurant restaurant = new Restaurant();
                restaurant.Title = restaurantRest.Title;
                restaurant.Address = restaurantRest.Address;
                restaurant.OwnerName = restaurantRest.OwnerName;
                restaurant.Seats = restaurantRest.Seats;
                restaurant.Id = Guid.NewGuid();
          
                return restaurant;
        }
    }
}