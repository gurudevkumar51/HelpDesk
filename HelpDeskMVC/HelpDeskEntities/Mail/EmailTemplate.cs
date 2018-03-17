using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskEntities.Mail
{
    public class EmailTemplate
    {
        public EmailTemplate()
        {
            Mail_To = new List<string>();
            Mail_bcc = new List<string>();
            Mail_Cc = new List<string>();
        }

        public List<string> Mail_To { get; set; }
        public List<string> Mail_bcc { get; set; }
        public List<string> Mail_Cc { get; set; }
        public string MailFrom { get; set; }

        public string Mail_Subject { get; set; }

        [Required(ErrorMessage = "Type your message to employee")]
        [System.Web.Mvc.AllowHtml]
        public string Mail_Content { get; set; }
    }
}
