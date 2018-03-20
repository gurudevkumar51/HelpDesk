using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelpDeskMVC.Controllers
{
    public class HomeController : Controller
    {
         
        [Authorize]
        public ActionResult Index()
        {
            if (User.IsInRole("admin"))
            {
                return View("AdminHome");
            }
            else
            {
                return RedirectToAction("AddUser", "User");
            }
        }
    }
}