using Example.Model;
using Example.Repository;
using Example.Service.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Service
{
    public class WaiterService /*: /*IWaiterService*/
    {
        public async Task<List<Waiter>> GetWaiters()
        {
            try
            {
                WaiterRepository waiterRepository = new WaiterRepository();
                return await waiterRepository.Get();
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
