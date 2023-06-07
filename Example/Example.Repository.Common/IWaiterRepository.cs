using Example.Common;
using Example.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Repository.Common
{
    public interface IWaiterRepository
    {
        Task<List<Waiter>> Get(Paging paging, Sorting sorting, Filter filter);
        Task<Waiter> Get(Guid id);
    }
}
