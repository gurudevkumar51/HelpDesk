using HelpDeskEntities.Ticket;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskDAL.DataAccess
{
    public class TicketRepository : BaseRepository
    {
        public int AddNewTicket(Ticket tkt, out string msg)
        {
            var flag = 0;msg = "";
            try
            {
                SqlParameter[] parameters = {
                new SqlParameter("@ModuleID",tkt.TicketModule.ModuleID),
                new SqlParameter("@NatureID",tkt.Nature.NatureID),
                new SqlParameter("@TktDescription",tkt.TicketDescription),
                new SqlParameter("@CreatedBy",tkt.CreatedBy),                
                new SqlParameter("@StatusID",tkt.Status.ID),
                new SqlParameter("@Type","")
            };
                
                flag = ExecuteNonQuery("SP_Manage_Ticket", parameters);
                msg = flag > 0 ? "New Ticket created successfully" : "Unable to create ticket try again later";
            }
            catch (Exception ex)
            {
                msg = "Unable to create due to: " + ex.Message;
                return 0;
            }
            return flag;
        }
    }
}
