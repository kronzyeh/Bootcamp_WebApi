using Example.Model;
using Example.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Example.Service.Common;

namespace Example.Service
{
    public class RestaurantService : IRestaurantService
    {
        public List<Restaurant> GetRestaurants()
        {
            try
            {
                RestaurantRepository _restaurantRepository = new RestaurantRepository();
                return _restaurantRepository.Get();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
                throw;
            }
        }
        public Restaurant GetSpecificRestaurant(Guid id)
        {
            try
            {
                RestaurantRepository _restaurantRepository = new RestaurantRepository();
                return _restaurantRepository.Get(id);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
                throw;
            }
        }
        public int SaveRestaurant([FromBody] Restaurant restaurant)
        {
            try
            {
                RestaurantRepository _restaurantRepository = new RestaurantRepository();
                return _restaurantRepository.Post(restaurant);
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
                throw;
            }
        }
        public int UpdateRestaurant(Guid id, [FromBody] Restaurant restaurant)
        {
            try
            {
                RestaurantRepository _restaurantRepository = new RestaurantRepository();
                return _restaurantRepository.Put(id, restaurant);
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
                throw;
            }
        }
        public int DeleteRestaurant(Guid id)
        {
            try
            {
                RestaurantRepository restaurantRepository = new RestaurantRepository();
                return restaurantRepository.Delete(id);
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
                throw;
            }
        }
    }
}
