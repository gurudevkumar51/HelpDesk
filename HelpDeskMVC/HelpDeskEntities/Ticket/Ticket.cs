using HelpDeskEntities.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HelpDeskEntities.Ticket
{
    public class Ticket
    {
        public Ticket()
        {
            Nature = new TicketNature();
            TicketModule = new Modules.Modules();
            Status = new TicketStatus();
            AssignedTo = new User();
            CreatedByUser = new User();
            Comments = new List<TicketComment>();
            AllFiles = new List<TicketFile>();
            TktLogs = new List<TicketLogs>();
        }

        public int TicketID { get; set; }
        public TicketNature Nature { get; set; }
        public Modules.Modules TicketModule { get; set; }

        [Required(ErrorMessage = "Enter your Ticket Description")]
        [DataType(DataType.MultilineText)]
        public string TicketDescription { get; set; }
        public int CreatedBy { get; set; }
        public TicketStatus Status { get; set; }
        public string CreatedDate { get; set; }
        public User AssignedTo { get; set; }
        public User CreatedByUser { get; set; }
        public List<TicketComment> Comments { get; set; }
        public List<TicketLogs> TktLogs { get; set; }
        public string TicketPriority { get; set; }

        [Display(Name = "Browse File")]
        public HttpPostedFileBase[] files { get; set; }
        public List<TicketFile> AllFiles { get; set; }
    }
}