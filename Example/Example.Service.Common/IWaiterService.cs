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
        Task<List<Waiter>> GetWaiters();
        Task<Waiter> GetSpecificWaiter(Guid id);
    }
}
