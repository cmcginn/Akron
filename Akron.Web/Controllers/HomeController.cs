using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Akron.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var r = new FilePathResult("~/index.html", "text/html");
            return r;
        }

        public ActionResult DC()
        {
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

        public ActionResult Scratch()
        {
            return View();
        }
    }
}