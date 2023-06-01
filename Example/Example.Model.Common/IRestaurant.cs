using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Model.Common
{
    public interface IRestaurant
    {
        Guid? Id { get; set; }
        string Title { get; set; }
        int? Seats { get; set; }
        string Address { get; set; }
        string OwnerName { get; set; }

    }
}
