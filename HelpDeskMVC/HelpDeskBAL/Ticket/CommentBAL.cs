using HelpDeskDAL.DataAccess;
using HelpDeskEntities;
using HelpDeskEntities.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskBAL.Ticket
{
    public class CommentBAL
    {
        private TktCommentRepository cmntRepo = new TktCommentRepository();

        public List<TicketComment> TicketComments(int tktID)
        {
            return cmntRepo.CommentList(tktID);
        }
               
        public Boolean PostComment(TicketComment tc, out string msg)
        {
            var flag = cmntRepo.PostComment(tc, out msg) > 0 ? true : false;
            return flag;
        }

        public List<TicketNature> TicketNature()
        {
            var TktNatures = new List<TicketNature>();

            foreach (var name in Enum.GetNames(typeof(Ticket_Nature)))
            {
                TicketNature ug = new TicketNature();
                ug.NatureID = (int)Enum.Parse(typeof(Ticket_Nature), name);
                ug.Nature = name;
                TktNatures.Add(ug);
            }
            return TktNatures;
        }
    }
}
