using HelpDeskCommon.CommonClasses;
using HelpDeskDAL.DataAccess;
using HelpDeskEntities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HelpDeskBAL.Ticket
{
    public class TicketBusiness
    {
        private TicketRepository tktRepo = new TicketRepository();
        private LogsRepository LogRepo = new LogsRepository();

        public List<HelpDeskEntities.Ticket.Ticket> AllTicket()
        {
            return tktRepo.AllTicket(null);
        }

        public List<HelpDeskEntities.Ticket.Ticket> AllActiceTickets()
        {
            //var role = HttpContext.Current.User.IsInRole("admin") ? "admin" : (HttpContext.Current.User.IsInRole("HelpdeskUser") ? "HelpdeskUser" : (HttpContext.Current.User.IsInRole("SupeUser") ? "SupeUser" : "EndUser"));
            return tktRepo.AllTicket(null).Where(t => t.Status.ID == 1 || t.Status.ID == 2).ToList(); 
        }

        public List<HelpDeskEntities.Ticket.Ticket> AllClosedTickets()
        {
            return tktRepo.AllTicket(null).Where(t => t.Status.ID == 3).ToList();
        }

        public List<HelpDeskEntities.Ticket.Ticket> AllTicketByCreatorUser(int UId)
        {
            return tktRepo.AllTicket(UId);
        }

        //When-ever we will assign ticket to any user Status will become InProgress status
        public Boolean AssignTicketToUser(int TktID, int AssignedTo, int AssignedBy, string comment)
        { string msg = "";
            var otpt = tktRepo.AssignTicketToUser(TktID, AssignedTo, AssignedBy, comment);
            var flag = otpt > 0 ? true : false;
            if (flag)
            {
                tktRepo.UpdateTicketStatus(TktID, 2);//2 Status id means InProgress
                LogRepo.AddTicketLog(AssignedBy, "Assigned to " + AssignedTo, TktID, out msg);
            }
            return flag;
        }

        public Boolean CloseTicket(int ClosedBy, int tktID)
        {
            string msg = "";
            var st = tktRepo.UpdateTicketStatus(tktID, 3);//3 Status id means Closed
            if (st > 0)
            {
                var log = LogRepo.AddTicketLog(ClosedBy, "Closed by " + ClosedBy, tktID, out msg);
                if (log > 0)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public int AddNewTicket(HelpDeskEntities.Ticket.Ticket tkt, out string msg)
        {
            tkt.CreatedBy = Convert.ToInt32(GenericClass.CsvToStringArray(HttpContext.Current.User.Identity.Name)[2]);
            tkt.Status.ID = (int)Enum.Parse(typeof(Ticket_Status), "Open");

            var InsertedId = tktRepo.AddNewTicket(tkt, out msg);

            #region Start Generate log for Created ticket
            if (InsertedId > 0)
            {
                var tktlogID = LogRepo.AddTicketLog(tkt.CreatedBy, msg, InsertedId, out msg);

                if (tkt.files.Count() > 0 && tktlogID > 0)
                {
                    foreach (HttpPostedFileBase file in tkt.files)
                    {
                        var fileFlag = SaveTicketFiles(file, tktlogID, msg, out msg);
                    }
                }
            }
            #endregion

            return InsertedId;
        }

        private Boolean SaveTicketFiles(HttpPostedFileBase file, int tktlogID, string InMsg, out string OutMsg)
        {
            OutMsg = InMsg;
            string Filepath = "~/TicketFiles";
            try
            {
                #region Saving file in TicketFiles folder
                bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(Filepath));
                if (!exists)
                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(Filepath));

                Filepath = HttpContext.Current.Server.MapPath(Filepath);

                var filename = Path.GetFileNameWithoutExtension(file.FileName) + "_" + DateTime.Now.Ticks + Path.GetExtension(file.FileName);

                file.SaveAs(Path.Combine(Filepath, filename));
                #endregion

                #region Storing file details in database
                var FileFlag = LogRepo.AddFileLog(tktlogID, filename, Path.GetExtension(file.FileName));
                OutMsg = FileFlag > 0 ? InMsg : InMsg + " But not added file details in database"; 
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
