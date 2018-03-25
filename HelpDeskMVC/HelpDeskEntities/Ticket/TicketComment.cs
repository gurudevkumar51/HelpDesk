using HelpDeskEntities.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskEntities.Ticket
{
    public class TicketComment
    {
        public TicketComment()
        {
            CommentBy = new User();
        }
        public int CommentID { get; set; }
        public int TicketID { get; set; }
        public string Comment { get; set; }
        public string CommentDate { get; set; }
        public User CommentBy { get; set; }
    }
}
