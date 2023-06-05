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
        Task<List<Waiter>> Get();
        Task<Waiter> Get(Guid id);
    }
}
