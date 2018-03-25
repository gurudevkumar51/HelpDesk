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
    public class TktCommentRepository : BaseRepository
    {
        public int PostComment(TicketComment tc, out string msg)
        {
            var cmntID = 0; msg = "";
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("@TicketID",tc.TicketID),
                    new SqlParameter("@Comment",tc.Comment),
                    new SqlParameter("@CommentedBy",tc.CommentBy.UID),
                    new SqlParameter("@Type","A"),
                    new SqlParameter("@id",0)
                };
                parameters[4].Direction = ParameterDirection.Output;

                cmntID = ExecuteNonQueryWithReturnValue("SP_Manage_Comment", "@id", parameters);
                msg = cmntID > 0 ? "Comment Posted Successfully" : "Unable to Post Comment";
            }
            catch (Exception ex)
            {
                msg = "Comment Posting Failed Due to " + ex.Message;
                return 0;
            }
            return cmntID;
        }

        public List<TicketComment> CommentList(int TktId)
        {
            CommentMapper mprobj = new CommentMapper();
            SqlParameter[] parameters =
            {
                new SqlParameter("@Type","B"),
                new SqlParameter("@TicketID",TktId)
            };
            IDataReader reader = base.GetReader("SP_Manage_Comment", parameters);
            using (reader)
            {
                return mprobj.Map(reader);
            }
        }
    }
}
