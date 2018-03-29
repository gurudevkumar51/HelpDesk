using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HelpDeskMVC.Models
{
    public class ResolveTicketViewModel
    {
        public int TicketID { get; set; }
        [DataType(DataType.MultilineText)]
        public string ResolutionComment { get; set; }
        public HttpPostedFileBase[] ResolutionAttachment { get; set; }
    }
}