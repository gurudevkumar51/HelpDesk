using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelpDeskMVC.Controllers
{
    public class MiscellaneousController : Controller
    {        
        public ActionResult CheckExistingEmail(string EmailID)
        {
            try
            {
                //var pp = Umng.GetAllUserList();
                //var gg = pp.Any(x => x.EmailID == EmailID);
                return Json(!true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}