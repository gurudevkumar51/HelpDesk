using HelpDeskEntities.Ticket;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskDAL.DataMapper
{
    public class FileMapper
    {
        public List<TicketFile> Map(IDataReader reader)
        {
            List<TicketFile> files = new List<TicketFile>();
            while (reader.Read())
            {
                TicketFile file = new TicketFile();
                file.FileID = Convert.ToInt32(reader["FileID"] == DBNull.Value ? 0 : reader["FileID"]);
                file.FileDisplayName = reader["FileNameOriginal"] == DBNull.Value ? "" : reader["FileNameOriginal"].ToString();
                file.FileName = reader["FileName"] == DBNull.Value ? "" : reader["FileName"].ToString();
                file.FileExtension = reader["FileExtension"] == DBNull.Value ? "" : reader["FileExtension"].ToString();
                file.UploadDate = reader["UploadDate"] == DBNull.Value ? "" : reader["UploadDate"].ToString();
                file.TicketId = Convert.ToInt32(reader["TicketID"] == DBNull.Value ? 0 : reader["TicketID"]);
                files.Add(file);
            }
            return files;
        }
    }
}
