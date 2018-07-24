using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskEntities.Mail
{
  public class SmtpDetails
    {
        public SmtpDetails()
        {
            Smtp_Host = ConfigurationManager.AppSettings["SMTP_HostName"].ToString();
            Smtp_Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTP_Port"].ToString());
            Smtp_username = ConfigurationManager.AppSettings["SMTP_UserName"].ToString();
            Smtp_password = ConfigurationManager.AppSettings["SMTP_PWD"].ToString();
            Smtp_mailfrom = Smtp_username;
        }
        public string Smtp_mailfrom { get; set; }
        public string Smtp_Host { get; set; }
        public int Smtp_Port { get; set; }
        public string Smtp_username { get; set; }
        public string Smtp_password { get; set; }
    }
}
