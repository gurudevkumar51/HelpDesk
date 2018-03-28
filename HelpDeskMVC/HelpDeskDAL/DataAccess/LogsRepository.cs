using HelpDeskDAL.DataMapper;
using HelpDeskEntities.Ticket;
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

        public int AddFileLog(int TicketLogID,int tktID, string FileNameOriginal, string FileName, string FileExtension, int UploadedBy)
        {
            var InsertedID = 0;
            try
            {
                SqlParameter[] parameters = {
                         new SqlParameter("@id",0),
                         new SqlParameter("@Type", "B"),
                         new SqlParameter("@TicketLogID", TicketLogID),
                         new SqlParameter("@TicketID", tktID),
                         new SqlParameter("@FileName",FileName),
                         new SqlParameter("@FileNameOriginal",FileNameOriginal),
                         new SqlParameter("@FileUploadedBy",UploadedBy),
                         new SqlParameter("@FileExtension", FileExtension)
                         
                };
                parameters[0].Direction = ParameterDirection.Output;

                InsertedID = ExecuteNonQueryWithReturnValue("SP_Manage_Logs", "@id", parameters);                
            }
            catch (Exception ex)
            {
                return 0;
            }
            return InsertedID;
        }

        public List<TicketFile> Files(int tktID)
        {
            try
            {
                FileMapper fileMapper = new FileMapper();
                SqlParameter[] parameters =
                    {
                    new SqlParameter("@Type","C"),
                    new SqlParameter("@TicketID",tktID)
                };

                IDataReader reader = base.GetReader("SP_Manage_Logs", parameters);
                using (reader)
                {
                    return fileMapper.Map(reader);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<TicketFile> TicketLogFiles(int tktID, string UploadDateTime, int UID)
        {
            try
            {
                FileMapper fileMapper = new FileMapper();
                SqlParameter[] parameters =
                    {
                    new SqlParameter("@Type","E"),
                    new SqlParameter("@TicketID",tktID),
                    new SqlParameter("@UploadDateTime",UploadDateTime),
                    new SqlParameter("@FileUploadedBy", UID)
                };

                IDataReader reader = base.GetReader("SP_Manage_Logs", parameters);
                using (reader)
                {
                    return fileMapper.Map(reader);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<TicketLogs> TicketLogs(int tktID)
        {
            try
            {
                TicketLogMapper LogMapper = new TicketLogMapper();
                SqlParameter[] parameters =
                    {
                    new SqlParameter("@Type","D"),
                    new SqlParameter("@TicketID",tktID)
                };

                IDataReader reader = base.GetReader("SP_Manage_Logs", parameters);
                using (reader)
                {
                    return LogMapper.Map(reader);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}