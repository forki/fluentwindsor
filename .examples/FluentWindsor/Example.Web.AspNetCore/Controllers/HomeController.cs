﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Example.Web.AspNetCore.Models;

namespace Example.Web.AspNetCore.Controllers
{
    public class HomeController : Controller
    {
	    private readonly IAnyService anyService;

	    public HomeController(IAnyService anyService)
	    {
		    this.anyService = anyService;
	    }

	    public IActionResult Index()
	    {
		    var result = anyService.Anything();
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}