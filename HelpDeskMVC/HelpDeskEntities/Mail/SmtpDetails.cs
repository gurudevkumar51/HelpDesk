using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskEntities.Mail
{
  public  class SmtpDetails
    {
        public string Smtp_mailfrom { get; set; }
        public string Smtp_Host { get; set; }
        public int Smtp_Port { get; set; }
        public string Smtp_username { get; set; }
        public string Smtp_password { get; set; }
    }
}
