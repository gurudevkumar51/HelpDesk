using HelpDeskBAL.User;
using HelpDeskCommon.CommonClasses;
using HelpDeskDAL.DataAccess;
using HelpDeskEntities;
using HelpDeskEntities.Account;
using HelpDeskEntities.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HelpDeskBAL.Account
{
    public class AccountBusiness
    {
        private AccountRepository accRepo = new AccountRepository();
        private UserBusiness usrRepo = new UserBusiness();
        private string[] CurrentUser = GenericClass.CsvToStringArray(HttpContext.Current.User.Identity.Name);

        public HelpDeskEntities.Account.User login(HelpDeskEntities.Account.Login lgn, out string msg)
        {
            msg = "";
            try
            {
                var v = usrRepo.GetUserList(lgn.Email).FirstOrDefault();
                if (v != null)
                {
                    if (string.Compare(GenericClass.Hash(lgn.Password), v.Password) == 0)
                    {
                        v.Password = null;
                        if (v.Status)
                        {
                            accRepo.UpdateLastLogin(lgn.Email);
                            return v;
                        }
                        else
                        {
                            msg = "Your Account Is deactivated contact administrator";
                            return null;
                        }
                    }
                    else
                    {
                        msg = "Wrong Password/Email user";
                        return null;
                    }
                }
                else
                {
                    msg = "You are not registered with us";
                    return null;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }

        public Boolean ChangePassword(ChangePassword chp, out string msg)
        {
            chp.UserEmail = CurrentUser[0];
            var flag = false; msg = "";
            var v = usrRepo.GetUserList(CurrentUser[0]).FirstOrDefault();

            if (string.Compare(GenericClass.Hash(chp.Password), v.Password) == 0)
            {
                flag = accRepo.UpdateNewPassword(chp, out msg) > 0 ? true : false;
                msg = flag ? "Successfully changed the password" : msg;
            }
            else
            {
                flag = false;
                msg = "Old password is not Correct";
            }
            
            return flag;
        }

        public Boolean ResetPassword(ResetPassword rp, out string msg)
        {
            ChangePassword chp = new HelpDeskEntities.Account.ChangePassword()
            { UserEmail = rp.UserEmail, NewPassword = rp.NewPassword };

            var flag = accRepo.UpdateNewPassword(chp, out msg) > 0 ? true : false;
            msg = flag ? "Successfully updated new password" : msg;
            return flag;
        }

        public Boolean sendResetPasswordMail(string mail, out string msg,out OTP otp)
        {
            var usr = usrRepo.GetUserList(mail).FirstOrDefault();
            string subject = "";
            EmailTemplate et = new EmailTemplate();
            et.Mail_Content = MailContent(usr, out subject,out otp);
            //et.Mail_bcc.Add("gurudevkumar51@hotmail.com");
            et.Mail_To.Add(mail);
            et.Mail_Subject = subject;
            var flag = GenericClass.sendMail(et, out msg);
            return flag;
        }

        private string MailContent(HelpDeskEntities.Account.User usr, out string subject, out OTP otp)
        {
            string resetURL = ConfigurationManager.AppSettings["ResetPasswordURL"].ToString() + usr.UID;
            string loginURL = ConfigurationManager.AppSettings["LoginURL"].ToString();
            otp = new OTP(DateTime.Now) { OTPvalue = GenericClass.GenerateOTP() };
            subject = "Help Desk Reset Password";
            string Content = "<span>Hi " + usr.Name + ",<span>" +
                "<br /><p> You have requested for reset password.</p>"+
                "<p>Your <b>one time password</b> to reset your password is <h1 style='color: blue;'>" + otp.OTPvalue + "</h1>"+
                "<br/><small>This OTP is only valid for next 4 minutes</small>"+
                "</p>"+
                "<p>Please click on the below link to reset your password.</p>" +
                "<a href=" + resetURL + ">Reset Password<a/>" +
                "<br/><br/><h4>NOTE: If you have not requested for reset password ,somebody else might have  requested from your mail.</h4>" +
                "<h4> Please <a href =" + loginURL + "> Login </a> & change your password.</h4>";
            return Content;
        }
    }
} 
