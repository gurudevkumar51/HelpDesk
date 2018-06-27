using HelpDeskBAL.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelpDeskMVC.Controllers
{
    public class HomeController : Controller
    {
        private TicketBusiness tktBAL = new TicketBusiness();
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DashboardData()
        {
            var data = tktBAL.DashboardFlagData();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}