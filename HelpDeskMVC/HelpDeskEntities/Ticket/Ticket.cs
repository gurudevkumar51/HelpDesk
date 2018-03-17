using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskEntities.Ticket
{
    public class Ticket
    {
        public int TicketID { get; set; }
        public TicketNature Nature { get; set; }
        public Modules.Modules TicketModule { get; set; }
        public string TicketDescription { get; set; }
        public int CreatedBy { get; set; }
        public TicketStatus Status { get; set; }
        public string CreatedDate { get; set; }
        public string ClosureDate { get; set; }
    }
}
