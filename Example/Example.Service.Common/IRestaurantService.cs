using Example.Common;
using Example.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Example.Service.Common
{
    public interface IRestaurantService
    {
        Task<List<Restaurant>> GetRestaurants();
        Task<PageDetails> GetRestaurants(Paging paging, Sorting sorting, Filter filter);
        Task<Restaurant> GetSpecificRestaurant(Guid id);
        Task<int> SaveRestaurant([FromBody] Restaurant restaurant);
        Task<int> UpdateRestaurant(Guid id, [FromBody] Restaurant restaurant);
        Task<int> DeleteRestaurant(Guid id);
    }
}
