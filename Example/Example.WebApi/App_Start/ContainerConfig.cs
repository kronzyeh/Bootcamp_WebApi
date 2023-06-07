using Autofac;
using Autofac.Integration.WebApi;
using Example.Repository;
using Example.Repository.Common;
using Example.Service;
using Example.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Example.WebApi.App_Start
{
    public class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).InstancePerDependency();
            builder.RegisterType<RestaurantService>().As<IRestaurantService>().InstancePerDependency();
            builder.RegisterType<RestaurantRepository>().As<IRestaurantRepository>().InstancePerDependency();

            builder.RegisterType<WaiterService>().As<IWaiterService>().InstancePerDependency();
            builder.RegisterType<WaiterRepository>().As<IWaiterRepository>().InstancePerDependency();
            

            return builder.Build();
        }
    }
}