using HelpDeskBAL.User;
using HelpDeskEntities;
using HelpDeskEntities.Account;
using HelpDeskMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HelpDeskMVC.Controllers
{
    public class UserController : Controller
    {
        private UserBusiness usrBAL = new UserBusiness();

        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            TempData["Title"] = "User List";
            var usrlist = usrBAL.GetUserList(null).Where(u => u.Status == true);
            return View(usrlist);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult AddUser()
        {
            AddUserViewModel m = new AddUserViewModel();
            return View(m);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult AddUser(List<int> modules, User usr)
        {
            string msg = "";
            var flag = usrBAL.AddNewUser(usr, modules, out msg) > 0 ? true : false;
            return Json(new { success = flag, responseText = msg }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles ="admin")]
        [HttpGet]
        public ActionResult UpdateUser(int? UID)
        {
            if (UID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "User Id not passed in request");
            }
            AddUserViewModel UVM = new AddUserViewModel();
            UVM.User.UID = Convert.ToInt32(UID);
            return View(UVM);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult UpdateUser(List<int> modules, User usr)
        {
            string msg;
            var flag = usrBAL.UpdateUserProfile(usr, modules.Where(i => i != 0).ToList(), out msg);

            if (flag > 0)
            {
                //TempData["msg"] = usr.Name + " Account succesfully Updated";
                return Json(new { status = true, msg = msg }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //TempData["msg"] = usr.Name + " Account succesfully Updated";
                return Json(new { status = false, msg = msg }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteUser(int? UID)
        {
            if (UID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "User Id not passed in request");
            }
            var flag = usrBAL.DeleteUser(Convert.ToInt32(UID));
            if (flag > 0)
            {
                return Json(new { status = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize(Roles ="admin")]
        public ActionResult UserData(int? UID)
        {
            var userData = usrBAL.UserDetails(Convert.ToInt32(UID));
            return Json(userData, JsonRequestBehavior.AllowGet);
        }



        [Authorize]
        public ActionResult UserProfile(int UID)
        {
            TempData["Title"] = "User Profile";
            var userData= usrBAL.UserDetails(UID);
            return View(userData);
        }
    }
}