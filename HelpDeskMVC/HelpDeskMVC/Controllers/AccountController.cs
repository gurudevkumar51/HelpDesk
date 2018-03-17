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
        [AllowAnonymous][HttpGet]
        public ActionResult login(string returnUrl)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Login model = new Login();
                if (Request.Cookies["MrLoginCookies"] != null)
                {
                    string Emid = Request.Cookies["HelpDeskCookies"].Values["Email"];
                    if (!string.IsNullOrEmpty(Emid))
                    {
                        model.Email = Emid;
                        model.Password = Request.Cookies["HelpDeskCookies"].Values["Password"];
                        model.Rememberme = true;
                        return View(model);
                    }
                    else
                    {
                        model.Email = "";
                        model.Password = "";
                        model.Rememberme = false;
                        return View(model);
                    }
                }
                model.Rememberme = false;
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "DashBoard");
            }
        }

        [HttpPost][AllowAnonymous]
        public ActionResult login(Login lgn, string returnUrl)
        {
            string message = "";
            var user = Umng.login(lgn, out message);
            if (user != null)
            {
                var authTicket = new FormsAuthenticationTicket(
                    1,
                    user.EmailID + "," + user.First_Name + " " + user.last_Name,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(20),
                    lgn.Rememberme,
                    user.Roles);
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                HttpContext.Response.Cookies.Add(authCookie);

                HttpCookie cookie = new HttpCookie("HelpDeskCookies");
                if (lgn.Rememberme)
                {
                    cookie.Values.Add("Email", lgn.Email);
                    cookie.Values.Add("Password", lgn.Password);
                    cookie.Values.Add("Name", user.First_Name + " " + user.last_Name);
                    cookie.Expires.AddDays(15);
                    Response.Cookies.Add(cookie);
                }
                else
                {
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie);
                }

                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                    && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {                    
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            else
            {
                ViewBag.Message = message;
            }
            return View();
        }
    }
}