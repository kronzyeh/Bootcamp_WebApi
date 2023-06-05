using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Model.Common
{
    public interface IWaiter
    {
        Guid Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        Guid RestaurantId { get; set; }
    }
}
