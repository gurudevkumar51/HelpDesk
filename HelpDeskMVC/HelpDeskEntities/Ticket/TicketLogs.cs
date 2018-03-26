using HelpDeskEntities.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskEntities.Ticket
{
    public class TicketLogs
    {
        public TicketLogs()
        {
            LogBy = new User();
            LogFor = new User();

        }
        public int TicketID { get; set; }
        public int LogTypeID { get; set; }
        public string Logtype { get; set; }
        public string LogContent { get; set; }
        public string LogDateTime { get; set; }
        public User LogBy { get; set; }
        public User LogFor { get; set; }
    }
}
