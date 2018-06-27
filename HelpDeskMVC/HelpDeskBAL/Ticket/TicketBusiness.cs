using HelpDeskBAL.Module;
using HelpDeskCommon.CommonClasses;
using HelpDeskDAL.DataAccess;
using HelpDeskEntities;
using HelpDeskEntities.Mail;
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
        public List<HelpDeskEntities.Ticket.Ticket> MyActiveTickets()
        {
            return tktRepo.AllTicket(Convert.ToInt32(CurrentUser[2])).Where(t => t.Status.ID != 3).ToList();
        }
        public List<HelpDeskEntities.Ticket.Ticket> MyClosedTickets()
        {
            return tktRepo.AllTicket(Convert.ToInt32(CurrentUser[2])).Where(t => t.Status.ID == 3).ToList();
        }
        public HelpDeskEntities.Ticket.Ticket TicketByTktID(int tktID)
        {
            //var tkt = tktRepo.TicketByID(tktID).Where(t => t.CreatedBy == Convert.ToInt32(CurrentUser[2])).FirstOrDefault();
            var tkt = tktRepo.TicketByID(tktID).FirstOrDefault();
            if (tkt != null)
            {
                tkt.AllFiles = LogRepo.Files(tktID);
                tkt.TktLogs = LogRepo.TicketLogs(tktID);
            }
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

        public Boolean SetTicketPriority(int ticketID, int priorityID, out string msg)
        {
            string msg2 = "";
            var data = tktRepo.SetTicketPriority(ticketID, priorityID, out msg) > 0 ? true : false;
            if (data && priorityID != 0)
            {
                var prio = Enum.GetName(typeof(Ticket_Priority), priorityID);
                LogRepo.AddTicketLog(Convert.ToInt32(CurrentUser[2]), "Set Priority as " + prio, ticketID, out msg2);
                msg = msg + " And " + msg2;
            }
            return data;
        }
        //When-ever we will assign ticket to any user Status will become InProgress status automatically
        public Boolean AssignTicketToUser(int TktID, int AssignedTo, string comment)
        {
            string msg = "";
            User.UserBusiness u = new User.UserBusiness();
            var asigneeUser = u.GetUserByUID(AssignedTo);

            var otpt = LogRepo.AddTicketLog(Convert.ToInt32(CurrentUser[2]), "Assigned to " + asigneeUser.Name, TktID, out msg);
            var flag = otpt > 0 ? true : false;
            if (flag)
            {
                //string mailMsg = "";
                EmailTemplate mailTemp = new EmailTemplate();
                var tkt = tktRepo.TicketByID(TktID).FirstOrDefault();
                mailTemp.Mail_To.Add(tkt.CreatedByUser.EmailID);
                mailTemp.Mail_Cc.Add(asigneeUser.EmailID);
                mailTemp.Mail_Cc.Add(CurrentUser[0]);
                mailTemp.Mail_Subject = "Ticket Assigned";
                mailTemp.Mail_Content = "Hello User,<br /> Ticket ID " + TktID + " Assigned to " + asigneeUser.Name + " Which was created by " + tkt.CreatedByUser.Name;
                //GenericClass.sendMail(mailTemp, out mailMsg);

                tktRepo.AssignTicketToUser(TktID, AssignedTo, Convert.ToInt32(CurrentUser[2]), comment);
                tktRepo.UpdateTicketStatus(TktID, 2);//2 Status id means InProgress
            }
            return flag;
        }

        public Boolean EsclateTicketToUser(int TktID, int AssignedTo, string comment)
        {
            string msg = "";
            User.UserBusiness u = new User.UserBusiness();
            var asigneeUser = u.GetUserByUID(AssignedTo);

            var otpt = LogRepo.AddTicketLog(Convert.ToInt32(CurrentUser[2]), "Esclated to " + u.GetUserByUID(AssignedTo).Name, TktID, out msg);
            var flag = otpt > 0 ? true : false;
            if (flag)
            {
                tktRepo.AssignTicketToUser(TktID, AssignedTo, Convert.ToInt32(CurrentUser[2]), comment);

                EmailTemplate mailTemp = new EmailTemplate();
                var tkt = tktRepo.TicketByID(TktID).FirstOrDefault();
                mailTemp.Mail_To.Add(tkt.CreatedByUser.EmailID);
                mailTemp.Mail_Cc.Add(asigneeUser.EmailID);
                mailTemp.Mail_Cc.Add(CurrentUser[0]);
                mailTemp.Mail_Subject = "Ticket Esclated";
                mailTemp.Mail_Content = "Hello User,<br /> Ticket ID " + TktID + " Esclated to " + asigneeUser.Name + " Which was created by " + tkt.CreatedByUser.Name;
                //string mailMsg = "";
                //GenericClass.sendMail(mailTemp, out mailMsg);
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
                EmailTemplate mailTemp = new EmailTemplate();
                var tkt = tktRepo.TicketByID(tktID).FirstOrDefault();
                mailTemp.Mail_To.Add(tkt.CreatedByUser.EmailID);
                mailTemp.Mail_Cc.Add(CurrentUser[0]);
                mailTemp.Mail_Subject = "Ticket Closed";
                mailTemp.Mail_Content = "Hello User,<br /> Ticket ID " + tktID + " Closed by " + CurrentUser[1] + " Which was created by " + tkt.CreatedByUser.Name;
                //string mailMsg = "";
                //GenericClass.sendMail(mailTemp, out mailMsg);

                var log = LogRepo.AddTicketLog(Convert.ToInt32(CurrentUser[2]), "Closed by " + CurrentUser[1], tktID, out msg);
                if (log > 0)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public Boolean ResolveTicket(int tktID, string comment, HttpPostedFileBase[] files)
        {
            string msg = "";
            var st = tktRepo.UpdateTicketStatus(tktID, 4);//4 Status id means Resolved
            if (st > 0)
            {
                EmailTemplate mailTemp = new EmailTemplate();
                var tkt = tktRepo.TicketByID(tktID).FirstOrDefault();
                mailTemp.Mail_To.Add(tkt.CreatedByUser.EmailID);
                mailTemp.Mail_Cc.Add(CurrentUser[0]);
                mailTemp.Mail_Subject = "Ticket Resolved";
                mailTemp.Mail_Content = "Hello User,<br /> Ticket ID " + tktID + " Resolved by " + CurrentUser[1] + " Which was created by " + tkt.CreatedByUser.Name;
                //string mailMsg = "";
                //GenericClass.sendMail(mailTemp, out mailMsg);

                string ResolveComment = "Resolved by " + CurrentUser[1] + "<br />" + comment;
                var log = LogRepo.AddTicketLog(Convert.ToInt32(CurrentUser[2]), ResolveComment, tktID, out msg);
                if (log > 0)
                {
                    if (files != null)
                    {
                        if (files.Count() > 0)
                        {
                            foreach (HttpPostedFileBase file in files)
                            {
                                var fileFlag = SaveTicketFiles(file, log, tktID, Convert.ToInt32(CurrentUser[2]), msg, out msg);
                            }
                        }
                    }
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
                EmailTemplate mailTemp = new EmailTemplate();
                var tkt = tktRepo.TicketByID(tktID).FirstOrDefault();
                mailTemp.Mail_To.Add(tkt.CreatedByUser.EmailID);
                mailTemp.Mail_Cc.Add(CurrentUser[0]);
                mailTemp.Mail_Subject = "Ticket Reopened";
                mailTemp.Mail_Content = "Hello User,<br /> Ticket ID " + tktID + " Reopened by " + CurrentUser[1] + " Which was created by " + tkt.CreatedByUser.Name;

                //string mailMsg = "";
                //GenericClass.sendMail(mailTemp, out mailMsg);

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
                EmailTemplate mailTemp = new EmailTemplate();
                mailTemp.Mail_To.Add(CurrentUser[0]);
                mailTemp.Mail_Cc.Add(CurrentUser[0]);
                mailTemp.Mail_Subject = "Ticket Created";
                mailTemp.Mail_Content = "Hello User,<br /> New Ticket created by " + CurrentUser[1];

                //string mailMsg = "";
                //GenericClass.sendMail(mailTemp, out mailMsg);

                var tktlogID = LogRepo.AddTicketLog(tkt.CreatedBy, msg, InsertedId, out msg);

                if (tkt.files.Count() > 0 && tktlogID > 0)
                {
                    foreach (HttpPostedFileBase file in tkt.files)
                    {
                        var fileFlag = SaveTicketFiles(file, tktlogID, InsertedId, Convert.ToInt32(CurrentUser[2]), msg, out msg);
                    }
                }
            }
            #endregion

            return InsertedId;
        }

        public Boolean UpdateTicket(HelpDeskEntities.Ticket.Ticket tkt, out string msg)
        {
            var flag = tktRepo.UpdateTicket(tkt, out msg) > 0 ? true : false;
            if (flag)
            {
                var otpt = LogRepo.AddTicketLog(Convert.ToInt32(CurrentUser[2]), "Ticket updated", tkt.TicketID, out msg);
            }
            return flag;
        }

        private Boolean SaveTicketFiles(HttpPostedFileBase file, int tktlogID, int tktID, int CurrentUser, string InMsg, out string OutMsg)
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
                var FileFlag = LogRepo.AddFileLog(tktlogID, tktID, OriginalFileName, filename, Path.GetExtension(file.FileName), CurrentUser);
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
            var LogData = LogRepo.TicketLogs(tktID);
            return LogData;
        }


        public List<TicketCountStatusWise> DashboardFlagData()
        {
            var data = tktRepo.DashboardFlagData();
            
            return data;
        }
    }
}