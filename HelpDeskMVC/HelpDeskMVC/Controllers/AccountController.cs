﻿using HelpDeskBAL.Account;
using HelpDeskCommon.CommonClasses;
using HelpDeskEntities.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HelpDeskMVC.Controllers
{
    public class AccountController : Controller
    {
        private AccountBusiness AccBAL = new AccountBusiness();

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

        [HttpPost][AllowAnonymous]
        public ActionResult login(Login lgn, string returnUrl)
        {
            string message = "";
            var user = AccBAL.login(lgn, out message);
            if (user != null)
            {
                var authTicket = new FormsAuthenticationTicket(
                    1,
                    user.EmailID + "," + user.Name + "," + user.UID+","+user.MemberSince,
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
                ViewBag.Message = message;
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

        public ActionResult ForgetPassword(string eml)
        {
            return Json(new { status = true }, JsonRequestBehavior.AllowGet);
        }
    }
}