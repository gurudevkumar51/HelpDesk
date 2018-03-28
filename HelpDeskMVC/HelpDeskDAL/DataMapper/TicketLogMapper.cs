using HelpDeskDAL.DataAccess;
using HelpDeskEntities.Ticket;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskDAL.DataMapper
{
    public class TicketLogMapper
    {
        //LogType =1 means its a log, LogType=2 means its a escalation
     private   LogsRepository lgRepo = new LogsRepository();
        public List<TicketLogs> Map(IDataReader reader)
        {
            List<TicketLogs> logs = new List<TicketLogs>();
            while (reader.Read())
            {
                TicketLogs log = new TicketLogs();
                log.TicketID = Convert.ToInt32(reader["TicketID"] == DBNull.Value ? 0 : reader["TicketID"]);
                log.LogDateTime = reader["LogDate"] == DBNull.Value ? "" : reader["LogDate"].ToString();
                log.LogContent = reader["Content"] == DBNull.Value ? "" : reader["Content"].ToString();
                log.LogTypeID= Convert.ToInt32(reader["LogType"] == DBNull.Value ? 0 : reader["LogType"]);
                log.LogBy.UID = Convert.ToInt32(reader["LogBy"] == DBNull.Value ? 0 : reader["LogBy"]);
                log.LogBy.Name = reader["LogByName"] == DBNull.Value ? "" : reader["LogByName"].ToString();
                log.LogBy.EmailID= reader["LogByEmail"] == DBNull.Value ? "" : reader["LogByEmail"].ToString();
                log.Logtype = log.LogTypeID == 1 ? "Status Log" : "Esclation Log";
                if (log.LogTypeID == 2)
                {
                    log.LogFor.Name = reader["LogForName"] == DBNull.Value ? "" : reader["LogForName"].ToString();
                    log.LogFor.EmailID = reader["LogForEmail"] == DBNull.Value ? "" : reader["LogForEmail"].ToString();
                }
                log.Files = lgRepo.TicketLogFiles(log.TicketID, log.LogDateTime, log.LogBy.UID);
                logs.Add(log);
            }
            return logs;
        }
    }
}
