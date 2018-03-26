using HelpDeskBAL.Module;
using HelpDeskCommon.CommonClasses;
using HelpDeskDAL.DataAccess;
using HelpDeskEntities;
using HelpDeskEntities.Ticket;
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

        public List<HelpDeskEntities.Ticket.Ticket> TicketsAssignedToCurrentUser(int UID)
        {
            ModuleBAL mdl = new ModuleBAL();
            List<HelpDeskEntities.Modules.Modules> mdls = new List<HelpDeskEntities.Modules.Modules>();
            return tktRepo.AllTicket(null).Where(t => t.AssignedTo.UID == UID).ToList();
        }

        public HelpDeskEntities.Ticket.Ticket TicketByTktID(int tktID)
        {
            var tkt= tktRepo.TicketByID(tktID).FirstOrDefault();
            tkt.AllFiles = LogRepo.Files(tktID);
            tkt.TktLogs = LogRepo.TicketLogs(tktID);
            return tkt;
        }

        public List<HelpDeskEntities.Ticket.Ticket> AllActiceTickets()
        {            
            return tktRepo.AllTicket(null).Where(t => t.Status.ID != 3).ToList(); 
        }

        public List<HelpDeskEntities.Ticket.Ticket> AllClosedTickets()
        {
            return tktRepo.AllTicket(null).Where(t => t.Status.ID == 3).ToList();
        }

        public List<HelpDeskEntities.Ticket.Ticket> AllTicketByCreatorUser(int UId)
        {
            return tktRepo.AllTicket(UId);
        }

        //When-ever we will assign ticket to any user Status will become InProgress status automatically
        public Boolean AssignTicketToUser(int TktID, int AssignedTo, int AssignedBy, string comment)
        {
            string msg = "";
            User.UserBusiness u = new User.UserBusiness();

            var otpt = LogRepo.AddTicketLog(AssignedBy, "Assigned to " + u.GetUserByUID(AssignedTo).Name, TktID, out msg);
            var flag = otpt > 0 ? true : false;
            if (flag)
            {
                tktRepo.AssignTicketToUser(TktID, AssignedTo, AssignedBy, comment);
                tktRepo.UpdateTicketStatus(TktID, 2);//2 Status id means InProgress
            }
            return flag;
        }

        public Boolean EsclateTicketToUser(int TktID, int AssignedTo, int AssignedBy, string comment)
        {
            string msg = "";
            User.UserBusiness u = new User.UserBusiness();

            var otpt = LogRepo.AddTicketLog(AssignedBy, "Esclated to " + u.GetUserByUID(AssignedTo).Name, TktID, out msg);
            var flag = otpt > 0 ? true : false;
            if (flag)
            {
                tktRepo.AssignTicketToUser(TktID, AssignedTo, AssignedBy, comment);
                //tktRepo.UpdateTicketStatus(TktID, 2);//2 Status id means InProgress
            }
            return flag;
        }

        public Boolean CloseTicket(int ClosedBy, int tktID, string ClosedByName)
        {
            string msg = "";
            var st = tktRepo.UpdateTicketStatus(tktID, 3);//3 Status id means Closed
            if (st > 0)
            {
                var log = LogRepo.AddTicketLog(ClosedBy, "Closed by " + ClosedByName, tktID, out msg);
                if (log > 0)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public Boolean ResolveTicket(int ResolvedByID, int tktID, string ResolvedByName)
        {
            string msg = "";
            var st = tktRepo.UpdateTicketStatus(tktID, 4);//4 Status id means Resolved
            if (st > 0)
            {
                var log = LogRepo.AddTicketLog(ResolvedByID, "Resolved by " + ResolvedByName, tktID, out msg);
                if (log > 0)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public Boolean ReopenTicket(int ReopenByID, int tktID, string ReopenByName)
        {
            string msg = "";
            var st = tktRepo.UpdateTicketStatus(tktID, 1);//1 Status id means Open
            if (st > 0)
            {
                var log = LogRepo.AddTicketLog(ReopenByID, "Reopened by " + ReopenByName, tktID, out msg);
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
                        var fileFlag = SaveTicketFiles(file, tktlogID, InsertedId, msg, out msg);
                    }
                }
            }
            #endregion

            return InsertedId;
        }

        private Boolean SaveTicketFiles(HttpPostedFileBase file, int tktlogID,int tktID, string InMsg, out string OutMsg)
        {
            OutMsg = InMsg;
            string Filepath = "~/TicketFiles";
            try
            {
                string OriginalFileName = file.FileName;
                #region Saving file in TicketFiles folder
                bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(Filepath));
                if (!exists)
                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(Filepath));

                Filepath = HttpContext.Current.Server.MapPath(Filepath);

                var filename = Path.GetFileNameWithoutExtension(file.FileName) + "_" + DateTime.Now.Ticks + Path.GetExtension(file.FileName);

                file.SaveAs(Path.Combine(Filepath, filename));
                #endregion

                #region Storing file details in database
                var FileFlag = LogRepo.AddFileLog(tktlogID, tktID, OriginalFileName, filename, Path.GetExtension(file.FileName));
                OutMsg = FileFlag > 0 ? InMsg : InMsg + " But not added file details in database"; 
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                OutMsg = InMsg + "File Not uploaded due to: " + ex.Message;
                return false;
            }
        }

        public List<TicketLogs> AllLogs(int tktID)
        {
            return LogRepo.TicketLogs(tktID);
        }
    }
}
