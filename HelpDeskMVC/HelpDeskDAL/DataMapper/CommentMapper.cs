using HelpDeskEntities.Ticket;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskDAL.DataMapper
{
  public  class CommentMapper
    {
        public List<TicketComment> Map(IDataReader reader)
        {
            List<TicketComment> cmnts = new List<TicketComment>();
            while (reader.Read())
            {
                TicketComment cmnt = new TicketComment();
                cmnt.CommentID = Convert.ToInt32(reader["CommentID"] == DBNull.Value ? 0 : reader["CommentID"]);
                cmnt.Comment = reader["Comment"] == DBNull.Value ? "" : reader["Comment"].ToString();
                cmnt.CommentDate = reader["CommentDate"] == DBNull.Value ? "" : reader["CommentDate"].ToString();
                cmnt.CommentBy.UID = Convert.ToInt32(reader["CommentedBy"] == DBNull.Value ? 0 : reader["CommentedBy"]);
                cmnt.CommentBy.Name = reader["Name"] == DBNull.Value ? "" : reader["Name"].ToString();
                cmnt.CommentBy.EmailID = reader["Email"] == DBNull.Value ? "" : reader["Email"].ToString();
                cmnts.Add(cmnt);
            }
            return cmnts;
        }
    }
}
