using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac.Core;
using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;

namespace SEACompliance.Web.AutofactModules
{
    public class AutofacWebAPIModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired().InstancePerRequest();
        }
    }
}