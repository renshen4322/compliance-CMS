using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac.Core;
using Autofac;
using System.ServiceModel;

using Autofac.Integration.Mvc;

namespace SEACompliance.Web.AutofactModules
{
    public class AutofacMvcModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //Register MVC Controller
            //builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired().InstancePerHttpRequest();

            /*
            builder.RegisterControllers(typeof(MvcApplication).Assembly).InstancePerHttpRequest();
            */
            // OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();

            // OPTIONAL: Enable property injection in view pages.
            //builder.RegisterSource(new ViewRegistrationSource());

            // OPTIONAL: Enable property injection into action filters.
            builder.RegisterFilterProvider();
        }
    }
}