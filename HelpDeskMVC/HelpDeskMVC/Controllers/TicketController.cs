using HelpDeskBAL.Module;
using HelpDeskBAL.Ticket;
using HelpDeskBAL.User;
using HelpDeskCommon.CommonClasses;
using HelpDeskEntities.Ticket;
using HelpDeskMVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelpDeskMVC.Controllers
{
    public class TicketController : Controller
    {
        private TicketBusiness Tkt = new TicketBusiness();
        private UserBusiness UserBAL = new UserBusiness();
        private CommentBAL cmntBAL = new CommentBAL();
        private ModuleBAL mdlb = new ModuleBAL();

        #region Ticket Listing Action Methods
        [Authorize(Roles = "admin, SupeUser, HelpdeskUser")]
        //It gives all Active Tickets
        public ActionResult Index()
        {
            TempData["Title"] = "Active Tickets";
            return View(Tkt.AllActiceTickets());
        }

        [Authorize(Roles = "admin, SupeUser, HelpdeskUser")]
        public ActionResult ClosedTickets()
        {
            TempData["Title"] = "Closed Tickets";
            return View("Index", Tkt.AllClosedTickets());
        }

        [Authorize(Roles = "SupeUser, EndUser, HelpdeskUser")]
        public ActionResult MyActiveTicket()
        {
            TempData["Title"] = "My Active Tickets";
            return View("Index", Tkt.MyActiveTickets());
        }

        [Authorize(Roles = "SupeUser, EndUser, HelpdeskUser")]
        public ActionResult MyClosedTicket()
        {
            TempData["Title"] = "My Closed Tickets";
            return View("Index", Tkt.MyClosedTickets());
        }

        [Authorize]
        public ActionResult TicketDetails(int tktID)
        {
            TempData["Title"] = "Ticket Details";
            var data = Tkt.TicketByTktID(tktID);
            return View(data);
        }
        #endregion

        #region Ticket Transactions Methods

        [Authorize(Roles = "SupeUser, EndUser, HelpdeskUser")]
        [HttpGet]
        public ActionResult AddTicket()
        {
            TempData["Title"] = "Add Ticket";
            AddTicketViewModel m = new AddTicketViewModel();
            return View(m);
        }

        [Authorize(Roles = "SupeUser, EndUser, HelpdeskUser")]
        [HttpPost]
        public ActionResult AddTicket(Ticket tkt)
        {
            string msg = "";
            Tkt.AddNewTicket(tkt, out msg);
            ViewBag.msg = msg;
            TempData["Title"] = "Add Ticket";
            AddTicketViewModel m = new AddTicketViewModel();
            return View(m);
        }

        [Authorize(Roles = "SupeUser, EndUser, HelpdeskUser")]
        [HttpGet]
        public ActionResult EditTicket(int id)
        {
            AddTicketViewModel m = new AddTicketViewModel();
            ViewBag.TktNatures = m.TktNatures;
            ViewBag.Modules = m.TktModules;
            return View(m.MyTickets.Where(t => t.TicketID == id).FirstOrDefault());
        }

        [Authorize(Roles = "SupeUser, EndUser, HelpdeskUser")]
        [HttpPost]
        public ActionResult EditTicket(Ticket tkt)
        {
            string msg = "";
            Tkt.UpdateTicket(tkt, out msg);
            ViewBag.msg = msg;
            AddTicketViewModel m = new AddTicketViewModel();
            return RedirectToAction("AddTicket", m);
        }

        [Authorize(Roles = "SupeUser,SupportStaff,HelpdeskUser")]
        [HttpGet]
        public ActionResult ResolveTicket(int tktID)
        {
            ResolveTicketViewModel resolve = new ResolveTicketViewModel();
            resolve.TicketID = tktID;
            return PartialView(resolve);
        }

        [Authorize(Roles = "SupeUser,SupportStaff,HelpdeskUser")]
        [HttpPost]
        public ActionResult ResolveTicket(ResolveTicketViewModel resolve)
        {
            string msg = "";
            msg = Tkt.ResolveTicket(resolve.TicketID,resolve.ResolutionComment,resolve.ResolutionAttachment) ? "Ticket is resolved" : "Unable to change status";
            TempData["msg"] = ViewBag.msg = msg;
            //return RedirectToAction("TicketDetails", Tkt.TicketByTktID(resolve.TicketID));// View("TicketDetails", Tkt.TicketByTktID(resolve.TicketID));
            //return RedirectToAction("TicketDetails", resolve.TicketID);
            return View("TicketDetails", Tkt.TicketByTktID(resolve.TicketID));
        }

        public ActionResult ReOpen(int TktID)
        {            
            TempData["Title"] = "Ticket Details";
            TempData["msg"] = Tkt.ReopenTicket(TktID) ? "Ticket Reopened Succesfully" : "Unable to Reopen Ticket";
            return View("TicketDetails", Tkt.TicketByTktID(TktID));
        }

        [Authorize(Roles = "HelpdeskUser")]
        [HttpPost]
        public ActionResult SetTicketPriority(int tktID, int priorityID)
        {
            string msg = "";
            var flag = Tkt.SetTicketPriority(tktID, priorityID, out msg);
            return Json(new { status = flag, response = msg }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "HelpdeskUser")]
        [HttpGet]
        public ActionResult TicketAssign(int id, int tktID)
        {
            TicketAssign ta = new Models.TicketAssign();
            ta.TktID = tktID;
            ta.UserList = UserBAL.AllSupportUsersForModule(id);//.GetAllSupportUsers();
            return PartialView(ta);
        }

        [HttpPost]
        [Authorize(Roles = "HelpdeskUser")]
        public ActionResult TicketAssign(TicketAssign tas)
        {
            Tkt.AssignTicketToUser(tas.TktID, tas.AssignTo, tas.Comment);
            return View("Index", Tkt.AllActiceTickets());
        }

        [Authorize(Roles = "HelpdeskUser")]
        [HttpGet]
        public ActionResult EsclateTicket(int id, int tktID)
        {
            TicketAssign ta = new Models.TicketAssign();
            ta.TktID = tktID;
            ta.UserList = UserBAL.GetAllSupportUsers();
            return PartialView("TicketAssign", ta);
        }

        [HttpPost]
        [Authorize(Roles = "HelpdeskUser")]
        public ActionResult EsclateTicket(TicketAssign tas)
        {
            Tkt.AssignTicketToUser(tas.TktID, tas.AssignTo, tas.Comment);
            return View("Index", Tkt.AllActiceTickets());
        }

        [HttpGet]
        [Authorize(Roles = "SupeUser, EndUser, admin")]
        public ActionResult CloseTicket(int tktID)
        {
            var flag = Tkt.CloseTicket(tktID);
            return Json(new { status = flag }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [Authorize]
        [HttpGet]
        public ActionResult TicketComments(int tktID)
        {
            CommentBAL cmntBAL = new CommentBAL();
            var cmnts = cmntBAL.TicketComments(tktID);
            return Json(cmnts, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        [HttpPost]
        public ActionResult TicketComment(HelpDeskMVC.Models.TicketComment tc)
        {
            CommentBAL cmntBAL = new CommentBAL();
            HelpDeskEntities.Ticket.TicketComment cmnt = new HelpDeskEntities.Ticket.TicketComment();
            cmnt.Comment = tc.Comment;
            cmnt.TicketID = tc.TicketID;
            cmnt.CommentBy.UID = Convert.ToInt32(GenericClass.CsvToStringArray(User.Identity.Name)[2]);

            string msg = "";

            var flag = cmntBAL.PostComment(cmnt, out msg);
            return Json(new { status = flag, Response = msg }, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult DownloadFile(string filename, string OriginalFileName)
        {
            var filePath = Server.MapPath("~/TicketFiles/" + filename);
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            string fileName = OriginalFileName;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            //try
            //{
            //    var filePath = Server.MapPath("~/TicketFiles/" + filename);
            //    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            //    string fileName = OriginalFileName;
            //    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            //}
            //catch (Exception ex)
            //{
            //    return Content("File not available on server");
            //}
        }
        [Authorize]
        public ActionResult TktLogs(int tktID)
        {
            return Json(Tkt.AllLogs(tktID), JsonRequestBehavior.AllowGet);
        }
    }
}