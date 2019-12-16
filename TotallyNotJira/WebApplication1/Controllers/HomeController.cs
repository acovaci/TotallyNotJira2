using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Member, Organizator,Administrator")]
        public ActionResult Projects()
        {
            // ViewBag.Message = "Your application description page.";

            //  return View();
            return RedirectToAction("Index", "Project");
        }
        [Authorize(Roles = "Member, Organizator,Administrator")]
        public ActionResult Teams()
        {
            // ViewBag.Message = "Your contact page.";

             return View();
        //    return RedirectToAction("Index", "Teams");
        }
    }
}