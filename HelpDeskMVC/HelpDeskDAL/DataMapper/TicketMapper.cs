using HelpDeskEntities;
using HelpDeskEntities.Ticket;
using System;
using System.Collections.Generic;
using System.Data;

namespace HelpDeskDAL.DataMapper
{
    public class TicketMapper
    {
        public List<Ticket> Map(IDataReader reader)
        {
            List<Ticket> tkts = new List<Ticket>();
            while (reader.Read())
            {
                Ticket tkt = new Ticket();
                tkt.TicketID = Convert.ToInt32(reader["TicketID"] == DBNull.Value ? 0 : reader["TicketID"]);
                tkt.TicketDescription = reader["TktDescription"] == DBNull.Value ? "" : reader["TktDescription"].ToString();
                tkt.TicketModule.ModuleID = Convert.ToInt32(reader["ModuleID"] == DBNull.Value ? 0 : reader["ModuleID"]);
                tkt.TicketModule.Module = reader["ModuleName"] == DBNull.Value ? "" : reader["ModuleName"].ToString();
                tkt.Nature.NatureID = Convert.ToInt32(reader["NatureID"] == DBNull.Value ? 0 : reader["NatureID"]);

                tkt.Nature.Nature = Enum.GetName(typeof(Ticket_Nature), tkt.Nature.NatureID);

                var PriorityID = Convert.ToInt32(reader["PriorityID"] == DBNull.Value ? 0 : reader["PriorityID"]);
                tkt.TicketPriority = Enum.GetName(typeof(Ticket_Priority), PriorityID);
                tkt.CreatedBy = Convert.ToInt32(reader["CreatedBy"] == DBNull.Value ? 0 : reader["CreatedBy"]);
                tkt.CreatedDate = reader["CreatedDate"] == DBNull.Value ? "" : reader["CreatedDate"].ToString();
                tkt.AssignedTo.UID = Convert.ToInt32(reader["AssignedUserID"] == DBNull.Value ? 0 : reader["AssignedUserID"]);
                tkt.AssignedTo.EmailID = reader["AssignedUserEmail"] == DBNull.Value ? "" : reader["AssignedUserEmail"].ToString();
                tkt.AssignedTo.Name = reader["AssignedUserName"] == DBNull.Value ? "Not Assigned" : reader["AssignedUserName"].ToString();//
                tkt.Status.ID = Convert.ToInt32(reader["StatusID"] == DBNull.Value ? 0 : reader["StatusID"]);
                //tkt.Status.Status = Enum.GetName(typeof(Ticket_Status), tkt.Status.ID);
                tkt.Status.Status = reader["TktStatus"] == DBNull.Value ? "" : reader["TktStatus"].ToString();
                //tkt.ClosureComment= reader["ClosureComment"] == DBNull.Value ? "" : reader["ClosureComment"].ToString();
                tkt.CreatedByUser.EmailID = reader["CreatedByEmail"] == DBNull.Value ? "" : reader["CreatedByEmail"].ToString();
                tkt.CreatedByUser.Name = reader["CreatedByName"] == DBNull.Value ? "" : reader["CreatedByName"].ToString();
                tkt.CreatedByUser.UID = tkt.CreatedBy;
                tkts.Add(tkt);
            }
            return tkts;
        }

        public List<TicketCountStatusWise> tktList(IDataReader reader)
        {
            List<TicketCountStatusWise> tkts = new List<TicketCountStatusWise>();
            while (reader.Read())
            {
                TicketCountStatusWise tkt = new TicketCountStatusWise();
                tkt.StatusID = Convert.ToInt32(reader["StatusID"] == DBNull.Value ? 0 : reader["StatusID"]);
                tkt.Count = Convert.ToInt32(reader["TktCount"] == DBNull.Value ? 0 : reader["TktCount"]);
                //tkt.StatusDesc = Enum.GetName(typeof(Ticket_Status), tkt.StatusID);
                tkt.StatusDesc = reader["TktStatus"] == DBNull.Value ? "" : reader["TktStatus"].ToString();
                tkts.Add(tkt);
            }
            return tkts;
        }
    }
}
