using HelpDeskEntities.Mail;
using HelpDeskEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskCommon.CommonClasses
{
   public class GenericClass
    {
        //private static string SMTP_HostName = ConfigurationManager.AppSettings["SMTP_HostName"].ToString();
        //private static string SMTP_Port = ConfigurationManager.AppSettings["SMTP_Port"].ToString();
        //private static string SMTP_UserName = ConfigurationManager.AppSettings["SMTP_UserName"].ToString();
        //private static string SMTP_PWD = ConfigurationManager.AppSettings["SMTP_PWD"].ToString();
        public static string Hash(string value)
        {
            return Convert.ToBase64String(
                System.Security.Cryptography.SHA256.Create()
                .ComputeHash(Encoding.UTF8.GetBytes(value))
                );
        }

        public static string IntToStringMonth(int? value)
        {
            string month = "";
            switch (value)
            {
                case 1:
                    month = "January"; break;
                case 2:
                    month = "February"; break;
                case 3:
                    month = "March"; break;
                case 4:
                    month = "April"; break;
                case 5:
                    month = "May"; break;
                case 6:
                    month = "June"; break;
                case 7:
                    month = "July"; break;
                case 8:
                    month = "August"; break;
                case 9:
                    month = "September"; break;
                case 10:
                    month = "October"; break;
                case 11:
                    month = "November"; break;
                case 12:
                    month = "December"; break;
                default:
                    month = "invalid Month number";
                    break;
            }
            return month;
        }

        public static string[] CsvToStringArray(string Csv)
        {
            string[] splitString = Csv.Split(',');
            return splitString;
        }

        public static DateTime yyyyMMToDate(string yyyymm)
        {
            DateTime OutputDate = DateTime.ParseExact(yyyymm, "yyyyMM", CultureInfo.InvariantCulture);
            return OutputDate;
        }

        public static Boolean sendMail(EmailTemplate mailDetails, out string msg)
        {
            try
            {
                SmtpDetails smtpdtls = new SmtpDetails();
                msg = "";
                MailMessage mail = new MailMessage();
                foreach (string ToMail in mailDetails.Mail_To)
                {
                    if (!String.IsNullOrEmpty(ToMail))
                        mail.To.Add(ToMail);
                }
                foreach (string bccMail in mailDetails.Mail_bcc)
                {
                    if (!String.IsNullOrEmpty(bccMail))
                        mail.Bcc.Add(bccMail);
                }
                foreach (string ccMail in mailDetails.Mail_Cc)
                {
                    if (!String.IsNullOrEmpty(ccMail))
                        mail.CC.Add(ccMail);
                }
                
                if (!String.IsNullOrEmpty(Convert.ToString(smtpdtls.Smtp_mailfrom)))
                    mail.From = new MailAddress(smtpdtls.Smtp_mailfrom);

                mail.Subject = mailDetails.Mail_Subject;
                string Body = mailDetails.Mail_Content;
                mail.Body = Body;
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = smtpdtls.Smtp_Host;
                smtp.Port = smtpdtls.Smtp_Port;                
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new System.Net.NetworkCredential(smtpdtls.Smtp_username, smtpdtls.Smtp_password);
                smtp.EnableSsl = true;
                //smtp.SendAsync(mail, null);//.Send(mail);
                smtp.Send(mail);
                mail.Dispose();
                return true;
            }
            catch (SmtpException ex)
            {
                msg = ex.Message;
                return false;
            }
        }

        public static string GenerateOTP()
        {
            Random rnd = new Random();
            int otp = rnd.Next(100000, 999999);
            return otp.ToString();
        }

        public static Boolean VerifyOTP(object sessionOtp, string otp, out string msg)
        {
            msg = string.Empty;
            try
            {
                if (sessionOtp != null)
                {
                    OTP otpObj = (OTP)sessionOtp;
                    var currentDt = DateTime.Now;
                    if (currentDt < otpObj.ExpireDateTime && currentDt > otpObj.CreateDateTime)
                    {
                        if (otpObj.OTPvalue == otp)
                        {
                            msg = "Successfully verified";
                            return true;
                        }
                        else
                        {
                            msg = "You have entered wrong OTP";
                            return false;
                        }
                    }
                    else
                    {
                        msg = "Your OTP is expired Resend OTP";
                        return false;
                    }
                }
                else
                {
                    msg = "OTP is expired generate again";
                    return false;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }
    }
}
