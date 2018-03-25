using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskDAL.DataAccess
{
    public class LogsRepository : BaseRepository
    {
        public int AddTicketLog(int currentUsr, string LogMsg, int TktID, out string msg)
        {
            var InsertedID = 0;
            try
            {
                SqlParameter[] parameters = {
                         new SqlParameter("@Type", "A"),
                         new SqlParameter("@LogDetails", LogMsg),
                         new SqlParameter("@LogBy",currentUsr),
                         new SqlParameter("@TicketID", TktID),
                         new SqlParameter("@id",0)
                };
                parameters[4].Direction = ParameterDirection.Output;

                InsertedID = ExecuteNonQueryWithReturnValue("SP_Manage_Logs", "@id", parameters);
                msg = InsertedID > 0 ? LogMsg : LogMsg + " But Log Not Recorded";
            }
            catch (Exception ex)
            {
                msg = "Unable to add " + ex.Message;
                return InsertedID;
            }
            return InsertedID;
        }

        public int AddFileLog(int TicketLogID, string FileName, string FileExtension)
        {
            var InsertedID = 0;
            try
            {
                SqlParameter[] parameters = {
                         new SqlParameter("@Type", "B"),
                         new SqlParameter("@TicketLogID", TicketLogID),
                         new SqlParameter("@FileName",FileName),
                         new SqlParameter("@FileExtension", FileExtension),
                         new SqlParameter("@id",0)
                };
                parameters[4].Direction = ParameterDirection.Output;

                InsertedID = ExecuteNonQueryWithReturnValue("SP_Manage_Logs", "@id", parameters);                
            }
            catch (Exception ex)
            {
                return 0;
            }
            return InsertedID;
        }
    }
}