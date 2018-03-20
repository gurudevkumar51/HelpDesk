using HelpDeskDAL.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskBAL.Ticket
{
    public class TicketBusiness
    {
        private TicketRepository tktRepo = new TicketRepository();
        public int AddNewTicket(HelpDeskEntities.Ticket.Ticket tkt, out string msg)
        {
            return tktRepo.AddNewTicket(tkt, out msg);
        }
    }
}
