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
        List<Restaurant> Get();
        Restaurant Get(Guid id);
        int Post([FromBody] Restaurant restaurant);
        int Put(Guid id, [FromBody] Restaurant restaurant);
        int Delete(Guid id);
        Restaurant GetRestaurantById(Guid id);
    }
}
