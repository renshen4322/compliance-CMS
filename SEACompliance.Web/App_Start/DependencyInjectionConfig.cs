using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using SEACompliance.DAL.Interface;
using SEACompliance.DAL;
using SEACompliance.Service;
using SEACompliance.Service.Interface;
using System;
using System.Reflection;
using System.Web.Http;
using SEACompliance.Web.App;
using SEACompliance.Core.Autofac;
using System.Web.Mvc;
using SEACompliance.Web.Filters;

namespace SEACompliance.Web
{
    public class DependencyInjectionConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;
            config.Filters.Add(new LogExceptionAttribute());
            //register front-end controller
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            //register backoffice controllers
            builder.RegisterControllers(typeof(Umbraco.Web.UmbracoApplication).Assembly);           

            //register front end api
            builder.RegisterApiControllers(typeof(Application).Assembly);

            //register backoffice api
            builder.RegisterApiControllers(typeof(Umbraco.Web.UmbracoApplication).Assembly);

            builder.Register(c => new MapperService()).As<IMapperService>().SingleInstance();
            //builder.RegisterType<DemoService>().As<IDemoService>().InstancePerLifetimeScope();
            //builder.RegisterType<DemoDataProvider>().As<IDemoDataProvider>().InstancePerLifetimeScope();
            //builder.RegisterModule(new AutofacBLLManagerModule());


         
            RegisterServices(builder);
            RegisterDataProviders(builder);


            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            ContainerManager.SetContainer(container);
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            //builder.Register(c => new MapperService()).As<IMapperService>().SingleInstance();
            //builder.RegisterType<lnRIRecordService>().As<IlnRIRecordService>().SingleInstance();
            //builder.RegisterType<lnRICheckItemService>().As<IlnRICheckItemService>().SingleInstance();
            //builder.RegisterType<lnRIRelRecordCheckItemService>().As<IlnRIRelRecordCheckItemService>().SingleInstance();
            //builder.RegisterType<RIRecordTypeService>().As<IRIRecordTypeService>().SingleInstance();

            var Services = Assembly.Load("SEACompliance.Service");
            var IServices = Assembly.Load("SEACompliance.Service.Interface");
            builder.RegisterAssemblyTypes(IServices, Services)
                    .Where(t => t.Name.EndsWith("Service"))
                    .AsImplementedInterfaces()
                    .InstancePerDependency(); ;    //推荐用这种或下面都可

            //Type baseServiceType = typeof(IBLLService);
            //Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            ////扫描Assembly，查看类是否实现了BLLService，并且不是BLLService本身
            //builder.RegisterAssemblyTypes(assemblies)
            //    .Where(t => baseServiceType.IsAssignableFrom(t) && t != baseServiceType)
            //    //.AsSelf()
            //    .PropertiesAutowired()
            //    .AsImplementedInterfaces()
            //    .InstancePerDependency();

        }


        private static void RegisterDataProviders(ContainerBuilder builder)
        {
            //builder.RegisterType<lnRIRecordDataProvider>().As<IlnRIRecordDataProvider>().SingleInstance();
            //builder.RegisterType<lnRICheckItemDataProvider>().As<IRICheckItemDataProvider>().SingleInstance();
            //builder.RegisterType<lnRIRelRecordCheckItemDataProvider>().As<IRIRecordCheckItemDataProvider>().SingleInstance();
            //builder.RegisterType<RIRecordTypeProvider>().As<IRIRecordTypeProvider>().SingleInstance();

            var DataProvider = Assembly.Load("SEACompliance.DAL");
            var _DataProvider = Assembly.Load("SEACompliance.DAL.Interface");
            builder.RegisterAssemblyTypes(_DataProvider, DataProvider)
                    .Where(t => t.Name.EndsWith("Provider"))
                    .AsImplementedInterfaces()
                    .InstancePerDependency(); ;

            //Type baseServiceType = typeof(IDALProvider);
            //Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            ////扫描Assembly，查看类是否实现了BLLService，并且不是BLLService本身
            //builder.RegisterAssemblyTypes(assemblies)
            //    .Where(t => baseServiceType.IsAssignableFrom(t) && t != baseServiceType)
            //    //.AsSelf()
            //    .PropertiesAutowired()
            //    .AsImplementedInterfaces()
            //    .InstancePerDependency();
        }
    }
}
