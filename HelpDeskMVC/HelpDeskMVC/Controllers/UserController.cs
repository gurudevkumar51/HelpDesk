using HelpDeskBAL.User;
using HelpDeskEntities;
using HelpDeskEntities.Account;
using HelpDeskMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelpDeskMVC.Controllers
{
    public class UserController : Controller
    {
        private UserBusiness usrBAL = new UserBusiness();
        [Authorize]
        public ActionResult Index()
        {
           var usrlist= usrBAL.GetUserList(null);
            return View(usrlist);
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddUser()
        {
            AddUserViewModel m = new AddUserViewModel();
            return View(m);
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddUser(List<int> modules, User usr)
        {
            string msg = "";
            var flag = usrBAL.AddNewUser(usr,modules, out msg) > 0 ? true : false;
            return Json(new { success = flag, responseText = msg }, JsonRequestBehavior.AllowGet);
        }
    }
}