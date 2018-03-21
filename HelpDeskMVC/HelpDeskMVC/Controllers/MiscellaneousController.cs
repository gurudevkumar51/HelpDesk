using HelpDeskBAL.Module;
using HelpDeskBAL.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelpDeskMVC.Controllers
{
    public class MiscellaneousController : Controller
    {
        private ModuleBAL mdlb = new ModuleBAL();
        private UserBusiness usr = new UserBusiness();
        
        [AllowAnonymous]
        public ActionResult CheckExistingEmail(string EmailID)
        {
            var pp = usr.GetUserList(EmailID);
            var gg = pp.Any(x => x.EmailID == EmailID);
            return Json(gg, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult ModuleList()
        {
            var Modules = new List<HelpDeskEntities.Modules.Modules>();
            Modules = mdlb.AllModuleList();
            return Json(Modules, JsonRequestBehavior.AllowGet);
        }

        [Authorize][ChildActionOnly]
        public ActionResult Menus()
        {
            return PartialView();
        }
    }
}