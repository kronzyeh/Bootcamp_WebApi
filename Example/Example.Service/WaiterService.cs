using Example.Common;
using Example.Model;
using Example.Repository;
using Example.Repository.Common;
using Example.Service.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Service
{
    public class WaiterService : IWaiterService
    {
        private readonly IWaiterRepository waiterRepository;
        public WaiterService(IWaiterRepository waiterRepository)
        {
            this.waiterRepository = waiterRepository;
        }

        public async Task<List<Waiter>> GetWaiters(Paging paging, Sorting sorting, Filter filter)
        {
            try
            {
                return await waiterRepository.Get(paging, sorting, filter);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
                throw;
            }
        }
        //public async Task<Waiter> GetSpecificWaiter()
        //{

        //}
    }
}
