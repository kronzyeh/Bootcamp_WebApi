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
        List<Restaurant> GetRestaurants();
        Restaurant GetSpecificRestaurant(Guid id);
        int SaveRestaurant([FromBody] Restaurant restaurant);
        int UpdateRestaurant(Guid id, [FromBody] Restaurant restaurant);
        int DeleteRestaurant(Guid id);
    }
}
