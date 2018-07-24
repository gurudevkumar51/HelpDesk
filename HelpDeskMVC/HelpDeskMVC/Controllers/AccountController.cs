using HelpDeskBAL.Account;
using HelpDeskBAL.User;
using HelpDeskCommon.CommonClasses;
using HelpDeskEntities;
using HelpDeskEntities.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HelpDeskMVC.Controllers
{
    public class AccountController : Controller
    {
        private AccountBusiness AccBAL = new AccountBusiness();
        private UserBusiness uBAL = new UserBusiness();

        [AllowAnonymous]
        [HttpGet]
        public ActionResult login(string returnUrl)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Login model = new Login();
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult login(Login lgn, string returnUrl)
        {
            string message = "";
            var user = AccBAL.login(lgn, out message);
            if (user != null)
            {
                var authTicket = new FormsAuthenticationTicket(
                    1,
                    user.EmailID + "," + user.Name + "," + user.UID + "," + user.MemberSince,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(20),
                    false,
                    user.UserGroup.UsrGroup);
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                HttpContext.Response.Cookies.Add(authCookie);

                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                    && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                TempData["errMsg"] = message;
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("login", "Account");
        }

        [Authorize]
        [HttpGet]
        public ActionResult ChangePassword()
        {
            //return PartialView();
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePassword Cp)
        {
            string msg = "";
            var flag = AccBAL.ChangePassword(Cp, out msg);
            //return Json(new { status = flag, response = msg }, JsonRequestBehavior.AllowGet);
            if (flag)
            {
                ViewBag.msg = msg;
            }
            else
            {
                ViewBag.msg = msg;
            }
            return View();
        }

        [HttpPost]
        public ActionResult SendResetPasswordMail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }

            Session["otp"] = null;
            OTP otp = new OTP();
            string msg = string.Empty;
            var flag = AccBAL.sendResetPasswordMail(email, out msg, out otp);
            if (flag)
            {
                Session["otp"] = otp;
            }            
            return Json(new { status = flag, message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ResetPassword(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "User Id not passed in request");
            }
            var u = uBAL.GetUserByUID(Convert.ToInt32(id));
            HelpDeskEntities.Account.ResetPassword cp = new HelpDeskEntities.Account.ResetPassword() { UserEmail = u.EmailID };
            return View(cp);
        }

        [HttpPost]
        public ActionResult ResetPassword(HelpDeskEntities.Account.ResetPassword cp)
        {
            var msg = "";

            if(GenericClass.VerifyOTP(Session["otp"], cp.otp, out msg))
            {
                var flag = AccBAL.ResetPassword(cp, out msg);
                if (flag)
                {
                    Session["otp"] = null;
                    TempData["succMsg"] = msg;
                    return RedirectToAction("login");
                }
                else
                {
                    TempData["errMsg"] = msg;
                    cp.otp = "";
                    return View(cp);
                }
            }
            else
            {
                cp.otp = "";
                TempData["errMsg"] = msg;
                return View(cp);
            }                        
        }
    }
}