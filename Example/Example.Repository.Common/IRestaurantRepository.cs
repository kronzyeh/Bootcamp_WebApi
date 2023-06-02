using Example.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Example.Repository.Common
{
    public interface IRestaurantRepository
    {
        Task<List<Restaurant>> Get();
        Task<Restaurant> Get(Guid id);
        Task<int> Post([FromBody] Restaurant restaurant);
        Task<int> Put(Guid id, [FromBody] Restaurant restaurant);
        Task<int> Delete(Guid id);
        Task<Restaurant> GetRestaurantById(Guid id);
    }
}
