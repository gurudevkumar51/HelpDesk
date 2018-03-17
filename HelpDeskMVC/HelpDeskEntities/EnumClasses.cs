using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskEntities
{
   public enum Ticket_Status
    {
        Open = 1,
        InProgress,
        Closed
    }

   public enum Ticket_Nature
    {
        query = 1,
        error,
        Suggestion
    }

   public enum User_Group
    {
        admin = 1,
        HelpdeskUser,
        SupeUser,
        EndUser
    }
}
