using Example.Common;
using Example.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Service.Common
{
    public interface IWaiterService
    {
        Task<List<Waiter>> GetWaiters(Paging paging, Sorting sorting, Filter filter);
        
    }
}
