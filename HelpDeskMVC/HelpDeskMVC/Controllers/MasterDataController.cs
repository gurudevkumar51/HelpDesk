using HelpDeskBAL.Module;
using HelpDeskBAL.Ticket;
using HelpDeskBAL.User;
using HelpDeskEntities.Account;
using HelpDeskEntities.Modules;
using HelpDeskEntities.Ticket;
using System.Web.Mvc;

namespace HelpDeskMVC.Controllers
{
    [Authorize(Roles = "admin")]
    public class MasterDataController : Controller
    {
        private UserGroupBusiness ugBAL = new UserGroupBusiness();
        private ModuleBAL mdlb = new ModuleBAL();
        private TicketNatureBusiness TnBAL = new TicketNatureBusiness();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult TicketNatureList()
        {
            var data = TnBAL.GetAllTicketNature();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddTicketNature(string tn)
        {
            if (string.IsNullOrEmpty(tn))
            {
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }
            TicketNature m = new TicketNature();
            m.Nature = tn;
            string msg = "";
            var flag = TnBAL.AddTicketNature(m, out msg);
            return Json(new { status = flag > 0 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UserGroupList()
        {
            var data = ugBAL.GetAllUserGroups();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddModule(string module)
        {
            if (string.IsNullOrEmpty(module))
            {
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }
            Modules m = new Modules();
            m.Module = module;
            string msg = "";
            var flag = mdlb.AddNewModule(m, out msg);
            return Json(new { status = flag > 0 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddUserGroup(string UsrGroup)
        {
            if (string.IsNullOrEmpty(UsrGroup))
            {
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }
            UserGroup ug = new UserGroup();
            ug.UsrGroup = UsrGroup;
            string msg = "";
            var flag = ugBAL.AddUserGroup(ug, out msg);
            return Json(new { status = flag > 0 }, JsonRequestBehavior.AllowGet);
        }
    }
}