using HelpDeskBAL.Module;
using HelpDeskBAL.Ticket;
using HelpDeskCommon.CommonClasses;
using HelpDeskEntities;
using HelpDeskEntities.Modules;
using HelpDeskEntities.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDeskMVC.Models
{
    public class AddTicketViewModel
    {
        private ModuleBAL mdlb = new ModuleBAL();
        private TicketBusiness tktBAL = new TicketBusiness();
        private CommentBAL cmntBAL = new CommentBAL();
        //private int CurrentUID = Convert.ToInt32(GenericClass.CsvToStringArray(HttpContext.Current.User.Identity.Name)[2]);
        public AddTicketViewModel()
        {
            TktNatures = new List<TicketNature>();
            TktNatures = cmntBAL.TicketNature();

            TktModules = new List<Modules>();
            TktModules = mdlb.AllModuleList();

            MyTickets = new List<Ticket>();
            MyTickets = tktBAL.TicketsByCreatedByCurrentUser().OrderBy(t => t.Status.ID).ToList();
        }
        public Ticket Tkt { get; set; }
        public List<TicketNature> TktNatures { get; set; }
        public List<Modules> TktModules { get; set; }
        public List<Ticket> MyTickets { get; set; }
    }
}