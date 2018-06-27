using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskEntities.Ticket
{
    public class TicketStatus
    {
        public int ID { get; set; }
        public string Status { get; set; }
    }

    public class TicketCountStatusWise
    {
        public int StatusID { get; set; }
        public int Count { get; set; }
        public string StatusDesc { get; set; }
    }
}
