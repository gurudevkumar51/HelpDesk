using HelpDeskDAL.DataAccess;
using HelpDeskEntities.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskBAL.Ticket
{
    public class TicketNatureBusiness
    {
        private TicketNatureRepository tnRepo = new TicketNatureRepository();
        public List<TicketNature> GetAllTicketNature()
        {
            return tnRepo.GetAllTicketNature();
        }

        public int AddTicketNature(TicketNature tn, out string msg)
        {
            return tnRepo.AddTicketNature(tn, out msg);
        }
    }
}
