using IExpro.Core.Entity;
using IExpro.Core.Interfaces.Common;
using IExpro.Core.Interfaces.Repository;
using IExpro.Core.Interfaces.Service;
using IExpro.Infrastructure.Repository;
using IExpro.Infrastructure.Services;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace ASP
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            container.RegisterType<ICommonRepository, CommonRepository>();
            container.RegisterType<ICommonService, CommonService>();

            container.RegisterType<IInvoiceRepository, InvoiceRepository>();
            container.RegisterType<IInvoiceService, InvoiceService>();



            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType<IDataContext, IExproContext>();
            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}