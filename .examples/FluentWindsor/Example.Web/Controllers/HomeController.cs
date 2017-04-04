using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Example.Test.AssemblyC;

namespace Example.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ServiceC _serviceC;

        public HomeController(ServiceC serviceC)
        {
            _serviceC = serviceC;
        }

        public ActionResult Index()
        {
            _serviceC.Execute();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}