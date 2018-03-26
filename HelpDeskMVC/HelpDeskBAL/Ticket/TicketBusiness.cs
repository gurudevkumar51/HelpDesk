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
        private List<HelpDeskEntities.Modules.Modules> UserModuleList = new List<HelpDeskEntities.Modules.Modules>();
        private string role = HttpContext.Current.User.IsInRole("admin") ? "admin" : (HttpContext.Current.User.IsInRole("HelpdeskUser") ? "HelpdeskUser" : (HttpContext.Current.User.IsInRole("SupeUser") ? "SupeUser" : (HttpContext.Current.User.IsInRole("SupportStaff") ? "SupportStaff" : "EndUser")));
        private string[] CurrentUser = GenericClass.CsvToStringArray(HttpContext.Current.User.Identity.Name);
        

        private List<HelpDeskEntities.Ticket.Ticket> AllTicket()
        {
            if (role == "EndUser")
            {
                return tktRepo.AllTicket(Convert.ToInt32(CurrentUser[2]));
            }
            else
            {
                var TktData = tktRepo.AllTicket(null);
                if (role == "admin")
                {
                    return TktData;
                }
                else if (role == "HelpdeskUser" || role == "SupeUser")
                {
                    ModuleBAL mb = new ModuleBAL();
                    UserModuleList = mb.ModuleListForUser(Convert.ToInt32(CurrentUser[2]));
                    return TktData.Where(t => UserModuleList.Any(m => m.ModuleID == t.TicketModule.ModuleID)).ToList();
                }
                else if (role == "SupportStaff")
                {
                    return TktData.Where(t => t.AssignedTo.UID == Convert.ToInt32(CurrentUser[2])).ToList();
                }
                else
                {
                    List<HelpDeskEntities.Ticket.Ticket> tkts = new List<HelpDeskEntities.Ticket.Ticket>();
                    return tkts;
                }
            }
        }

        #region Ticket Listing
        public List< HelpDeskEntities.Ticket.Ticket> MyActiveTickets()
        {            
            return tktRepo.AllTicket(Convert.ToInt32(CurrentUser[2])).Where(t => t.Status.ID != 3).ToList();
        }
        public List<HelpDeskEntities.Ticket.Ticket> MyClosedTickets()
        {
            return tktRepo.AllTicket(Convert.ToInt32(CurrentUser[2])).Where(t => t.Status.ID == 3).ToList();
        }
        public HelpDeskEntities.Ticket.Ticket TicketByTktID(int tktID)
        {
            var tkt = tktRepo.TicketByID(tktID).Where(t => t.CreatedBy == Convert.ToInt32(CurrentUser[2])).FirstOrDefault();
            //if(tkt != null)
            //{
                tkt.AllFiles = LogRepo.Files(tktID);
                tkt.TktLogs = LogRepo.TicketLogs(tktID);
            //}
            return tkt;
        }
        public List<HelpDeskEntities.Ticket.Ticket> AllActiceTickets()
        {
            return AllTicket().Where(t => t.Status.ID != 3).ToList();
        }
        public List<HelpDeskEntities.Ticket.Ticket> AllClosedTickets()
        {
            return AllTicket().Where(t => t.Status.ID == 3).ToList();
        }
        public List<HelpDeskEntities.Ticket.Ticket> TicketsByCreatedByCurrentUser()
        {
            return tktRepo.AllTicket(Convert.ToInt32(CurrentUser[2]));
        }
        #endregion

        #region Ticket Operations
        //When-ever we will assign ticket to any user Status will become InProgress status automatically
        public Boolean AssignTicketToUser(int TktID, int AssignedTo, string comment)
        {
            string msg = "";
            User.UserBusiness u = new User.UserBusiness();

            var otpt = LogRepo.AddTicketLog(Convert.ToInt32(CurrentUser[2]), "Assigned to " + u.GetUserByUID(AssignedTo).Name, TktID, out msg);
            var flag = otpt > 0 ? true : false;
            if (flag)
            {
                tktRepo.AssignTicketToUser(TktID, AssignedTo, Convert.ToInt32(CurrentUser[2]), comment);
                tktRepo.UpdateTicketStatus(TktID, 2);//2 Status id means InProgress
            }
            return flag;
        }

        public Boolean EsclateTicketToUser(int TktID, int AssignedTo, string comment)
        {
            string msg = "";
            User.UserBusiness u = new User.UserBusiness();

            var otpt = LogRepo.AddTicketLog(Convert.ToInt32(CurrentUser[2]), "Esclated to " + u.GetUserByUID(AssignedTo).Name, TktID, out msg);
            var flag = otpt > 0 ? true : false;
            if (flag)
            {
                tktRepo.AssignTicketToUser(TktID, AssignedTo, Convert.ToInt32(CurrentUser[2]), comment);
                //tktRepo.UpdateTicketStatus(TktID, 2);//2 Status id means InProgress
            }
            return flag;
        }

        public Boolean CloseTicket(int tktID)
        {
            string msg = "";
            var st = tktRepo.UpdateTicketStatus(tktID, 3);//3 Status id means Closed
            if (st > 0)
            {
                var log = LogRepo.AddTicketLog(Convert.ToInt32(CurrentUser[2]), "Closed by " + CurrentUser[1], tktID, out msg);
                if (log > 0)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public Boolean ResolveTicket(int tktID)
        {
            string msg = "";
            var st = tktRepo.UpdateTicketStatus(tktID, 4);//4 Status id means Resolved
            if (st > 0)
            {
                var log = LogRepo.AddTicketLog(Convert.ToInt32(CurrentUser[2]), "Resolved by " + CurrentUser[1], tktID, out msg);
                if (log > 0)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public Boolean ReopenTicket(int tktID)
        {
            string msg = "";
            var st = tktRepo.UpdateTicketStatus(tktID, 1);//1 Status id means Open
            if (st > 0)
            {
                var log = LogRepo.AddTicketLog(Convert.ToInt32(CurrentUser[2]), "Reopened by " + CurrentUser[1], tktID, out msg);
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
            tkt.CreatedBy = Convert.ToInt32(CurrentUser[2]);
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

        private Boolean SaveTicketFiles(HttpPostedFileBase file, int tktlogID, int tktID, string InMsg, out string OutMsg)
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
        #endregion
        
        public List<TicketLogs> AllLogs(int tktID)
        {
            return LogRepo.TicketLogs(tktID);
        }
    }
}
