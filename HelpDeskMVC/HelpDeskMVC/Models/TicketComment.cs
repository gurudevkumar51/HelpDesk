using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDeskMVC.Models
{
    public class TicketComment
    {
        public int TicketID { get; set; }
        public string Comment { get; set; }
    }
}