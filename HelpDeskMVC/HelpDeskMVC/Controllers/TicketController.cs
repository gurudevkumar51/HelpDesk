using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelpDeskMVC.Controllers
{
    public class TicketController : Controller
    {
        [Authorize(Roles = "admin, SupeUser, HelpdeskUser")]
        public ActionResult Index()
        {
            return View();
        }
        
        [Authorize(Roles = "admin, SupeUser, HelpdeskUser")]
        public ActionResult ClosedTickets()
        {
            return View();
        }

        [Authorize(Roles = "SupeUser, EndUser")]
        public ActionResult AddTicket()
        {
            return View();
        }

        [Authorize(Roles = "SupeUser, EndUser")]
        public ActionResult MyActiveTicket()
        {
            return View();
        }

        [Authorize(Roles = "SupeUser, EndUser")]
        public ActionResult MyClosedTicket()
        {
            return View();
        }
    }
}