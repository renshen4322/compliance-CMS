using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Umbraco.Core;
using Umbraco.Core.Sync;
using Umbraco.Web;
using Umbraco.Web.Mvc;

using SEACompliance.Core.ConfigurationManagement;
using SEACompliance.Core.Enums;

namespace SEACompliance.Web.App
{
    public class Application : IApplicationEventHandler
    {
        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {

        }

        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {        
            DependencyInjectionConfig.RegisterDependencies();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BackOfficeMappingConfig.RegisterMapping();
            RouteTable.Routes.MapMvcAttributeRoutes();
        }

        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ConfigureServerRegistrar();
        }

        private void ConfigureServerRegistrar()
        {
            string serverModeString = ComplianceConfigurationManager.GetString("ProductServerMode");
            ProductServerMode serverMode;
            if (!Enum.TryParse<ProductServerMode>(serverModeString, out serverMode))
            {
                serverMode = ProductServerMode.None;
            }

            switch (serverMode)
            {
                case ProductServerMode.None:
                    break;
                //case ProductServerMode.Master:
                //    ServerRegistrarResolver.Current.SetServerRegistrar(new MasterServerRegistrar());
                //    break;
                //case ProductServerMode.Slave:
                //    ServerRegistrarResolver.Current.SetServerRegistrar(new FrontEndReadOnlyServerRegistrar());
                //    break;

            }
        }
    }
}
