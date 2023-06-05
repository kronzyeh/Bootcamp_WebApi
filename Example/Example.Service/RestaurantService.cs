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
using Example.Common;
using Example.Repository.Common;

namespace Example.Service
{
    public class RestaurantService : IRestaurantService
    {
        protected readonly IRestaurantRepository restaurantRepository;
        public RestaurantService(IRestaurantRepository restaurantRepository)
        {
            this.restaurantRepository = restaurantRepository;
        }

        public async Task<List<Restaurant>> GetRestaurants()
        {
            try
            {
                return await restaurantRepository.Get();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
                throw;
            }
        }
        public async Task<PageDetails> GetRestaurants(Paging paging, Sorting sorting, Filter filter)
        {
            try
            {
                return await restaurantRepository.Get(paging, sorting, filter);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
                throw;
            }
        }
        public async Task<Restaurant> GetSpecificRestaurant(Guid id)
        {
            try
            {
                return await restaurantRepository.Get(id);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
                throw;
            }
        }
        public async Task<int> SaveRestaurant([FromBody] Restaurant restaurant)
        {
            try
            {
                return await restaurantRepository.Post(restaurant);
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
                throw;
            }
        }
        public async Task<int> UpdateRestaurant(Guid id, [FromBody] Restaurant restaurant)
        {
            try
            {
                return await restaurantRepository.Put(id, restaurant);
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
                throw;
            }
        }
        public async Task<int> DeleteRestaurant(Guid id)
        {
            try
            {
                return await restaurantRepository.Delete(id);
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
                throw;
            }
        }
    }
}
