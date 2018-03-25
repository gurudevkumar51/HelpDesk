using HelpDeskBAL.Ticket;
using HelpDeskBAL.User;
using HelpDeskCommon.CommonClasses;
using HelpDeskEntities.Ticket;
using HelpDeskMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelpDeskMVC.Controllers
{
    public class TicketController : Controller
    {
        private TicketBusiness Tkt = new TicketBusiness();
        private UserBusiness UserBAL = new UserBusiness();

        [Authorize(Roles = "admin, SupeUser, HelpdeskUser")]
        public ActionResult Index()
        {
            return View(Tkt.AllActiceTickets());
        }

        [Authorize(Roles = "admin, SupeUser, HelpdeskUser")]
        public ActionResult ClosedTickets()
        {
            return View("Index", Tkt.AllClosedTickets());
        }

        [Authorize(Roles = "SupeUser, EndUser")]
        [HttpGet]
        public ActionResult AddTicket()
        {
            AddTicketViewModel m = new AddTicketViewModel();
            return View(m);
        }

        [Authorize(Roles = "SupeUser, EndUser")]
        [HttpPost]
        public ActionResult AddTicket(Ticket tkt)
        {
            string msg = "";
            Tkt.AddNewTicket(tkt, out msg);
            ViewBag.msg = msg;

            AddTicketViewModel m = new AddTicketViewModel();
            return View(m);
        }

        [Authorize(Roles = "SupeUser, EndUser")]
        public ActionResult MyActiveTicket()
        {
            var AssignBy = Convert.ToInt32(GenericClass.CsvToStringArray(User.Identity.Name)[2]);
            return View("Index", Tkt.AllActiceTickets().Where(u=>u.CreatedBy==AssignBy));
        }

        [Authorize(Roles = "SupeUser, EndUser")]
        public ActionResult MyClosedTicket()
        {
            var AssignBy = Convert.ToInt32(GenericClass.CsvToStringArray(User.Identity.Name)[2]);
            return View("Index", Tkt.AllClosedTickets().Where(u => u.CreatedBy == AssignBy));
        }

        [HttpGet]
        public ActionResult TicketAssign(int id, int tktID)
        {
            TicketAssign ta = new Models.TicketAssign();
            ta.TktID = tktID;
            ta.UserList = UserBAL.HelpDeskUserModuleWise(id);
            return PartialView(ta);
        }

        [HttpPost]
        public ActionResult TicketAssign(TicketAssign tas)
        {
            var AssignBy = Convert.ToInt32(GenericClass.CsvToStringArray(User.Identity.Name)[2]);
            Tkt.AssignTicketToUser(tas.TktID, tas.AssignTo, AssignBy, tas.Comment);
            return View("Index", Tkt.AllTicket().Where(t => t.Status.ID == 1));
        }

        [HttpGet]
        public ActionResult CloseTicket(int tktID)
        {
            var AssignBy = Convert.ToInt32(GenericClass.CsvToStringArray(User.Identity.Name)[2]);
            var flag = Tkt.CloseTicket(AssignBy, tktID);
            return Json(new { status = flag }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TicketDetails(int tktID)
        {
            return View(Tkt.TicketByID(tktID));
        }

        [HttpGet]
        public ActionResult TicketComments(int tktID)
        {
            CommentBAL cmntBAL = new CommentBAL();
            var cmnts = cmntBAL.TicketComments(tktID);
            return Json(cmnts, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult TicketComment(HelpDeskMVC.Models.TicketComment tc)
        {
            CommentBAL cmntBAL = new CommentBAL();
            HelpDeskEntities.Ticket.TicketComment cmnt = new HelpDeskEntities.Ticket.TicketComment();
            cmnt.Comment = tc.Comment;
            cmnt.TicketID = tc.TicketID;
            cmnt.CommentBy.UID= Convert.ToInt32(GenericClass.CsvToStringArray(User.Identity.Name)[2]);

            string msg = "";

           var flag= cmntBAL.PostComment(cmnt, out msg);
            return Json(new { status = flag, Response = msg }, JsonRequestBehavior.AllowGet);
        }
    }
}