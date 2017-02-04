using System;
using System.Web.Http;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Hawkeye;

namespace Example.MVC.Api
{
    [Interceptor(typeof (Hawkeye.Hawkeye))]
    public class DateTimeService
    {
        [Log(LogLevel.Info)]
        public virtual string GetDateTimeStamp()
        {
            var dateTime = DateTime.Now;
            return dateTime.ToLongDateString() + " " + dateTime.ToLongTimeString();
        }
    }

    public class DateTimeServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<DateTimeService>());
        }
    }

    [Interceptor(typeof(Hawkeye.Hawkeye))]
    public class DefaultController : ApiController
    {
        private readonly DateTimeService dateTimeService;

        public DefaultController(DateTimeService dateTimeService)
        {
            this.dateTimeService = dateTimeService;
        }

        [HttpGet]
        [Log(LogLevel.Info)]
        public virtual string Get()
        {
            return dateTimeService.GetDateTimeStamp();
        }
    }
}